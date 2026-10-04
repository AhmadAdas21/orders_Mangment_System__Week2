using System;
using Microsoft.EntityFrameworkCore;
using Week2_Task_2;
using Week2_Task_2.Data;
using Week2_Task_2.services;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<iservices, services>();
builder.Services.AddScoped<customer_services>();
builder.Services.AddScoped<order_services>();
builder.Services.AddScoped<iservices_reservation, reservation_service>();
builder.Services.AddHostedService<reservation_expiration_service>();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();


builder.Services.AddDbContext<data_base>(options =>options.UseSqlServer( builder.Configuration.GetConnectionString("DefaultConnection")));
var app = builder.Build();
app.UseExceptionHandler();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
public partial class Program
{

}
