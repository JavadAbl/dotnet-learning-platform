namespace Users.Dto.Request;

internal record UserChangePasswordOtpDto(
    string Mobile,
    string NewPassword,
    string Otp
);
