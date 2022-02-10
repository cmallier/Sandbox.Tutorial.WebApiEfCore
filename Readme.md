# Sandbox.Tutorial.WebApiEfCore


## Apis

``` csharp
app.MapGet( "/pizzas", async (PizzaDb db) => await db.Pizzas.ToListAsync() );

app.MapGet("/pizza/{id}", async (PizzaDb db, int id) => await db.Pizzas.FindAsync(id));
```

``` csharp
app.MapPost("/pizza", async (PizzaDb db, Pizza pizza) =>
{
    await db.Pizzas.AddAsync(pizza);
    await db.SaveChangesAsync();

    return Results.Created($"/pizza/{pizza.Id}", pizza);
});
```

``` csharp
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
```

``` csharp
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
```


## Packages

``` bash
> dotnet add package Microsoft.EntityFrameworkCore.Sqlite
> dotnet add package Microsoft.EntityFrameworkCore.Design

```

## Migrations

``` bash
> dotnet tool install --local dotnet-ef

> dotnet ef migrations add InitialCreate
> dotnet ef database update
```


## Resources

[Learn Module](https://docs.microsoft.com/fr-fr/learn/modules/build-web-api-minimal-database/5-exercise-use-sqlite-database)

[Minimal Apis c#](https://minimal-apis.github.io/)

[Damian Edwards MinimalApiPlayground](https://github.com/DamianEdwards/MinimalApiPlayground)
