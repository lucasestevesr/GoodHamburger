using System.Text.Json;

namespace GoodHamburger.Web.Infrastructure.Http;

internal static class ApiErrorReader
{
    public static string Read(string content, string? fallback)
    {
        if (string.IsNullOrWhiteSpace(content))
            return fallback ?? "Erro ao chamar a API.";

        try
        {
            using var document = JsonDocument.Parse(content);
            var root = document.RootElement;

            var title = root.TryGetProperty("title", out var titleProperty)
                ? titleProperty.GetString()
                : null;

            var detail = root.TryGetProperty("detail", out var detailProperty)
                ? detailProperty.GetString()
                : null;

            var validationMessages = ReadValidationMessages(root);
            if (validationMessages.Count > 0)
                return string.Join(" ", validationMessages);

            return string.Join(" ", new[] { title, detail }.Where(value => !string.IsNullOrWhiteSpace(value)));
        }
        catch (JsonException)
        {
            return content;
        }
    }

    private static List<string> ReadValidationMessages(JsonElement root)
    {
        var messages = new List<string>();
        if (!root.TryGetProperty("errors", out var errorsProperty) || errorsProperty.ValueKind != JsonValueKind.Object)
            return messages;

        foreach (var error in errorsProperty.EnumerateObject())
        {
            if (error.Value.ValueKind != JsonValueKind.Array)
                continue;

            messages.AddRange(error.Value
                .EnumerateArray()
                .Select(item => item.GetString())
                .Where(message => !string.IsNullOrWhiteSpace(message))!);
        }

        return messages;
    }
}
