using Microsoft.Extensions.Options;
using static StorePasswordSecurly.Models.AUTHENTICATEAPIDatabasesettings;
using StorePasswordSecurly.Models;
using StorePasswordSecurly.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.Configure<AUTHENTICATEAPIDatabasesettings>(
        builder.Configuration.GetSection(("AUTHENTICATEAPIDatabase")));

builder.Services.AddSingleton<apiDatabaseSettings>(sp =>
        sp.GetRequiredService<IOptions<AUTHENTICATEAPIDatabasesettings>>().Value);

builder.Services.AddSingleton<AuthenticateService>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowOrigin", ApplicationBuilder =>
    {        //  **** IMPORTANT                      //change this address to allow conections to API
        ApplicationBuilder.WithOrigins("http://localhost:4200").AllowAnyHeader().AllowAnyMethod();
    });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowOrigin");//Using CORS

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
