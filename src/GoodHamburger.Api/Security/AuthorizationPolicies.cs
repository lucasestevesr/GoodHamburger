namespace GoodHamburger.Api.Security
{
    public static class AuthorizationPolicies
    {
        public const string OrderManagement = nameof(OrderManagement);
        public const string ProductManagement = nameof(ProductManagement);
        public const string CreateAttendantManagement = nameof(CreateAttendantManagement);
        public const string UserManagement = nameof(UserManagement);
    }
}
