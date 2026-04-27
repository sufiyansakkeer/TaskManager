from fastapi import APIRouter, Depends, HTTPException
from sqlalchemy import select
from sqlalchemy.ext.asyncio import AsyncSession
from app.core.auth import create_access_token, get_current_user
from app.core.security import hash_password, verify_password
from app.dependencies import get_db
from app.models.user import User
from app.schemas.user import UserResponse,UserCreate


router = APIRouter(prefix="/auth",tags= ["Auth"])

@router.post("/register", response_model=UserResponse) # this response_model will validate response . if the return is wrong then it throw error.
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

@router.post("/login")
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

@router.get("/me")
async def get_me(current_user: User = Depends(get_current_user)):
    return {
        "email": current_user.email,
        "id": current_user.id
    }
