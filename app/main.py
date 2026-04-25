from fastapi import FastAPI, Depends, HTTPException
from sqlalchemy.ext.asyncio import AsyncSession
from sqlalchemy import select
from app.database import engine,Base
from app.dependencies import get_db
from app.models.user import User
from app.schemas.user import UserCreate, UserResponse
from app.core.security import hash_password, verify_password
from app.core.auth import create_access_token

app = FastAPI()

@app.on_event("startup")
async def startup():
    async with engine.begin() as conn:
        await conn.run_sync(Base.metadata.create_all) # create tables automatically

@app.get("/")
async def root():
    return {"message": "API running"}

@app.post("/users", response_model=UserResponse) # this response_model will validate response . if the return is wrong then it throw error.
async def create_user(
    user:UserCreate,
    db: AsyncSession = Depends(get_db)
    ):
    
    result = await db.execute(
        select(User).where(User.email==user.email)
        )
    if result.scalar_one_or_none():
        raise HTTPException(
        status_code=400,
        detail="User email already exists"
    )
    
    hashed_pwd= hash_password(user.password)
    
    new_user = User(
        email = user.email,
        password= hashed_pwd
    )
    
    db.add(new_user)
    await db.commit()
    await db.refresh(new_user)
    
    return new_user

@app.post("/login")
async def login(user:UserCreate, db: AsyncSession= Depends(get_db)):
    result = await db.execute(
        select(User).where(User.email==user.email)
    )
    
    db_user = result.scalar_one_or_none()
    
    if not db_user:
        raise HTTPException(status_code=400, detail="Invalid email or password")
    
    if not verify_password(user.password,db_user.password):
        raise HTTPException(status_code=400, detail="Invalid email or password")
    
    access_token = create_access_token(
        data={"sub": db_user.email}
    )
    
    return {"access_token": access_token, "token_type": "bearer"}
