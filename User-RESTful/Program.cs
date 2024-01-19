using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore;
using Microsoft.OpenApi.Models;
using BE.DataAccess;
using BE.Services;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
{
    var services = builder.Services;

    services.AddSingleton<DataContext>();
    services.AddCors();
    services.AddRouting();
    services.AddControllers().AddJsonOptions(x =>
    {
        // serialize enums as strings in api responses (e.g. Role)
        x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());

        // ignore omitted parameters on models to enable optional params (e.g. User update)
        x.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });
    services.AddHttpContextAccessor();
    services.AddLogging();

    services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
    services.AddScoped<IUserService, UserService>();
    services.AddScoped<IUserRepository, UserRepository>();

    services.AddAuthorization();
    services.AddAuthentication("Bearer").AddJwtBearer();

    services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen(c =>
    {
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
        });
        c.AddSecurityRequirement(new OpenApiSecurityRequirement(){
    {
        new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
            }
        },
            new string[] { }
        }
        });
    });
}

var app = builder.Build();
{
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<DataContext>();
        await context.Init();
    }
    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.UseCors(x => x
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());

    app.MapControllers();

    app.Run();
}
