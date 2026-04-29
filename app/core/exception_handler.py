from fastapi import HTTPException, Request
from fastapi.exceptions import RequestValidationError
from fastapi.responses import JSONResponse

from app.core.exception import AppException, TaskNotFoundException, error_response


# App-level exceptions
async def app_exception_handler(request: Request, exc: Exception) -> JSONResponse:
    if isinstance(exc, AppException):
        return JSONResponse(
            status_code=exc.status_code, content=error_response(exc.message)
        )

    return JSONResponse(
        status_code=500,
        content=error_response("Unexpected application error"),
    )


# Fast api exceptions
async def http_exception_handler(request: Request, exc: Exception) -> JSONResponse:
    if isinstance(exc, HTTPException):
        return JSONResponse(
            status_code=exc.status_code,
            content=error_response(exc.detail),
        )
    return JSONResponse(
        status_code=500,
        content=error_response("HTTP error"),
    )


async def validation_exception_handler(
    request: Request, exc: Exception
) -> JSONResponse:
    if isinstance(exc, RequestValidationError):
        return JSONResponse(
            status_code=422,
            content=error_response("validation error", data=exc.errors()),
        )
    return JSONResponse(
        status_code=422,
        content=error_response("Invalid request"),
    )


# global
async def generic_exception_handler(request: Request, exc: Exception) -> JSONResponse:
    return JSONResponse(
        status_code=500,
        content=error_response("Internal server error"),
    )
