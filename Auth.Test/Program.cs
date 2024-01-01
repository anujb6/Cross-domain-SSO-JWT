using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Auth.Test;
using Auth.Test.Controllers;
using Swashbuckle.AspNetCore.Filters;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

builder.Services.AddSwaggerGen(options => {
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard Authorization header using the Bearer scheme (\"bearer {token}\")",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("oLOMxbL81N3ouIWWAb6AvkU6V88l0hOF")),
        ValidateIssuer = false,
        ValidateAudience = false
    };

});

builder.Services.AddTransient<WebSocket>();
builder.Services.AddTransient<AuthController>();

builder.Services.AddSignalR();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowOrigin", builder =>
    {
        builder
        .WithOrigins("http://localhost:57054", "http://localhost:4200", "https://localhost:7225")
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials();
    });
});


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("AllowOrigin");

app.UseAuthentication();
app.UseAuthorization(); 

app.MapControllers();

app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<WebSocket>("/websocket");
});

app.Run();
