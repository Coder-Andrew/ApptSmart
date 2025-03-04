namespace ApptSmartBackend.Services
{
    public enum GenericStatusCode
    {
        Success,
        Failure,
        UserAlreadyExists,
        FailedToCreateUser,
        InvalidCredentials,
        SuccessfulLogin,
        UserCreated,
        FailedToAddRole,
    }
    public class GenericResponse<T>
    {
        public T? Data { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
        public GenericStatusCode? StatusCode { get; set; }

        public GenericResponse(T? data, bool success, string message, GenericStatusCode? statusCode = null)
        {
            Data = data;
            Success = success;
            Message = message;
            StatusCode = statusCode;
        }
    }
}
