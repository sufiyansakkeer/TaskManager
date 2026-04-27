from fastapi import APIRouter, Depends, HTTPException
from sqlalchemy import select
from sqlalchemy.ext.asyncio import AsyncSession

from app.core.auth import get_current_user
from app.dependencies import get_db
from app.models.task import Task
from app.schemas.task import TaskCreate, TaskResponse
from app.models.user import User
from app.services import task_service


router = APIRouter(prefix="/tasks", tags=["Tasks"])

@router.post("/",response_model=TaskResponse)
async def create_task(
    task: TaskCreate,
    db: AsyncSession = Depends(get_db),
    current_user: User = Depends(get_current_user)
):
    return await task_service.create_task(task,db,current_user.id)

@router.get("/", response_model=list[TaskResponse])
async def get_tasks(
    db: AsyncSession = Depends(get_db),
    current_user: User = Depends(get_current_user)
):
    return await task_service.get_tasks(db,current_user.id)

@router.put("/{task_id}",response_model=TaskResponse)
async def update_task(
    task_id: int,
    updated_task: TaskCreate,
    db:AsyncSession = Depends(get_db),
    current_user: User= Depends(get_current_user)
):
    task= await task_service.update_task(task_id,updated_task,db,current_user.id)
    if not task:
        raise HTTPException(status_code=404, detail="Task not found")
    return task

@router.delete("/{task_id}")
async def delete_task(
    task_id:int,
    db: AsyncSession = Depends(get_db),
    current_user: User = Depends(get_current_user)
):
    task= await task_service.delete_task(task_id,db,current_user.id)
    if not task:
        raise HTTPException(status_code=404, detail="Task not found")
    return {"message": "Task deleted successfully"}