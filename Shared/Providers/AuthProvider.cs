namespace Shared.Providers;

public class AuthProvider
{
}

public record TokenPayload(
    int UserId,
    string role,
    string[] permissions
    );