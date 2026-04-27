from sqlalchemy import Column, Integer,String, Boolean, ForeignKey
from sqlalchemy.orm import relationship, mapped_column , Mapped
from app.database import Base
from app.models.user import User
from typing import Optional


class Task(Base):
    __tablename__ = "tasks"
    
    id: Mapped[int] = mapped_column(primary_key=True)
    title : Mapped[str]= mapped_column(String)
    description : Mapped[Optional[str]] = mapped_column(String, nullable= True)
    is_completed : Mapped[bool] = mapped_column(Boolean)
    
    user_id: Mapped[int] = mapped_column(ForeignKey("users.id"))
    
    owner: Mapped["User"] = relationship("User", back_populates="tasks")

