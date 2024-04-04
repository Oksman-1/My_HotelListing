using AspNetCoreRateLimit;
using AutoMapper;
using CompanyEmployees.Presentation.ActionFilters;
using CompanyEmployees.Utility;
using Contracts;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Options;
using My_HotelListing.Extensions;
using My_HotelListing.Presentation.ActionFilters;
using Serilog;
using Service.DataShaping;
using Shared.DataTranferObjects;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

////Configure Serilog
//var logger = new LoggerConfiguration()
//	.ReadFrom.Configuration(builder.Configuration)
//	.Enrich.FromLogContext()
//	.CreateLogger();
	
//builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Host.UseSerilog();
//builder.Logging.AddSerilog(logger);

builder.Services.AddScoped<ValidationFilterAttribute>();
builder.Services.AddScoped<IDataShaper<HotelDto>, DataShaper<HotelDto>>();
builder.Services.AddScoped<ValidateMediaTypeAttribute>();
builder.Services.AddScoped<IHotelLinks, HotelLinks>();

builder.Services.AddControllers(config =>
{
	config.RespectBrowserAcceptHeader = true;
	config.ReturnHttpNotAcceptable = true;
	config.InputFormatters.Insert(0, GetJsonPatchInputFormatter());
	config.CacheProfiles.Add("120SecondsDuration", new CacheProfile { Duration = 120 });
})//.AddXmlDataContractSerializerFormatters()
  .AddCustomCSVFormatter()
  .AddApplicationPart(typeof(My_HotelListing.Presentation.AssemblyReference).Assembly);

builder.Services.AddCustomMediaTypes();

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
	options.SuppressModelStateInvalidFilter = true;	
});


builder.Services.ConfigureCors();
builder.Services.ConfigurIISIntegration();
builder.Services.ConfigureRepositoryManager();
builder.Services.ConfigureServiceManager();
builder.Services.ConfigureSqlContext(builder.Configuration);
builder.Services.ConfigureLoggerService();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.ConfigureVersioning();
builder.Services.ConfigureResponseCaching();
builder.Services.ConfigureHttpCacheHeaders();	
builder.Services.AddMemoryCache();
builder.Services.ConfigureRateLimitingOptions();
builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthentication();
builder.Services.ConfigureIdentity();
builder.Services.ConfigureJWT(builder.Configuration);
builder.Services.AddJwtConfiguration(builder.Configuration);
builder.Services.ConfigureSwagger();

NewtonsoftJsonPatchInputFormatter GetJsonPatchInputFormatter() =>
new ServiceCollection().AddLogging().AddMvc().AddNewtonsoftJson()
.Services.BuildServiceProvider()
.GetRequiredService<IOptions<MvcOptions>>().Value.InputFormatters
.OfType<NewtonsoftJsonPatchInputFormatter>().First();


var app = builder.Build();

var logger = app.Services.GetRequiredService<ILoggerManager>();
app.ConfigureExceptionHandler(logger);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
     app.UseHsts();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
	ForwardedHeaders = ForwardedHeaders.All	
});

app.UseIpRateLimiting();

app.UseCors("AllowAll");	

app.UseResponseCaching();	

app.UseHttpCacheHeaders();

app.UseAuthentication();

app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI(s =>
{
	s.SwaggerEndpoint("/swagger/v1/swagger.json", "Hotel List API v1");
	s.SwaggerEndpoint("/swagger/v2/swagger.json", "Hotel List API v2");
});

app.MapControllers();

app.Run();
