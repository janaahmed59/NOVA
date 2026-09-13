using NOVA.Domain.Common.Results;
namespace NOVA.Application.Common.Errors
{
    internal static partial class NovaErrors
    {
        public static readonly Error ValidationError =
             Error.Validation(
                 "VALIDATION_ERROR",
                 "One or more fields are invalid.");

        public static readonly Error NotFound =
            Error.NotFound(
                "NOT_FOUND",
                "The requested resource was not found.");

        public static readonly Error ConcurrencyConflict =
            Error.Conflict(
                "CONCURRENCY_CONFLICT",
                "The resource was modified by another request.");

        public static readonly Error ConfirmationRequired =
            Error.Conflict(
                "CONFIRMATION_REQUIRED",
                "This action requires explicit confirmation.");

        public static readonly Error IllegalStatusTransition =
            Error.Conflict(
                "ILLEGAL_STATUS_TRANSITION",
                "This status change is not allowed.");

        public static readonly Error Unauthenticated =
        Error.Unauthorized(
            "UNAUTHENTICATED",
            "Authentication is required.");

        public static readonly Error Forbidden =
            Error.Forbidden(
                "FORBIDDEN",
                "You do not have permission to perform this action.");

        public static readonly Error EmailTaken =
            Error.Conflict(
                "EMAIL_TAKEN",
                "This email address is already registered.");
        public static readonly Error CurrentPasswordIncorrect =
        Error.BadRequest(
            "CURRENT_PASSWORD_INCORRECT",
            "The current password is incorrect.");

        public static readonly Error AccountDeactivated =
            Error.Forbidden(
                "ACCOUNT_DEACTIVATED",
                "This account has been deactivated. Please contact an organizer.");

        public static readonly Error EmailNotConfirmed =
            Error.Forbidden(
                "EMAIL_NOT_CONFIRMED",
                "Please confirm your email address before signing in.");

        public static readonly Error WeakPassword =
            Error.Validation(
                "WEAK_PASSWORD",
                "The password does not meet the strength policy.");

        public static readonly Error TokenInvalid =
            Error.Unauthorized(
                "TOKEN_INVALID",
                "The token is expired or unknown.");

        public static readonly Error TokenReused =
            Error.Unauthorized(
                "TOKEN_REUSED",
                "The token has already been used; re-authentication is required.");

        public static readonly Error ResetTokenInvalid =
            Error.BadRequest(
                "RESET_TOKEN_INVALID",
                "The password-reset token is invalid or expired.");

        public static readonly Error ConfirmTokenInvalid =
            Error.BadRequest(
                "CONFIRM_TOKEN_INVALID",
                "The email-confirmation token is invalid or expired.");

        public static readonly Error UserNotFound =
            Error.NotFound(
                "USER_NOT_FOUND",
                "The specified user was not found.");

        public static readonly Error UploadFailed =
        Error.Unexpected(
            "IMAGE_UPLOAD_FAILED",
            "The image could not be stored. Please try again.");

        public static readonly Error InvalidFileType =
        Error.Business(
            "INVALID_FILE_TYPE",
            "The uploaded file is not an allowed image type.");

    }
}
