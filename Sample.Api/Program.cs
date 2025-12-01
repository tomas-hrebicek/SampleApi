using Microsoft.EntityFrameworkCore;
using Sample.Api;
using Sample.Application;
using Sample.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Logging.AddConfiguration(builder.Configuration).AddConsole();

builder.Services.AddTokenAuthentication(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration.GetConnectionString("Default"));
builder.Services.AddApplicationLayer();

builder.Services.AddValidatedControllers(options =>
{
    options.ModelBinderProviders.Insert(0, new ModelBinderProvider());
}).AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    options.JsonSerializerOptions.Converters.Add(new IsoDateTimeConverter());
    options.JsonSerializerOptions.NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString;
});

builder.Services.AddVersioning();

builder.Services.AddRouting(options => options.LowercaseUrls = true);

builder.Services.AddSwagger();

builder.Services.AddHealthChecks().AddCheck<HealthCheck>("basic");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseHsts();
}

// TODO: builder.Services.AddExceptionHandler<ExceptionHandler>(); (see topkioskapi)
app.UseMiddleware<SampleApiExceptionHandlerMiddleware>();

app.UseSwaggerVersionedUI(app.Services);

app.UseHttpsRedirection();

app.UseCors(app.Configuration);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

#if DEBUG

MigrationProvider.TryUpdate(app.Services);

#endif

app.Run();
