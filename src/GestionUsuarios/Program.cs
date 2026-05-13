using FluentValidation;
using GestionUsuarios.Capas.Aplicacion.Dto;
using GestionUsuarios.Capas.Aplicacion.Dto.Validaciones;
using GestionUsuarios.Capas.Aplicacion.Interfaz;
using GestionUsuarios.Capas.Aplicacion.Mapeadores;
using GestionUsuarios.Capas.Aplicacion.Principal;
using GestionUsuarios.Capas.Dominio.Core;
using GestionUsuarios.Capas.Dominio.Interfaz;
using GestionUsuarios.Capas.Infraestructura.Interfaz;
using GestionUsuarios.Capas.Infraestructura.Repositorios;
using GestionUsuarios.Capas.Transversal.Fabricas;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(UsuarioPerfil));

builder.Services.AddSingleton<IFabricaConexionSql, FabricaConexionSqlServer>();
builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();
builder.Services.AddScoped<IUsuarioDominio, UsuarioDominio>();
builder.Services.AddScoped<IUsuarioAplicacion, UsuarioAplicacion>();
builder.Services.AddScoped<IValidator<UsuarioDto>, UsuarioDtoValidacion>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "GestionUsuarios API");
    options.RoutePrefix = string.Empty;
});

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
