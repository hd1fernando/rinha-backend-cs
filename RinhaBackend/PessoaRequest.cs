using System.Globalization;

public class PessoaRequest
{
    public string? Apelido { get; set; }

    public string? Nome { get; set; }

    public string? Nascimento { get; set; }

    public IEnumerable<string>? Stack { get; set; }

    public bool IsValid(AppDbContext context)
    {
        if (string.IsNullOrEmpty(Apelido) || string.IsNullOrEmpty(Nome) || string.IsNullOrEmpty(Nascimento))
        {
            return false;
        }

        if (Apelido.Length > 32 || Nome.Length > 100)
        {
            return false;
        }

        if (!DateTime.TryParseExact(Nascimento, "yyyy-mm-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var _))
        {
            return false;
        }

        if (Stack?.Count() > 0 && Stack.Any(s => s.Length > 32))
        {
            return false;
        }

        var pessoaExiste = context.Pessoas.Any(p => p.Apelido == Apelido);
        if (pessoaExiste)
        {
            return false;
        }

        return true;

    }

    public Pessoas ToModel()
    {
        var pessoa = new Pessoas
        {
            Apelido = Apelido,
            Nome = Nome,
            Nascimento = Nascimento,
            Stack = Stack?.Select(s => new Stack { Name = s }).ToList()
        };

        return pessoa;
    }
}
