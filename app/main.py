from fastapi import FastAPI, Depends, HTTPException
from fastapi.exceptions import RequestValidationError
from sqlalchemy.ext.asyncio import AsyncSession
from sqlalchemy import select
from app.core.exception import http_exception_handler,validation_exception_handler
from app.database import engine,Base
from app.dependencies import get_db
from app.models.user import User
from app.schemas.user import UserCreate, UserResponse
from app.models.task import Task
from app.schemas.task import TaskCreate,TaskResponse
from app.core.security import hash_password, verify_password
from app.core.auth import create_access_token, get_current_user
from app.router import auth, tasks

app = FastAPI()

app.include_router(tasks.router)
app.include_router(auth.router)
app.add_exception_handler(HTTPException, http_exception_handler)
app.add_exception_handler(RequestValidationError, validation_exception_handler)

@app.on_event("startup")
async def startup():
    async with engine.begin() as conn:
        await conn.run_sync(Base.metadata.create_all) # create tables automatically

@app.get("/")
async def root():
    return {"message": "API running"}


