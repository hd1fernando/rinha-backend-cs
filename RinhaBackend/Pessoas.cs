public class Pessoas
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string? Apelido { get; set; }

    public string? Nome { get; set; }

    public string? Nascimento { get; set; }

    public ICollection<Stack>? Stack { get; set; }

    public PessoaResponse ToResponse()
    {
        var stack = Stack?.Select(s => s.Name).ToList();
        return new PessoaResponse
        {
            Id = Id,
            Apelido = Apelido,
            Nome = Nome,
            Nascimento = Nascimento,
            Stack = stack.Count > 0 ? stack : null
        };
    }


}
