namespace ProjectOrkestra.Domain.Exceptions;

public class UnauthorizedCredentialsException : Exception {
    public UnauthorizedCredentialsException(string message)
        : base(message) { }
}
