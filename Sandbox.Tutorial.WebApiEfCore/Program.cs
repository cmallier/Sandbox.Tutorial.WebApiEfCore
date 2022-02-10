using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using Sandbox.Tutorial.WebApiEfCore;
using Microsoft.AspNetCore.Builder;
using Sandbox.Tutorial.WebApiEfCore.Models;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("Pizzas") ?? "Data Source=Pizzas.db";

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();

//builder.Services.AddDbContext<PizzaDb>(options => options.UseInMemoryDatabase( "MemoryDatabase" ));
builder.Services.AddSqlite<PizzaDb>(connectionString);



builder.Services.AddSwaggerGen( x =>
{
    x.SwaggerDoc("v1", new OpenApiInfo { 
        Title = "PizzaStore Api",
        Description = "Description",
        Version = "v1",
    } );
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI( x => {
        x.SwaggerEndpoint("/swagger/v1/swagger.json", "PizzaStore Api V1");
    });
}

app.UseHttpsRedirection();

app.MapGet( "/pizzas", async (PizzaDb db) => await db.Pizzas.ToListAsync() );

app.MapGet("/pizza/{id}", async (PizzaDb db, int id) => await db.Pizzas.FindAsync(id));

app.MapPost("/pizza", async (PizzaDb db, Pizza pizza) =>
{
    await db.Pizzas.AddAsync(pizza);
    await db.SaveChangesAsync();

    return Results.Created($"/pizza/{pizza.Id}", pizza);
});

app.MapPut("/pizza/{id}", async (PizzaDb db, Pizza updatePizza, int id) => {

    var pizza = await db.Pizzas.FindAsync(id);
    if (pizza is null)
    {
        return Results.NotFound();
    }

    pizza.Name = updatePizza.Name;
    pizza.Description = updatePizza.Description;

    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.MapDelete( "/pizza/{id}", async ( PizzaDb db, int id ) => {

    var pizza = await db.Pizzas.FindAsync(id);
    if (pizza is null)
    {
        return Results.NotFound();
    }

    db.Pizzas.Remove(pizza);
    await db.SaveChangesAsync();
    return Results.Ok();
});

app.Run();

