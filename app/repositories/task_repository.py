from sqlalchemy import select
from sqlalchemy.ext.asyncio import AsyncSession

from app.models.task import Task


class TaskRepository:
    def __init__(self, db: AsyncSession):
        self.db = db

    async def create(
        self,
        title: str,
        description: str | None,
        user_id: int,
    ):
        new_task = Task(
            title=title, description=description, user_id=user_id, is_completed=False
        )
        self.db.add(new_task)
        await self.db.commit()
        await self.db.refresh(new_task)
        return new_task

    async def get_all_by_user(
        self,
        user_id: int,
        skip: int = 0,
        limit: int = 10,
        is_completed: bool | None = None,
    ):
        query = select(Task).where(Task.user_id == user_id)

        if is_completed is not None:
            query = query.where(Task.is_completed == is_completed)

        query = query.offset(skip).limit(limit)
        result = await self.db.execute(query)
        return result.scalars().all()

    async def get_by_id_and_user(self, task_id: int, user_id: int):
        result = await self.db.execute(
            select(Task).where(Task.id == task_id, Task.user_id == user_id)
        )
        return result.scalar_one_or_none()

    async def update(self, task: Task, title: str, description: str):
        task.title = title
        task.description = description
        await self.db.commit()
        await self.db.refresh(task)
        return task

    async def delete(self, task: Task):
        await self.db.delete(task)
        await self.db.commit()
