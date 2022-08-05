using System.Text.Json.Serialization;
using Application.Hubs;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();
app.UseCors(allowClientPolicy);
app.MapHub<ChatHub>("/chatHub");

app.Run();