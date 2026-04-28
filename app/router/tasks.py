from fastapi import APIRouter, Depends, HTTPException
from sqlalchemy.ext.asyncio import AsyncSession

from app.core.auth import get_current_user
from app.dependencies import get_db
from app.schemas.response import APIResponse
from app.schemas.task import TaskCreate, TaskResponse
from app.models.user import User
from app.services.task_service import TaskService


router = APIRouter(prefix="/tasks", tags=["Tasks"])


@router.post("/", response_model=APIResponse[TaskResponse])
async def create_task(
    task: TaskCreate,
    db: AsyncSession = Depends(get_db),
    current_user: User = Depends(get_current_user),
):
    service = TaskService(db)

    new_task = await service.create_task(task, current_user.id)
    return APIResponse(success=True, message="Task created successfully", data=new_task)


@router.get("/", response_model=APIResponse[list[TaskResponse]])
async def get_tasks(
    skip: int = 0,
    limit: int = 10,
    is_completed: bool | None = None,
    db: AsyncSession = Depends(get_db),
    current_user: User = Depends(get_current_user),
):
    service = TaskService(db)
    tasks = await service.get_tasks(current_user.id, skip, limit, is_completed)
    return APIResponse(success=True, message="Tasks fetched successfully", data=tasks)


@router.put("/{task_id}", response_model=APIResponse[TaskResponse])
async def update_task(
    task_id: int,
    updated_task: TaskCreate,
    db: AsyncSession = Depends(get_db),
    current_user: User = Depends(get_current_user),
):
    service = TaskService(db)
    task = await service.update_task(task_id, updated_task, current_user.id)
    if not task:
        raise HTTPException(status_code=404, detail="Task not found")
    return APIResponse(success=True, message="Task updated successfully", data=task)


@router.delete("/{task_id}", response_model=APIResponse[None])
async def delete_task(
    task_id: int,
    db: AsyncSession = Depends(get_db),
    current_user: User = Depends(get_current_user),
):
    service = TaskService(db)
    task = await service.delete_task(task_id, current_user.id)
    if not task:
        raise HTTPException(status_code=404, detail="Task not found")
    return APIResponse(success=True, message="Task deleted successfully", data=None)
