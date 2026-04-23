using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using GoodHamburger.Web.Security;

namespace GoodHamburger.Web.Infrastructure.Http;

public sealed class ApiHttpClient(HttpClient httpClient, AuthSession authSession)
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        Converters = { new JsonStringEnumConverter() }
    };

    public Task<T> GetAsync<T>(string path, CancellationToken ct = default)
    {
        return SendAsync<T>(HttpMethod.Get, path, null, true, ct);
    }

    public Task<T> GetAnonymousAsync<T>(string path, CancellationToken ct = default)
    {
        return SendAsync<T>(HttpMethod.Get, path, null, false, ct);
    }

    public Task<T> PostAnonymousAsync<T>(string path, object body, CancellationToken ct = default)
    {
        return SendAsync<T>(HttpMethod.Post, path, body, false, ct);
    }

    public Task<T> PostAsync<T>(string path, object body, CancellationToken ct = default)
    {
        return SendAsync<T>(HttpMethod.Post, path, body, true, ct);
    }

    public Task<T> PutAsync<T>(string path, object body, CancellationToken ct = default)
    {
        return SendAsync<T>(HttpMethod.Put, path, body, true, ct);
    }

    public Task<T> PatchAsync<T>(string path, object body, CancellationToken ct = default)
    {
        return SendAsync<T>(HttpMethod.Patch, path, body, true, ct);
    }

    public Task DeleteAsync(string path, CancellationToken ct = default)
    {
        return SendAsync(HttpMethod.Delete, path, null, true, ct);
    }

    private async Task<T> SendAsync<T>(HttpMethod method, string path, object? body, bool authenticated, CancellationToken ct)
    {
        using var response = await SendCoreAsync(method, path, body, authenticated, ct);
        var content = await response.Content.ReadAsStringAsync(ct);

        if (!response.IsSuccessStatusCode)
            throw new ApiClientException(ReadErrorMessage(response.StatusCode, content, response.ReasonPhrase));

        var result = JsonSerializer.Deserialize<T>(content, JsonOptions);
        return result ?? throw new ApiClientException("A API retornou uma resposta vazia ou inválida.");
    }

    private async Task SendAsync(HttpMethod method, string path, object? body, bool authenticated, CancellationToken ct)
    {
        using var response = await SendCoreAsync(method, path, body, authenticated, ct);
        if (response.IsSuccessStatusCode)
            return;

        var content = await response.Content.ReadAsStringAsync(ct);
        throw new ApiClientException(ReadErrorMessage(response.StatusCode, content, response.ReasonPhrase));
    }

    private async Task<HttpResponseMessage> SendCoreAsync(HttpMethod method, string path, object? body, bool authenticated, CancellationToken ct)
    {
        using var request = new HttpRequestMessage(method, path);

        if (authenticated)
            AddBearerToken(request);

        if (body is not null)
        {
            var json = JsonSerializer.Serialize(body, JsonOptions);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");
        }

        return await httpClient.SendAsync(request, ct);
    }

    private void AddBearerToken(HttpRequestMessage request)
    {
        if (string.IsNullOrWhiteSpace(authSession.AccessToken))
            throw new ApiClientException("Sessão expirada. Faça login novamente.");

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authSession.AccessToken);
    }

    private string ReadErrorMessage(System.Net.HttpStatusCode statusCode, string content, string? reasonPhrase)
    {
        if (statusCode == System.Net.HttpStatusCode.Unauthorized)
            return "Não autorizado. Verifique sua permissão ou refaça o login.";

        return ApiErrorReader.Read(content, reasonPhrase);
    }
}
