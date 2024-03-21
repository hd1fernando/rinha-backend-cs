public class Stack
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public override string ToString() => Name!;

    public bool IsValid() => Name?.Length <= 32;
}
