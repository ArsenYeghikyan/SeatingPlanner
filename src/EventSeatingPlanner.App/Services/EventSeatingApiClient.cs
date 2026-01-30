using System.Net.Http.Json;
using EventSeatingPlanner.App.Models;
using Microsoft.AspNetCore.Components.Forms;

namespace EventSeatingPlanner.App.Services;

public sealed class EventSeatingApiClient
{
    private readonly HttpClient _httpClient;

    public EventSeatingApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IReadOnlyList<EventSummaryDto>> GetEventsAsync(CancellationToken cancellationToken = default)
    {
        return await _httpClient.GetFromJsonAsync<IReadOnlyList<EventSummaryDto>>("api/events", cancellationToken)
            ?? Array.Empty<EventSummaryDto>();
    }

    public Task<EventDetailDto?> GetEventAsync(Guid eventId, CancellationToken cancellationToken = default)
    {
        return _httpClient.GetFromJsonAsync<EventDetailDto>($"api/events/{eventId}", cancellationToken);
    }

    public async Task<EventDetailDto> CreateEventAsync(CreateEventRequest request, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PostAsJsonAsync("api/events", request, cancellationToken);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<EventDetailDto>(cancellationToken: cancellationToken)
            ?? throw new InvalidOperationException("Не удалось создать мероприятие.");
    }

    public async Task<IReadOnlyList<TableDto>> GetTablesAsync(Guid eventId, CancellationToken cancellationToken = default)
    {
        return await _httpClient.GetFromJsonAsync<IReadOnlyList<TableDto>>($"api/events/{eventId}/tables", cancellationToken)
            ?? Array.Empty<TableDto>();
    }

    public async Task<TableDto> CreateTableAsync(Guid eventId, CreateTableRequest request, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PostAsJsonAsync($"api/events/{eventId}/tables", request, cancellationToken);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<TableDto>(cancellationToken: cancellationToken)
            ?? throw new InvalidOperationException("Не удалось создать стол.");
    }

    public async Task<IReadOnlyList<GuestDto>> GetGuestsAsync(Guid eventId, CancellationToken cancellationToken = default)
    {
        return await _httpClient.GetFromJsonAsync<IReadOnlyList<GuestDto>>($"api/events/{eventId}/guests", cancellationToken)
            ?? Array.Empty<GuestDto>();
    }

    public async Task<GuestDto> CreateGuestAsync(Guid eventId, CreateGuestRequest request, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PostAsJsonAsync($"api/events/{eventId}/guests", request, cancellationToken);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<GuestDto>(cancellationToken: cancellationToken)
            ?? throw new InvalidOperationException("Не удалось создать гостя.");
    }

    public async Task<IReadOnlyList<AssignmentDto>> GetAssignmentsAsync(Guid eventId, CancellationToken cancellationToken = default)
    {
        return await _httpClient.GetFromJsonAsync<IReadOnlyList<AssignmentDto>>($"api/events/{eventId}/assignments", cancellationToken)
            ?? Array.Empty<AssignmentDto>();
    }

    public async Task<AssignmentDto> CreateAssignmentAsync(
        Guid eventId,
        CreateAssignmentRequest request,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PostAsJsonAsync($"api/events/{eventId}/assignments", request, cancellationToken);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<AssignmentDto>(cancellationToken: cancellationToken)
            ?? throw new InvalidOperationException("Не удалось назначить гостя.");
    }

    public async Task DeleteAssignmentAsync(Guid eventId, Guid assignmentId, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.DeleteAsync($"api/events/{eventId}/assignments/{assignmentId}", cancellationToken);
        response.EnsureSuccessStatusCode();
    }

    public Task<PrintSettingsDto?> GetPrintSettingsAsync(Guid eventId, CancellationToken cancellationToken = default)
    {
        return _httpClient.GetFromJsonAsync<PrintSettingsDto>($"api/events/{eventId}/print-settings", cancellationToken);
    }

    public async Task<IReadOnlyList<AssetDto>> GetAssetsAsync(Guid eventId, CancellationToken cancellationToken = default)
    {
        return await _httpClient.GetFromJsonAsync<IReadOnlyList<AssetDto>>($"api/events/{eventId}/assets", cancellationToken)
            ?? Array.Empty<AssetDto>();
    }

    public async Task<AssetDto> UploadAssetAsync(
        Guid eventId,
        string assetType,
        IBrowserFile file,
        CancellationToken cancellationToken = default)
    {
        await using var stream = file.OpenReadStream(maxAllowedSize: 10 * 1024 * 1024, cancellationToken);
        using var content = new MultipartFormDataContent();
        var fileContent = new StreamContent(stream);
        fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);
        content.Add(fileContent, "file", file.Name);

        var response = await _httpClient.PostAsync($"api/events/{eventId}/assets/{assetType}", content, cancellationToken);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<AssetDto>(cancellationToken: cancellationToken)
            ?? throw new InvalidOperationException("Не удалось загрузить файл.");
    }

    public async Task<PrintSettingsDto> UpdatePrintSettingsAsync(
        Guid eventId,
        UpdatePrintSettingsRequest request,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/events/{eventId}/print-settings", request, cancellationToken);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<PrintSettingsDto>(cancellationToken: cancellationToken)
            ?? throw new InvalidOperationException("Не удалось обновить настройки печати.");
    }
}
