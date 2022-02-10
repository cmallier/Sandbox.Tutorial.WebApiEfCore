using Microsoft.EntityFrameworkCore;

using Sandbox.Tutorial.WebApiEfCore.Models;

namespace Sandbox.Tutorial.WebApiEfCore;

public class PizzaDb : DbContext
{
    public PizzaDb(DbContextOptions options): base(options)
    {

    }

    public DbSet<Pizza> Pizzas { get; set; }
}
