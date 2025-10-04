using Server.API.Extensions;
using Server.API.Middlewares;
using Server.Application;
using Server.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// --------- DI registrations ----------

builder.Services.AddApi(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

var app = builder.Build();

// --------- Middleware Pipeline ----------

app.UseMiddleware<ExceptionHandlingMiddleware>();

if (!app.Environment.IsDevelopment())
    app.UseHttpsRedirection();

app.UseCors("FrontendPolicy");

app.MapGet("/", () => "Hello World!");

app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

// ---------- Run ----------
app.Run();