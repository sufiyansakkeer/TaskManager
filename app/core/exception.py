class AppException(Exception):
    def __init__(self, message: str, status_code: int = 400):
        self.message = message
        self.status_code = status_code
        super().__init__(message)


class UserNotFoundException(AppException):
    def __init__(self, message: str = "User not found"):
        super().__init__(message, status_code=404)


class TaskNotFoundException(AppException):
    def __init__(self, message: str = "Task not found"):
        super().__init__(message, status_code=404)


def error_response(message: str, data=None):
    return {
        "success": False,
        "message": message,
        "data": data,
    }
