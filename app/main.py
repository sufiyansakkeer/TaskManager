from fastapi import FastAPI, HTTPException, Request
from fastapi.exceptions import RequestValidationError
from app.core.exception import AppException, TaskNotFoundException
from app.core.exception_handler import (
    http_exception_handler,
    validation_exception_handler,
    app_exception_handler,
    task_not_found_exception_handler,
)
from app.database import engine, Base
from app.router import auth_router, tasks

app = FastAPI()

app.include_router(tasks.router)
app.include_router(auth_router.router)

@app.exception_handler(HTTPException)
async def handle_http_exception(request: Request, exc: HTTPException):
    return await http_exception_handler(request,exc)

@app.exception_handler(RequestValidationError)
async def handle_validation_exception(request: Request, exc: RequestValidationError):
    return await validation_exception_handler(request, exc)

@app.exception_handler(AppException)
async def handle_app_exception(request: Request, exc: AppException):
    return await app_exception_handler(request, exc)

@app.exception_handler(TaskNotFoundException)
async def handle_task_not_found_exception(request: Request, exc: TaskNotFoundException):
    return await task_not_found_exception_handler(request, exc)


@app.on_event("startup")
async def startup():
    async with engine.begin() as conn:
        await conn.run_sync(Base.metadata.create_all)  # create tables automatically


@app.get("/")
async def root():
    return {"message": "API running"}
