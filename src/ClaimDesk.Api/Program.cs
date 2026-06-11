using ClaimDesk.Api.Infrastructure;
using ClaimDesk.Application.Abstractions;
using ClaimDesk.Infrastructure.Persistence;
using ClaimDesk.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();

builder.Services.AddDbContext<ClaimDeskDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("ClaimDeskDatabase")
        ?? "Data Source=claimdesk.local.db";
    options.UseSqlite(connectionString);
});

builder.Services.AddScoped<ICurrentUserContext, HeaderCurrentUserContext>();
builder.Services.AddScoped<IClaimService, ClaimService>();
builder.Services.AddScoped<IClaimActivityNoteService, ClaimActivityNoteService>();

var app = builder.Build();

app.UseMiddleware<DeveloperExceptionResponseMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ClaimDeskDbContext>();
    await DbSeeder.SeedAsync(dbContext);
}

app.Run();

public partial class Program;
