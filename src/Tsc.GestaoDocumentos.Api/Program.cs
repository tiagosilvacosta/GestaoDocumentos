using Tsc.GestaoDocumentos.Api;
using Tsc.GestaoDocumentos.Application;
using Tsc.GestaoDocumentos.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Gest�o Documentos API", Version = "v1" });
    
    // Configura��o para autentica��o JWT no Swagger
    c.AddSecurityDefinition("Bearer", new()
    {
        Description = "JWT Authorization header usando o esquema Bearer. \r\n\r\n Digite 'Bearer' [espa�o] e ent�o seu token na entrada de texto abaixo.\r\n\r\nExemplo: \"Bearer 12345abcdef\"",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new()
    {
        {
            new()
            {
                Reference = new()
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Add API services (includes JWT authentication)
builder.Services.AddApiServices(builder.Configuration);

// Add Application services
builder.Services.AddApplication();

// Add Infrastructure services
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Ordem importante: Authentication antes de Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
