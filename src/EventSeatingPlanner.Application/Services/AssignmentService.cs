using EventSeatingPlanner.Application.DTOs.Assignments;
using EventSeatingPlanner.Application.Entities;
using EventSeatingPlanner.Application.Interfaces.Repositories;
using EventSeatingPlanner.Application.Interfaces.Services;

namespace EventSeatingPlanner.Application.Services;

public sealed class AssignmentService(
    IAssignmentRepository assignmentRepository,
    ITableRepository tableRepository,
    IGuestRepository guestRepository) : IAssignmentService
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
        var tables = await tableRepository.ListByEventAsync(eventId, cancellationToken);
        var table = tables.FirstOrDefault(t => t.Id == request.TableId);
        if (table is null)
        {
            throw new InvalidOperationException("Стол не найден.");
        }

        var guests = await guestRepository.ListByEventAsync(eventId, cancellationToken);
        var guest = guests.FirstOrDefault(g => g.Id == request.GuestId);
        if (guest is null)
        {
            throw new InvalidOperationException("Гость не найден.");
        }

        var assignments = await assignmentRepository.ListByEventAsync(eventId, cancellationToken);
        if (assignments.Any(a => a.GuestId == request.GuestId))
        {
            throw new InvalidOperationException("Гость уже назначен за стол.");
        }

        var tableAssignments = assignments.Where(a => a.TableId == request.TableId).ToList();
        if (tableAssignments.Count >= table.Capacity)
        {
            throw new InvalidOperationException("Вместимость стола превышена.");
        }

        if (request.SeatNumber is not null)
        {
            if (request.SeatNumber <= 0 || request.SeatNumber > table.Capacity)
            {
                throw new InvalidOperationException("Номер места выходит за пределы вместимости стола.");
            }

            if (tableAssignments.Any(a => a.SeatNumber == request.SeatNumber))
            {
                throw new InvalidOperationException("Место уже занято.");
            }
        }

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

    public async Task DeleteAsync(Guid eventId, Guid assignmentId, CancellationToken cancellationToken)
    {
        var assignments = await assignmentRepository.ListByEventAsync(eventId, cancellationToken);
        var assignment = assignments.FirstOrDefault(item => item.Id == assignmentId);
        if (assignment is null)
        {
            throw new InvalidOperationException("Рассадка не найдена.");
        }

        await assignmentRepository.DeleteAsync(assignment, cancellationToken);
    }
}
