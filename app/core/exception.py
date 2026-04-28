from fastapi import HTTPException, Request
from fastapi.exceptions import RequestValidationError
from fastapi.responses import JSONResponse


class AppException(Exception):
    def __init__(self, message: str):
        self.message = message
        super().__init__(message)


class UserNotFoundException(AppException):
    pass


class TaskNotFoundException(AppException):
    pass
