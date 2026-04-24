import enum


class UserRole(str, enum.Enum):
    ADMIN = "admin"
    USER = "user"


class TaskStatus(str, enum.Enum):
    TODO = "todo"
    IN_PROGRESS = "in_progress"
    DONE = "done"