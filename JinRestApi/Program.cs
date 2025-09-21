using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.EntityFrameworkCore;
using JinRestApi.Data;
using JinRestApi.Endpoints;

var builder = WebApplication.CreateBuilder(args);


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
                       ?? Environment.GetEnvironmentVariable("Help_JSINI");

builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Authorization 미들웨어
builder.Services.AddAuthorization();

// JWT 
var jwtKey = builder.Configuration["Jwt:Key"] ?? "quristyle_blabbbbbla_secret_key_1234567890!@#$";
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "JinRestApi";

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});

var app = builder.Build();

if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

// endpoints 
app.MapRegistEndpoints();
//app.MapCompanyEndpoints();

// Endpoint 모듈 등록
app.MapCompanyEndpoints();
app.MapCustomerEndpoints();
app.MapAdminEndpoints();
app.MapTeamEndpoints();
app.MapRequestEndpoints();
app.MapCommentEndpoints();
app.MapAttachmentEndpoints();
app.MapDashboardEndpoints();



app.Run();
