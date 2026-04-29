import pytest
from app.services.task_service import TaskService


class FakeTask:
    def __init__(self, id, title, description, owner_id):
        self.id = id
        self.title = title
        self.description = description
        self.owner_id = owner_id


class FakeRepo:
    def __init__(self):
        self.tasks = []

    async def create(self, title, description, user_id):
        task = FakeTask(id=1, title=title, description=description, owner_id=user_id)
        self.tasks.append(task)
        return task

    async def get_all_by_user(self, user_id):
        return [t for t in self.tasks if t.owner_id == user_id]

    async def get_by_id(self, task_id, user_id):
        for t in self.tasks:
            if t.id == task_id and t.owner_id == user_id:
                return t
        return None

    async def delete(self, task):
        self.tasks.remove(task)


@pytest.fixture
def fake_repo():
    return FakeRepo()


@pytest.fixture
def task_service(fake_repo):
    service = TaskService(db=None)
    service.repo = fake_repo  # override real repo
    return service
