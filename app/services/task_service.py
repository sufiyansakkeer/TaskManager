from sqlalchemy import select

from app.core.exception import TaskNotFoundException
from app.models.task import Task
from app.repositories.task_repository import TaskRepository
from app.schemas.task import TaskCreate
from app.core.logger import logger


class TaskService:
    def __init__(self, db):
        self.repo = TaskRepository(db)

    async def create_task(self, task: TaskCreate, user_id: int):
        new_task = await self.repo.create(
            title=task.title,
            description=task.description,
            user_id=user_id,
        )
        logger.info("Task created")
        return new_task

    async def get_tasks(
        self,
        user_id: int,
        skip: int = 0,
        limit: int = 10,
        is_completed: bool | None = None,
    ):
        tasks = await self.repo.get_all_by_user(
            user_id=user_id, skip=skip, limit=limit, is_completed=is_completed
        )
        logger.info(f"{len(tasks)} tasks fetched")
        return tasks

    async def update_task(self, task_id: int, updated_task: TaskCreate, user_id: int):
        task = await self.repo.get_by_id_and_user(task_id, user_id)

        if not task:
            logger.info("Task Not there")
            raise TaskNotFoundException("Task not found")

        updated = await self.repo.update(
            task, updated_task.title, updated_task.description
        )

        logger.info("Task updated")
        return updated

    async def delete_task(self, task_id: int, user_id: int):
        task = await self.repo.get_by_id_and_user(task_id, user_id)

        if not task:
            logger.info("Task Not there")
            raise TaskNotFoundException("Task not found")

        await self.repo.delete(task)
        return {"message": "Task deleted"}
