using EventSeatingPlanner.Application.DTOs.Assignments;
using EventSeatingPlanner.Application.Entities;
using EventSeatingPlanner.Application.Interfaces.Repositories;
using EventSeatingPlanner.Application.Interfaces.Services;

namespace EventSeatingPlanner.Application.Services;

public sealed class AssignmentService(IAssignmentRepository assignmentRepository) : IAssignmentService
{
    public async Task<IReadOnlyList<AssignmentDto>> ListAsync(Guid eventId, CancellationToken cancellationToken)
    {
        var assignments = await assignmentRepository.ListByEventAsync(eventId, cancellationToken);

        return assignments
            .Select(assignment => new AssignmentDto(
                assignment.Id,
                assignment.EventId,
                assignment.TableId,
                assignment.GuestId,
                assignment.SeatNumber))
            .ToList();
    }

    public async Task<AssignmentDto> CreateAsync(
        Guid eventId,
        CreateAssignmentRequest request,
        CancellationToken cancellationToken)
    {
        var assignment = new Assignment
        {
            Id = Guid.NewGuid(),
            EventId = eventId,
            TableId = request.TableId,
            GuestId = request.GuestId,
            SeatNumber = request.SeatNumber
        };

        await assignmentRepository.AddAsync(assignment, cancellationToken);

        return new AssignmentDto(
            assignment.Id,
            assignment.EventId,
            assignment.TableId,
            assignment.GuestId,
            assignment.SeatNumber);
    }
}
