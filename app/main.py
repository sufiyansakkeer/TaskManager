from fastapi import FastAPI, HTTPException, Request
from fastapi.exceptions import RequestValidationError
from app.core.exception import AppException, TaskNotFoundException
from app.core.exception_handler import (
    http_exception_handler,
    validation_exception_handler,
    app_exception_handler,
)
from app.core.exception_registry import register_exception_handler
from app.database import engine, Base
from app.router import auth_router, tasks

app = FastAPI()

register_exception_handler(app)

app.include_router(tasks.router)
app.include_router(auth_router.router)


@app.on_event("startup")
async def startup():
    async with engine.begin() as conn:
        await conn.run_sync(Base.metadata.create_all)  # create tables automatically


@app.get("/")
async def root():
    return {"message": "API running"}
