from sqlalchemy.ext.asyncio import AsyncSession
from sqlalchemy import select

from app.models.task import Task
from app.schemas.task import TaskCreate
from app.core.logger import logger


async def create_task(
    task: TaskCreate,
    db: AsyncSession ,
    user_id: int
):
    new_task = Task(
        title = task.title,
        description= task.description,
        user_id = user_id,
        is_completed= False,
    
        
    )
    
    db.add(new_task)
    await db.commit()
    await db.refresh(new_task)
    
    return new_task

async def get_tasks(
    db: AsyncSession,
    user_id: int,
    skip:int =0,
    limit: int = 10,
    is_completed:bool | None =None
):
    query = select(Task).where(Task.user_id==user_id)
    
    if is_completed is not None:
        query = query.where(Task.is_completed == is_completed)
        
    query = query.offset(skip).limit(limit)
    result= await db.execute(query)
    
    tasks = result.scalars().all()
    logger.info(f"{len(tasks)} is the length of the task in logger")
    return tasks

async def update_task(
    task_id: int,
    updated_task: TaskCreate,
    db:AsyncSession,
    user_id: int
):
    result = await db.execute(
        select(Task).where(
            Task.id == task_id,
            Task.user_id == user_id
        )
    )
    
    task = result.scalar_one_or_none()
    
    if not task:
        return None
    
    task.title = updated_task.title
    task.description = updated_task.description
    
    await db.commit()
    await db.refresh(task)
    logger.info("Task updated")
    return task

async def delete_task(
    task_id:int,
    db: AsyncSession ,
    user_id: int
):
    result = await db.execute(
        select(Task).where(
            Task.id== task_id,
            Task.user_id == user_id
        )
    )
    
    task = result.scalar_one_or_none()
    
    if not task:
        logger.info("Task Not there")
        return None
    
    await db.delete(task)
    await db.commit()
    logger.info("Task Deleted")
    return {"message": "Task deleted"}