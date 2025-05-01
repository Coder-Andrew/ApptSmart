export enum LoginErrorCode {
    INVALID_CREDENTIALS = "INVALID_CREDENTIALS",
    SERVER_ERROR = "SERVER_ERROR",
    TOO_MANY_ATTEMPTS = "TOO_MANY_ATTEMPTS",
    UNKNOWN_ERROR = "UNKNOWN_ERROR"
}

export class LoginError extends Error {
    code: LoginErrorCode;
    constructor(message: string, code: LoginErrorCode) {
        super(message);
        this.code = code;
        this.name = "LoginError";
    }
}