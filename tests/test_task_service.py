from fastapi import HTTPException
import pytest
from app.models.task import Task
from app.schemas.task import TaskCreate
from app.services.task_service import TaskService


@pytest.mark.asyncio
async def test_create_task(task_service):
    result = await task_service.create_task(
        TaskCreate(title="Test Task", description="Test Description"), user_id=1
    )
    assert result.title == "Test Task"
    assert result.description == "Test Description"
    assert result.owner_id == 1

@pytest.mark.asyncio
async def test_delete_task_success(task_service):
    task = await task_service.create_task(
        TaskCreate(title="Task to Delete", description="To be deleted"), user_id=1
    )
    await task_service.delete_task(task.id,1)
    tasks = await task_service.get_tasks(1)
    assert len(tasks)==0

@pytest.mark.asyncio
async def test_get_tasks(task_service):
    await task_service.create_task(
        TaskCreate(title="Task 1", description="Description 1"), user_id=1
    )
    await task_service.create_task(
        TaskCreate(title="Task 2", description="Description 2"), user_id=1
    )
    tasks = await task_service.get_tasks(1)
    assert len(tasks) == 2

@pytest.mark.asyncio
async def test_delete_task_not_found(task_service):
    with pytest.raises(HTTPException) as exc_info:
        await task_service.delete_task(999,1)
    assert exc_info.value.status_code ==404