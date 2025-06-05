using API.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDataContext>();
var app = builder.Build();


/*  1.	Criar Tarefa (POST)
        Rota: /api/tarefas
        Validações Essenciais: 
            titulo: Obrigatório e com no mínimo 3 caracteres.
            status: Campo obrigatório.
            data_vencimento: Obrigatório e no formato AAAA-MM-DD.
*/

app.MapPost("/api/tarefas", ([FromBody] Tarefa tarefa, [FromServices] AppDataContext ctx) =>
{
    ctx.Tarefa.Add(tarefa);
    ctx.SaveChanges();
    return Results.Created($"/api/tarefas/{tarefa.Id}", tarefa);

});

/*
    2.	Listar Tarefas (GET)
        Rota: /api/tarefas
        Funcionalidade: Retorna todas as tarefas registradas no sistema.
*/

app.MapGet("/api/tarefas", ([FromServices] AppDataContext ctx) => 
{
    var tarefa = ctx.Tarefa.ToList();
    return tarefa.Any() ? Results.Ok(tarefa):Results.NotFound();
});

/*
     3.	Obter Tarefa por ID (GET)
        Rota: /api/tarefas/{id}
        Funcionalidade: Recupera os detalhes de uma tarefa específica usando seu identificador único (id).
*/

app.MapGet("/api/tarefas/{id}", ([FromRoute] int Id, [FromServices] AppDataContext ctx) =>
{
    var tarefa = ctx.Tarefa.Find(Id);
    
    if(tarefa == null)
        return Results.NotFound("Nenhuma tarefa encontrada com este ID");

    return Results.Ok(tarefa);
});

/*
     4.	Atualizar Tarefa (PUT)
        Rota: /api/tarefas/{id}
        Validações: As mesmas regras de validação aplicadas na criação (POST) devem ser observadas.
        Funcionalidade: Permite a atualização de todos os campos de uma tarefa existente.
*/

app.MapPut("api/tarefas/{id}", ([FromRoute] int Id, [FromBody] Tarefa tarefaUpdate, [FromServices] AppDataContext ctx) =>
{
    var tarefa = ctx.Tarefa.Find(Id);

    if(tarefa == null)
        return Results.NotFound("Nenhuma tarefa encontrada com este ID");

    var status = ctx.Status.Find(tarefaUpdate.StatusId);
    if(status == null)
        return Results.NotFound();

    tarefa.StatusId = tarefaUpdate.StatusId;
    tarefa.Titulo = tarefaUpdate.Titulo;
    tarefa.DataVencimento = tarefaUpdate.DataVencimento;
    ctx.Tarefa.Update(tarefa);
    ctx.SaveChanges();
    return Results.Ok("Tarefa alterada com sucesso!");
    
    
});


/*
    5.	Deletar Tarefa (DELETE)
        Rota: /api/tarefas/{id}
        Funcionalidade: Remove uma tarefa do sistema com base no seu identificador.
*/

app.MapDelete("/api/tarefas/{id}", ([FromRoute] int Id, [FromServices] AppDataContext ctx) => 
{
    var tarefa = ctx.Tarefa.Find(Id);

    if(tarefa == null)
        return Results.NotFound("Não foi encontrado a Tarefa com este ID");

    ctx.Tarefa.Remove(tarefa);
    ctx.SaveChanges();
    return Results.Ok("Tarefa removida com sucesso!");
});
app.Run();
