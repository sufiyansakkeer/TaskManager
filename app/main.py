from fastapi import FastAPI, Depends
from sqlalchemy.ext.asyncio import AsyncSession
from app.database import engine,Base
from app.dependencies import get_db
from app.models.user import User
from app.schemas.user import UserCreate, UserResponse

app = FastAPI()

@app.on_event("startup")
async def startup():
    async with engine.begin() as conn:
        await conn.run_sync(Base.metadata.create_all) # create tables automatically

@app.get("/")
async def root():
    return {"message": "API running"}

@app.post("/users", response_model=UserResponse)
async def create_user(
    user:UserCreate,
    db: AsyncSession = Depends(get_db)
    ):
    
    new_user = User(
        email = user.email,
        password= user.password
    )
    
    db.add(new_user)
    await db.commit()
    await db.refresh(new_user)
    
    return new_user