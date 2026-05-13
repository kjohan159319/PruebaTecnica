using CoreWCF;
using CoreWCF.Configuration;
using CoreWCF.Description;
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
using GestionUsuarios.WCF.Servicios;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

builder.Services.AddServiceModelServices();
builder.Services.AddServiceModelMetadata();
builder.Services.AddAutoMapper(typeof(UsuarioPerfil));

builder.Services.AddSingleton<IFabricaConexionSql, FabricaConexionSqlServer>();
builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();
builder.Services.AddScoped<IUsuarioDominio, UsuarioDominio>();
builder.Services.AddScoped<IUsuarioAplicacion, UsuarioAplicacion>();
builder.Services.AddScoped<IValidator<UsuarioDto>, UsuarioDtoValidacion>();

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();

app.UseServiceModel(serviceBuilder =>
{
    serviceBuilder.AddService<UsuarioServicioWCF>();
    serviceBuilder.AddServiceEndpoint<UsuarioServicioWCF, IUsuarioServicioWCF>(
        new BasicHttpBinding(),
        "/UsuarioServicio");
});

var serviceMetadataBehavior = app.Services.GetRequiredService<ServiceMetadataBehavior>();
serviceMetadataBehavior.HttpGetEnabled = true;

app.MapRazorPages();
app.MapGet("/", () => Results.Redirect("/Usuario"));

app.Run();
