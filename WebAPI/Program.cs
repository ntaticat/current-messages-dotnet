using System.Text.Json.Serialization;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using WebAPI.Hubs;

const string allowClientPolicy = "_allow_frontend_angular";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(opt =>
    opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<CurrentMessagesNetContext>(
    opt => opt.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )     
);

builder.Services.AddSignalR();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: allowClientPolicy, policy =>
    {       
        policy.WithOrigins("https://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    
        policy.WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
        policy.WithOrigins("http://192.168.0.156:4200")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

builder.Services.AddMediatR(typeof(Application.Commands.CreateChatCommand.Handler));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    try
    {
        var db = scope.ServiceProvider.GetRequiredService<CurrentMessagesNetContext>();
        db.Database.Migrate();
    }
    catch (System.Exception ex)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Database migration failed");
        throw;
    }
    
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "CurrentMessagesNet V1");
        c.RoutePrefix = "";
    });
}

// app.UseHttpsRedirection();

app.UseAuthorization();
app.UseCors(allowClientPolicy);

app.MapControllers();
app.MapHub<ChatHub>("/hubs/chat");

app.Run();