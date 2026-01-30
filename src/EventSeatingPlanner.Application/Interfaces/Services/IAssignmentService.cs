using EventSeatingPlanner.Application.DTOs.Assignments;

namespace EventSeatingPlanner.Application.Interfaces.Services;

public interface IAssignmentService
{
    Task<IReadOnlyList<AssignmentDto>> ListAsync(Guid eventId, CancellationToken cancellationToken);
    Task<AssignmentDto> CreateAsync(Guid eventId, CreateAssignmentRequest request, CancellationToken cancellationToken);
}
