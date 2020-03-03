using Microsoft.EntityFrameworkCore;

namespace Api.Database
{

  public class StoreContext : DbContext
  {
    public StoreContext(){}
    public StoreContext(DbContextOptions<StoreContext> options)
      : base(options)
    { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            //optionsBuilder.UseInMemoryDatabase("BooksDb");
        }

    public DbSet<Book> Books { get; set; }
    public DbSet<Author> Authors { get; set; }
  }
}