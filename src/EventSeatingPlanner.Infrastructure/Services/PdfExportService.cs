using EventSeatingPlanner.Application.Interfaces.Repositories;
using EventSeatingPlanner.Application.Interfaces.Services;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace EventSeatingPlanner.Infrastructure.Services;

public sealed class PdfExportService : IPdfExportService
{
    private readonly IEventRepository _eventRepository;
    private readonly ITableRepository _tableRepository;
    private readonly IGuestRepository _guestRepository;
    private readonly IAssignmentRepository _assignmentRepository;
    private readonly IPrintSettingsRepository _printSettingsRepository;
    private readonly IAssetRepository _assetRepository;
    private readonly IAssetStorage _assetStorage;

    public PdfExportService(
        IEventRepository eventRepository,
        ITableRepository tableRepository,
        IGuestRepository guestRepository,
        IAssignmentRepository assignmentRepository,
        IPrintSettingsRepository printSettingsRepository,
        IAssetRepository assetRepository,
        IAssetStorage assetStorage)
    {
        _eventRepository = eventRepository;
        _tableRepository = tableRepository;
        _guestRepository = guestRepository;
        _assignmentRepository = assignmentRepository;
        _printSettingsRepository = printSettingsRepository;
        _assetRepository = assetRepository;
        _assetStorage = assetStorage;
    }

    public async Task<byte[]> ExportSeatingPlanAsync(Guid eventId, CancellationToken cancellationToken)
    {
        var @event = await _eventRepository.GetByIdAsync(eventId, cancellationToken);
        if (@event is null)
        {
            return BuildPdf(document =>
            {
                document.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(32);
                    page.Content().Text("Мероприятие не найдено.").FontSize(14);
                });
            });
        }

        var tables = await _tableRepository.ListByEventAsync(eventId, cancellationToken);
        var guests = await _guestRepository.ListByEventAsync(eventId, cancellationToken);
        var assignments = await _assignmentRepository.ListByEventAsync(eventId, cancellationToken);

        var printSettings = await _printSettingsRepository.GetByEventIdAsync(eventId, cancellationToken);
        var backgroundImage = await ResolveBackgroundAsync(printSettings?.BackgroundAssetId, cancellationToken);
        var titleFontSize = printSettings?.TitleFontSize ?? 24;
        var bodyFontSize = printSettings?.BodyFontSize ?? 12;
        var textColor = ResolveTextColor(printSettings?.TextColorHex);

        return BuildPdf(document =>
        {
            document.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(32);

                if (backgroundImage is not null)
                {
                    page.Background()
                        .Image(backgroundImage)
                        .FitArea();
                }


                page.Content().Column(column =>
                {
                    column.Spacing(6);

                    column.Item().Text($"Мероприятие: {@event.Title}").FontSize(titleFontSize).SemiBold().FontColor(textColor);
                    column.Item().Text($"Дата: {@event.Date:dd.MM.yyyy}").FontSize(bodyFontSize).FontColor(textColor);
                    column.Item().Text($"Локация: {@event.Location ?? "—"}").FontSize(bodyFontSize).FontColor(textColor);
                    //column.Item().Text($"Шрифт: {printSettings?.FontKey ?? "Default"}").FontSize(bodyFontSize).FontColor(textColor);
                    //column.Item().Text($"Размер заголовка: {titleFontSize}").FontSize(bodyFontSize).FontColor(textColor);
                    //column.Item().Text($"Размер текста: {bodyFontSize}").FontSize(bodyFontSize).FontColor(textColor);
                    //column.Item().Text($"Цвет текста: {printSettings?.TextColorHex ?? "#000000"}").FontSize(bodyFontSize).FontColor(textColor);

                    column.Item().PaddingTop(8).Text("Рассадка:").FontSize(titleFontSize).SemiBold().FontColor(textColor);

                    foreach (var table in tables.OrderBy(t => t.SortOrder))
                    {
                        column.Item().Text($"Стол: {table.Name} (мест: {table.Capacity})")
                            .FontSize(bodyFontSize + 2)
                            .SemiBold()
                            .FontColor(textColor);

                        var tableAssignments = assignments
                            .Where(a => a.TableId == table.Id)
                            .OrderBy(a => a.SeatNumber ?? int.MaxValue)
                            .ToList();

                        if (tableAssignments.Count == 0)
                        {
                            column.Item().PaddingLeft(12).Text("— Нет гостей").FontSize(bodyFontSize).FontColor(textColor);
                            continue;
                        }

                        foreach (var assignment in tableAssignments)
                        {
                            var guest = guests.FirstOrDefault(g => g.Id == assignment.GuestId);
                            var seatLabel = assignment.SeatNumber is null ? string.Empty : $" (место {assignment.SeatNumber})";
                            column.Item().PaddingLeft(12)
                                .Text($"• {guest?.FullName ?? "Неизвестный гость"}{seatLabel}")
                                .FontSize(bodyFontSize)
                                .FontColor(textColor);
                        }
                    }
                });
            });
        });
    }

    private async Task<byte[]?> ResolveBackgroundAsync(Guid? backgroundAssetId, CancellationToken cancellationToken)
    {
        if (backgroundAssetId is null)
        {
            return null;
        }

        var asset = await _assetRepository.GetByIdAsync(backgroundAssetId.Value, cancellationToken);
        if (asset is null || !asset.ContentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }

        var content = await _assetStorage.GetAsync(asset.StoragePath, cancellationToken);
        return content?.Content;
    }

    private static Color ResolveTextColor(string? hexValue)
    {
        if (string.IsNullOrWhiteSpace(hexValue))
        {
            return Colors.Black;
        }

        var normalized = hexValue.Trim();
        if (!normalized.StartsWith("#", StringComparison.Ordinal))
        {
            normalized = $"#{normalized}";
        }

        if (normalized.Length != 7 || normalized[0] != '#')
        {
            return Colors.Black;
        }

        for (var i = 1; i < normalized.Length; i++)
        {
            if (!Uri.IsHexDigit(normalized[i]))
            {
                return Colors.Black;
            }
        }

        return Color.FromHex(normalized);
    }

    private static byte[] BuildPdf(Action<IDocumentContainer> build)
    {
        var document = Document.Create(build);
        return document.GeneratePdf();
    }
}
