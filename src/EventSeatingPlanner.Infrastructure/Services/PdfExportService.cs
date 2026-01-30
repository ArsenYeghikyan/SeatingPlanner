using System.Text;
using EventSeatingPlanner.Application.Interfaces.Repositories;
using EventSeatingPlanner.Application.Interfaces.Services;

namespace EventSeatingPlanner.Infrastructure.Services;

public sealed class PdfExportService : IPdfExportService
{
    private readonly IEventRepository _eventRepository;
    private readonly ITableRepository _tableRepository;
    private readonly IGuestRepository _guestRepository;
    private readonly IAssignmentRepository _assignmentRepository;
    private readonly IPrintSettingsRepository _printSettingsRepository;
    private readonly IAssetRepository _assetRepository;

    public PdfExportService(
        IEventRepository eventRepository,
        ITableRepository tableRepository,
        IGuestRepository guestRepository,
        IAssignmentRepository assignmentRepository,
        IPrintSettingsRepository printSettingsRepository,
        IAssetRepository assetRepository)
    {
        _eventRepository = eventRepository;
        _tableRepository = tableRepository;
        _guestRepository = guestRepository;
        _assignmentRepository = assignmentRepository;
        _printSettingsRepository = printSettingsRepository;
        _assetRepository = assetRepository;
    }

    public async Task<byte[]> ExportSeatingPlanAsync(Guid eventId, CancellationToken cancellationToken)
    {
        var @event = await _eventRepository.GetByIdAsync(eventId, cancellationToken);
        if (@event is null)
        {
            return BuildPdf(new[] { "Мероприятие не найдено." });
        }

        var tables = await _tableRepository.ListByEventAsync(eventId, cancellationToken);
        var guests = await _guestRepository.ListByEventAsync(eventId, cancellationToken);
        var assignments = await _assignmentRepository.ListByEventAsync(eventId, cancellationToken);

        var printSettings = await _printSettingsRepository.GetByEventIdAsync(eventId, cancellationToken);
        var backgroundName = "—";
        if (printSettings?.BackgroundAssetId is not null)
        {
            var asset = await _assetRepository.GetByIdAsync(printSettings.BackgroundAssetId.Value, cancellationToken);
            backgroundName = asset?.FileName ?? backgroundName;
        }

        var lines = new List<string>
        {
            $"Мероприятие: {@event.Title}",
            $"Дата: {@event.Date:dd.MM.yyyy}",
            $"Локация: {@event.Location ?? "—"}",
            $"Шрифт: {printSettings?.FontKey ?? "Default"}",
            $"Размер заголовка: {printSettings?.TitleFontSize ?? 24}",
            $"Размер текста: {printSettings?.BodyFontSize ?? 12}",
            $"Цвет текста: {printSettings?.TextColorHex ?? "#000000"}",
            $"Фон: {backgroundName}",
            string.Empty,
            "Рассадка:"
        };

        foreach (var table in tables.OrderBy(t => t.SortOrder))
        {
            lines.Add($"Стол: {table.Name} (мест: {table.Capacity})");
            var tableAssignments = assignments
                .Where(a => a.TableId == table.Id)
                .OrderBy(a => a.SeatNumber ?? int.MaxValue)
                .ToList();

            if (tableAssignments.Count == 0)
            {
                lines.Add("  — Нет гостей");
                continue;
            }

            foreach (var assignment in tableAssignments)
            {
                var guest = guests.FirstOrDefault(g => g.Id == assignment.GuestId);
                var seatLabel = assignment.SeatNumber is null ? string.Empty : $" (место {assignment.SeatNumber})";
                lines.Add($"  • {guest?.FullName ?? "Неизвестный гость"}{seatLabel}");
            }
        }

        return BuildPdf(lines);
    }

    private static byte[] BuildPdf(IEnumerable<string> lines)
    {
        var contentStream = BuildContentStream(lines);
        var objects = new List<string>
        {
            "<< /Type /Catalog /Pages 2 0 R >>",
            "<< /Type /Pages /Kids [3 0 R] /Count 1 >>",
            "<< /Type /Page /Parent 2 0 R /MediaBox [0 0 612 792] /Contents 4 0 R /Resources << /Font << /F1 5 0 R >> >> >>",
            $"<< /Length {contentStream.Length} >>\nstream\n{contentStream}\nendstream",
            "<< /Type /Font /Subtype /Type1 /BaseFont /Helvetica >>"
        };

        using var stream = new MemoryStream();
        using var writer = new StreamWriter(stream, Encoding.ASCII, leaveOpen: true);

        writer.WriteLine("%PDF-1.4");

        var offsets = new List<long> { 0 };
        for (var i = 0; i < objects.Count; i++)
        {
            offsets.Add(stream.Position);
            var objectId = i + 1;
            writer.WriteLine($"{objectId} 0 obj");
            writer.WriteLine(objects[i]);
            writer.WriteLine("endobj");
        }

        writer.Flush();
        var xrefPosition = stream.Position;
        writer.WriteLine("xref");
        writer.WriteLine($"0 {offsets.Count}");
        writer.WriteLine("0000000000 65535 f ");

        for (var i = 1; i < offsets.Count; i++)
        {
            writer.WriteLine($"{offsets[i]:0000000000} 00000 n ");
        }

        writer.WriteLine("trailer");
        writer.WriteLine("<< /Size 6 /Root 1 0 R >>");
        writer.WriteLine("startxref");
        writer.WriteLine(xrefPosition);
        writer.WriteLine("%%EOF");
        writer.Flush();

        return stream.ToArray();
    }

    private static string BuildContentStream(IEnumerable<string> lines)
    {
        var builder = new StringBuilder();
        builder.AppendLine("BT");
        builder.AppendLine("/F1 12 Tf");
        builder.AppendLine("14 TL");
        builder.AppendLine("50 750 Td");

        var lineList = lines.ToList();
        for (var i = 0; i < lineList.Count; i++)
        {
            builder.AppendLine($"({Escape(lineList[i])}) Tj");
            if (i != lineList.Count - 1)
            {
                builder.AppendLine("T*");
            }
        }

        builder.AppendLine("ET");
        return builder.ToString();
    }

    private static string Escape(string value)
    {
        return value.Replace("\\", "\\\\", StringComparison.Ordinal)
            .Replace("(", "\\(", StringComparison.Ordinal)
            .Replace(")", "\\)", StringComparison.Ordinal);
    }
}
