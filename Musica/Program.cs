using Models;
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

Musica[] musicas = new Musica[100];
int totalMusica = 0;

app.MapGet("/", () =>
{
    return Results.Ok("API Musica funcionando com sucesso!");
});

app.MapPost("/musica", (JsonElement body) =>
{
    Random random= new();
    Musica novaMusica = new Musica();

    novaMusica.Id = random.Next(1000,9999);
    novaMusica.Titulo = body.GetProperty("titulo").GetString();
    novaMusica.Artista = body.GetProperty("artista").GetString();
    novaMusica.Compositor = body.GetProperty("compositor").GetString();
    novaMusica.Genero = body.GetProperty("genero").GetString();
    novaMusica.Ano = body.GetProperty("ano").GetInt32();

    musicas[totalMusica] = novaMusica;
    totalMusica++;

    return Results.Ok(
        new{
            musica = novaMusica
        }
    );
});

app.MapGet("/musica", () =>
{
    Musica[] MusicasCadastradas = new Musica[totalMusica];

    for(int i = 0; i < totalMusica; i ++)
    {
        MusicasCadastradas[i] = musicas[i];
    }
    return Results.Ok(
        new{
            musicas = MusicasCadastradas
        }
    );

});

app.MapGet("/musica/{titulo}", (string titulo) =>
{
    Musica[] encontradas = new Musica[totalMusica];
    int totalEncontrados = 0;

    for(int i = 0; i < totalMusica; i++)
    {
        if (musicas[i].Titulo.ToLower() == titulo.ToLower())
        {
            encontradas[totalEncontrados] = musicas[i];
            totalEncontrados++;
        }
    }

    if (totalEncontrados > 0)
    {
        Musica[] resultadoFinal = new Musica[totalEncontrados];
        for (int i = 0; i < totalEncontrados; i++)
        {
            resultadoFinal[i] = encontradas[i];
        }
        return Results.Ok(
            new{
                titulo, musicas = resultadoFinal 
            }
        );
    }

    return Results.NotFound(new 
    {
            message = "Nenhuma música encontrada com este título." 
    });
});

app.MapPatch("/musica/{id}", (int id, JsonElement body) =>
{
    string novoTitulo = body.GetProperty("titulo").GetString();

    for (int i = 0; i < totalMusica; i++)
    {
        if (musicas[i].Id == id)
        {
            musicas[i].Titulo = novoTitulo;
            return Results.Ok(
                new{
                    mensagem = "Título modificado com sucesso.",
                    musica = musicas[i]
                }
            );
        }
    }

    return Results.NotFound(new
    {
        message = "Música não encontrada."
    });
});

app.MapDelete("/musica/{id}", (int id) =>
{
    for (int i = 0; i < totalMusica; i++)
    {
        if (musicas[i].Id == id)
        {
            Musica musicaRemovida = musicas[i];
            
            for (int j = i; j < totalMusica - 1; j++)
            {
                musicas[j] = musicas[j + 1];
            }            

            totalMusica--;

            return Results.Ok(new
            {
                mensagem = "Música removida com sucesso.",
                musica = musicaRemovida
            });
        }
    }

    return Results.NotFound(new
    {
        message = "Música não encontrada."
    });
});

app.MapGet("/musica/artista/{artista}", (string artista) =>
{
    Musica[] encontradas = new Musica[totalMusica];

    int totalEncontrados = 0;

    for (int i = 0; i < totalMusica; i++)
    {
        if (musicas[i].Artista.ToLower() == artista.ToLower())
        {
            encontradas[totalEncontrados] = musicas[i];
            totalEncontrados++;
        }
    }

    if (totalEncontrados > 0)
    {
        Musica[] resultadoFinal = new Musica[totalEncontrados];

        for (int i = 0; i < totalEncontrados; i++)
        {
            resultadoFinal[i] = encontradas[i];
        }        

        return Results.Ok(new
        {
            artista,
            musicas = resultadoFinal
        });
    } 

    return Results.NotFound(new
    {
        message = "Nenhuma música encontrada para esse artista."
    });
});

app.MapGet("/musica/compositor/{compositor}", (string compositor) =>
{
    Musica[] encontradas = new Musica[totalMusica];

    int totalEncontrados = 0;

    for (int i = 0; i < totalMusica; i++)
    {
        if (musicas[i].Compositor.ToLower() == compositor.ToLower())
        {
           encontradas[totalEncontrados] = musicas[i];
           totalEncontrados++;
        }
    }

    if (totalEncontrados > 0)
    {
        Musica[] resultadoFinal = new Musica[totalEncontrados];

        for (int i = 0; i < totalEncontrados; i++)
        {
            resultadoFinal[i] = encontradas[i];
        }        

        return Results.Ok(new
        {
            compositor,
            musicas = resultadoFinal
        });
    } 

    return Results.NotFound(new
    {
        message = "Nenhuma música encontrada para esse compositor."
    });
});

app.MapGet("/musica/genero/{genero}", (string genero) =>
{
    Musica[] encontradas = new Musica[totalMusica];

    int totalEncontrados = 0;

    for (int i = 0; i < totalMusica; i++)
    {
        if (musicas[i].Genero.ToLower() == genero.ToLower())
        {
            encontradas[totalEncontrados] = musicas[i];
            totalEncontrados++;
        }
    }

    if (totalEncontrados > 0)
    {
        Musica[] resultadoFinal = new Musica[totalEncontrados];

        for (int i = 0; i < totalEncontrados; i++)
        {
            resultadoFinal[i] = encontradas[i];
        }
        return Results.Ok(new 
        { 
            genero, musicas = resultadoFinal 
        });
    }

    return Results.NotFound(new 
    {
        message = "Nenhuma música encontrada para esse gênero." 
    });
});

app.MapGet("/musica/ano/{ano}", (int ano) =>
{
    Musica[] encontradas = new Musica[totalMusica];

    int totalEncontrados = 0;

    for (int i = 0; i < totalMusica; i++)
    {
        if (musicas[i].Ano == ano)
        {
            encontradas[totalEncontrados] = musicas[i];
            totalEncontrados++;
        }
    }

    if (totalEncontrados > 0)
    {
        Musica[] resultadoFinal = new Musica[totalEncontrados];

        for (int i = 0; i < totalEncontrados; i++)
        {
            resultadoFinal[i] = encontradas[i];
        }
        return Results.Ok(new 
        { 
            ano, musicas = resultadoFinal 
        });
    }

    return Results.NotFound(new
     { 
        message = "Nenhuma música encontrada para esse ano." 
    });
});

app.Run();