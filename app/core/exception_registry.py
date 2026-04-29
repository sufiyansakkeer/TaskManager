from fastapi import FastAPI, Request, HTTPException
from fastapi.exceptions import RequestValidationError
from app.core.exception import AppException, TaskNotFoundException
from app.core.exception_handler import (
    http_exception_handler,
    validation_exception_handler,
    app_exception_handler,
    generic_exception_handler,
)


def register_exception_handler(app: FastAPI):
    app.add_exception_handler(AppException, app_exception_handler)
    app.add_exception_handler(HTTPException, http_exception_handler)
    app.add_exception_handler(RequestValidationError, validation_exception_handler)
    app.add_exception_handler(Exception, generic_exception_handler)
