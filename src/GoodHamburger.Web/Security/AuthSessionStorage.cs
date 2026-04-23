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
        var json = await jsRuntime.InvokeAsync<string?>("goodHamburgerAuth.get", StorageKey);
        if (string.IsNullOrWhiteSpace(json))
            return null;

        return JsonSerializer.Deserialize<AuthResponse>(json, JsonOptions);
    }

    public ValueTask SetAsync(AuthResponse response)
    {
        var json = JsonSerializer.Serialize(response, JsonOptions);
        return jsRuntime.InvokeVoidAsync("goodHamburgerAuth.set", StorageKey, json);
    }

    public ValueTask ClearAsync()
    {
        return jsRuntime.InvokeVoidAsync("goodHamburgerAuth.remove", StorageKey);
    }
}
