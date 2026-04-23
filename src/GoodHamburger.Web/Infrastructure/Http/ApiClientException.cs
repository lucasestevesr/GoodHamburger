namespace GoodHamburger.Web.Infrastructure.Http;

public sealed class ApiClientException(string message) : Exception(message);
