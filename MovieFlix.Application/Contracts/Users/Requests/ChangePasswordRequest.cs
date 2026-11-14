namespace MovieFlix.Application.Contracts.Users.Requests;

public record ChangePasswordRequest(
    string CurrentPassword,
    string NewPassword,
    string ConfirmNewPassword);
