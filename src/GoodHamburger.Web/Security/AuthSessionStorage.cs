using System.Text.Json;
using GoodHamburger.Web.Auth.Responses;
using Microsoft.JSInterop;

namespace GoodHamburger.Web.Security;

public sealed class AuthSessionStorage(IJSRuntime jsRuntime)
{
    private const string StorageKey = "goodhamburger.auth";
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    public async Task<AuthResponse?> GetAsync()
    {
        try
        {
            var json = await jsRuntime.InvokeAsync<string?>("goodHamburgerAuth.get", StorageKey);
            if (string.IsNullOrWhiteSpace(json))
                return null;

            var storedSession = JsonSerializer.Deserialize<StoredAuthSession>(json, JsonOptions);
            if (storedSession?.Response is null || storedSession.ExpiresAtUtc <= DateTimeOffset.UtcNow)
            {
                await ClearAsync();
                return null;
            }

            return storedSession.Response;
        }
        catch (JsonException)
        {
            await ClearAsync();
            return null;
        }
    }

    public ValueTask SetAsync(AuthResponse response)
    {
        var storedSession = new StoredAuthSession
        {
            Response = response,
            ExpiresAtUtc = DateTimeOffset.UtcNow.AddSeconds(Math.Max(response.ExpiresIn, 0))
        };

        var json = JsonSerializer.Serialize(storedSession, JsonOptions);
        return jsRuntime.InvokeVoidAsync("goodHamburgerAuth.set", StorageKey, json);
    }

    public ValueTask ClearAsync()
    {
        return jsRuntime.InvokeVoidAsync("goodHamburgerAuth.remove", StorageKey);
    }

    private sealed class StoredAuthSession
    {
        public AuthResponse? Response { get; set; }

        public DateTimeOffset ExpiresAtUtc { get; set; }
    }
}
