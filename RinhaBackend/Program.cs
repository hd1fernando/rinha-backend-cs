using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDbContext>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

if (app.Environment.IsDevelopment())
{
    app.ApplyMigrations();
}

app.MapPost("/pessoas", (AppDbContext context, HttpContext http, PessoaRequest pessoaViewModel) =>
{
    if (!pessoaViewModel.IsValid(context))
    {
        return Results.UnprocessableEntity();
    }

    var pessoas = pessoaViewModel.ToModel();

    context.Pessoas.Add(pessoas);
    context.SaveChanges();

    http.Response?.Headers?.Add("Location", $"/pessoas/{pessoas.Id}");

    return Results.Created();
});

app.MapGet("/pessoas/{id}", (AppDbContext dbContext, Guid id) =>
{
    var pessoa = dbContext.Pessoas
        .Include(p => p.Stack)
        .FirstOrDefault(p => p.Id == id);


    if (pessoa is null)
    {
        return Results.NotFound();
    }

    return Results.Ok(pessoa.ToResponse());
});

app.MapGet("/pessoas", (AppDbContext dbContext, string? t) =>
{
    var pessoas = dbContext.Pessoas
         .Include(p => p.Stack)
         .Where(p =>
            p.Apelido.Contains(t)
            || p.Nome.Contains(t)
            || p.Stack!.Any(s => s.Name.Contains(t))
            )
         .ToList();


    return Results.Ok(pessoas.Select(p => p.ToResponse()));
});

app.MapGet("/contagem-pessoas", (AppDbContext dbContext) =>
{
    var count = dbContext.Pessoas.Count();
    return Results.Ok(count);
});

app.Run();