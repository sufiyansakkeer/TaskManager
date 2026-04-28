from fastapi import HTTPException, Request
from fastapi.exceptions import RequestValidationError
from fastapi.responses import JSONResponse

from app.core.exception import AppException, TaskNotFoundException


async def task_not_found_exception_handler(request: Request, exc: Exception):
    return JSONResponse(
        status_code=404,
        content={
            "success": False,
            "message": getattr(exc, "message", "Task not found"),
            "data": None,
        },
    )


async def app_exception_handler(request: Request, exc: Exception):

    return JSONResponse(
        status_code=400,
        content={
            "success": False,
            "message": getattr(exc, "message", str(exc)),
            "data": None,
        },
    )


async def http_exception_handler(request: Request, exc: HTTPException):
    return JSONResponse(
        status_code=exc.status_code,
        content={"success": False, "message": exc.detail, "data": None},
    )


async def validation_exception_handler(request: Request, exc: RequestValidationError):
    return JSONResponse(
        status_code=422,
        content={
            "success": False,
            "message": "Validation error",
            "data": exc.errors(),
        },
    )
