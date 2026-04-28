from fastapi import FastAPI, HTTPException
from fastapi.exceptions import RequestValidationError
from app.core.exception import http_exception_handler, validation_exception_handler
from app.database import engine, Base
from app.router import auth_router, tasks

app = FastAPI()

app.include_router(tasks.router)
app.include_router(auth_router.router)
app.add_exception_handler(HTTPException, http_exception_handler)
app.add_exception_handler(RequestValidationError, validation_exception_handler)


@app.on_event("startup")
async def startup():
    async with engine.begin() as conn:
        await conn.run_sync(Base.metadata.create_all)  # create tables automatically


@app.get("/")
async def root():
    return {"message": "API running"}
