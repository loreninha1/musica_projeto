using Musica.Models;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.WebHost.UseUrls("http://0.0.0.0:8000");

var app = builder.Build();

app.UseCors("AllowAll");

Ritmo[] ritmos = new Ritmo[100];
int totalRitmo = 0;

app.MapGet("/", () =>
{
    return Results.Ok("API Ritmo funcionando com sucesso!");
});

app.MapPost("/ritmos", (JsonElement body) =>
{
    Random random= new();
    Ritmo ritmos = new Ritmo();

    ritmos.Id = random.Next(1000,9999);
    ritmos.Titulo = body.GetProperty("titulo").GetString();
    ritmos.Artista = body.GetProperty("artista").GetString();
    ritmos.Compositor = body.GetProperty("compositor").GetString();
    ritmos.Genero = body.GetProperty("genero").GetString();
    ritmos.Ano = body.GetProperty("ano").GetInt32();

    ritmos[totalMusica] = ritmos;
    totalMusica++;

    return Results.Ok(
        new{
            ritmos
        }
    );
});

app.MapGet("/ritmos", () =>
{
    Musica[] ritmosCadastrados = new Musica[totalMusica];

    for(int i = 0; i < totalMusica; i ++)
    {
        ritmosCadastrados[i] = ritmos[i];
    }
    return Results.Ok(
        new{
            ritmosCadastrados
        }
    );

});

app.MapPatch("/ritmos/{id}", (int id, JsonElement body) =>
{
    double novo_salario = body.GetProperty("salario").GetDouble();

    for (int i = 0; i < totalFuncionarios; i++)
    {
        if (ritmos[i].Id == id)
        {
            ritmos[i].Salario = novo_salario;
            return Results.Ok(
                new
                {
                    ritmos = ritmos[i]
                }
            );
        }
    }

    return Results.NotFound(new
    {
        message = "Funcionário não encontrado."
    });
});

app.MapDelete("/ritmos/{id}", (int id) =>
{
    for (int i = 0; i < totalFuncionarios; i++)
    {
        if (ritmos[i].Id == id)
        {
            Funcionario ritmosRemovidos = ritmos[i];
            
            for (int j = i; j < totalRitmo - 1; j++)
            {
                ritmos[j] = ritmos[j + 1];
            }            

            totalRitmo--;

            return Results.Ok(new
            {
                mensagem = "Ritmo removido com sucesso.",
                ritmos = ritmosRemovidos
            });
        }
    }

    return Results.NotFound(new
    {
        message = "Ritmo não encontrado."
    });
});

app.MapGet("/ritmos/genero/busca/{genero}", (string genero) =>
{
    Ritmo[] ritmosEncontrados = new Ritmo[totalRitmo];

    int totalEncontrados = 0;

    for (int i = 0; i < totalRitmo; i++)
    {
        if (ritmos[i].Genero.ToLower() == genero.ToLower())
        // if (ritmos[i].Genero.ToLower().Equals(genero, StringComparison.CurrentCultureIgnoreCase))
        {
            ritmosEncontrados[totalEncontrados] = ritmos[i];
            totalEncontrados++;
        }
    }

    if (totalEncontrados > 0)
    {
        Ritmo[] resultadoFinal = new Ritmo[totalEncontrados];

        for (int i = 0; i < totalEncontrados; i++)
        {
            resultadoFinal[i] = ritmosEncontrados[i];
        }        

        return Results.Ok(new
        {
            genero,
            ritmos = ritmosEncontrados
        });
    } 

    return Results.NotFound(new
    {
        message = "Nenhum ritmo encontrado para esse gênero."
    });
});

app.MapGet("/ritmos/busca/{titulo}", (string titulo) =>
{
   Ritmo[] ritmosEncontrados = new Ritmo[totalRitmo];

    int totalEncontrados = 0;

    for (int i = 0; i < totalRitmo; i++)
    {
        if (ritmos[i].Titulo.ToLower() == titulo.ToLower())
        {
           ritmosEncontrados[totalEncontrados] = ritmos[i];
            totalEncontrados++;
        }
    }

    if (totalEncontrados > 0)
    {
        Ritmo[] resultadoFinal = new Ritmo[totalEncontrados];

        for (int i = 0; i < totalEncontrados; i++)
        {
            resultadoFinal[i] = ritmosEncontrados[i];
        }        

        return Results.Ok(new
        {
            nome,
            ritmos = ritmosEncontrados
        });
    } 

    return Results.NotFound(new
    {
        message = "Nenhum ritmo encontrado nesse título."
    });
});

app.Run();