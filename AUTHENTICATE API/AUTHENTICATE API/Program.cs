using AUTHENTICATE_API.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Stripe;
using System.Configuration;
using static AUTHENTICATE_API.Models.AUTHENTICATEAPIDatabasesettings;

using AUTHENTICATE_API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System.Text;




    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.

    builder.Services.Configure<AUTHENTICATEAPIDatabasesettings>(
            builder.Configuration.GetSection(("AUTHENTICATEAPIDatabase")));

builder.Services.AddSingleton<apiDatabaseSettings>(sp =>
        sp.GetRequiredService<IOptions<AUTHENTICATEAPIDatabasesettings>>().Value);

    builder.Services.AddSingleton<AuthenticateService>();
//Adding JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
                options =>
                {
        options.TokenValidationParameters = new TokenValidationParameters //Vaalidating JWT Token on the basis of certain Parameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = "jwt:validIssuer",
            ValidAudience = "jwt:ValidAudince",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
            ClockSkew = TimeSpan.Zero
        };
    });
builder.Services.AddAuthorization();// Adding JWT Authorization (For Role Based Authorization)
builder.Services.AddControllers();
//ADDED CORS FUNCTIONALITY So that API can be easily accessed with CORS error 
builder.Services.AddCors(options => 
{
    options.AddPolicy(name: "AllowOrigin", ApplicationBuilder =>
    {        //  **** IMPORTANT                      //change this address to allow conections to API
        ApplicationBuilder.WithOrigins("http://localhost:4200").AllowAnyHeader().AllowAnyMethod();
    });
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
app.UseCors("AllowOrigin");//Using CORS
app.UseHttpsRedirection();
    app.UseAuthentication();// ****IMPORTANT****  Authentication Must be Declared before Authorization
    app.UseAuthorization();

    app.MapControllers();

    app.Run();

    