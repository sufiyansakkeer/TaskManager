public class TaskItem
{
    public Guid Id { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public bool IsCompleted { get; private set; }
    public Guid UserId { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    private TaskItem() { }

    public TaskItem(string title, string? description, Guid userId)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title cannot be empty");

        if (userId == Guid.Empty)
            throw new ArgumentException("Invalid userId");

        Id = Guid.NewGuid();
        Title = title;
        Description = description;
        UserId = userId;
        IsCompleted = false;
        CreatedAt = DateTime.UtcNow;
    }

    public void MarkComplete()
    {
        if (IsCompleted) return;

        IsCompleted = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public void MarkIncomplete()
    {
        if (!IsCompleted) return;

        IsCompleted = false;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Update(string title, string? description)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title cannot be empty");

        Title = title;
        Description = description;
        UpdatedAt = DateTime.UtcNow;
    }
}