using Microsoft.EntityFrameworkCore;

// dotnet tool install --global dotnet-ef
// dotnet ef migrations add InitialCreate
// dotnet ef database update

public class AppDbContext : DbContext
{
    private static readonly ILoggerFactory Logger = LoggerFactory.Create(p => p.AddConsole());
    private readonly IWebHostEnvironment Env;

    public AppDbContext(DbContextOptions<AppDbContext> options, IWebHostEnvironment env) : base(options)
    {
        Env = env;
    }

    public DbSet<Pessoas> Pessoas { get; set; }
    public DbSet<Stack> Stack { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        var postgres = "Server=localhost;Database=postgres;Port=5432;User Id=postgres;Password=postgres; Include Error Detail=true";
        //postgres = "Host=rinha.db;Port=5432;Database=postgres;User Id=postgres;Password=postgres; Include Error Detail=true";

        options.UseNpgsql(postgres);

        if (Env.IsDevelopment())
        {
            options.UseLoggerFactory(Logger);
            options.EnableSensitiveDataLogging();
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Pessoas>(pessoa =>
        {
            pessoa.HasKey(p => p.Id);
            pessoa.Property(pessoa => pessoa.Apelido).IsRequired().HasMaxLength(32);
            pessoa.HasIndex(p => p.Apelido).IsUnique();
            pessoa.Property(pessoa => pessoa.Nome).IsRequired().HasMaxLength(100);
            pessoa.Property(pessoa => pessoa.Nascimento).IsRequired();
        });

        modelBuilder.Entity<Stack>(stack =>
        {
            stack.HasKey(s => s.Id);
            stack.Property(s => s.Name).IsRequired().HasMaxLength(32);
        });
    }
}