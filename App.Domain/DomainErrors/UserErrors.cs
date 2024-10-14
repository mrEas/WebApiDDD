using ErrorOr;

namespace App.Domain.DomainErrors
{
    public static class UserErrors
    {
        public static Error UserUnauthorized => Error.Validation("User", "User is unauthorized");
        public static Error UserNotFound => Error.Validation("User", "User has not found");
    }
}
