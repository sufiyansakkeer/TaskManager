from sqlalchemy.orm import Mapped,mapped_column, relationship
from sqlalchemy import String, Integer
from app.database import Base
from typing import TYPE_CHECKING

if TYPE_CHECKING:
    from app.models.task import Task

class User(Base):
    __tablename__= "users"
    
    id: Mapped[int] = mapped_column(Integer,primary_key=True)
    email: Mapped[str] = mapped_column(String,unique= True,index=True)
    password: Mapped[str] = mapped_column(String)
    
    tasks : Mapped[list["Task"]]= relationship("Task", back_populates="owner")