using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.EntityFrameworkCore;
using JinRestApi.Data;
using JinRestApi.Endpoints;

var builder = WebApplication.CreateBuilder(args);

// 환경별 appsettings.json 자동 로드됨

/* local 개발환경 실행시
export DOTNET_ENVIRONMENT=Development
dotnet run
*/

/* ubuntu systemd
[Service]
Environment=DOTNET_ENVIRONMENT=Production
*/

/*
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
*/

// 환경변수에서 직접 읽기 (없으면 appsettings.json fallback)

/*

linux, macos

export POSTGRES_CONNECTION="Host=localhost;Port=5432;Database=dev_db;Username=dev_user;Password=secret_pw"
dotnet run

// windows powershell

setx POSTGRES_CONNECTION "Host=localhost;Port=5432;Database=dev_db;Username=dev_user;Password=secret_pw"

// ubuntu systemd

[Service]
Environment=DOTNET_ENVIRONMENT=Production
Environment=POSTGRES_CONNECTION=Host=localhost;Port=5432;Database=prod_db;Username=prod_user;Password=secret_pw


sudo systemctl daemon-reload
sudo systemctl restart myrestapi



*/


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
                       ?? Environment.GetEnvironmentVariable("Help_JSINI");


builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));




builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Authorization 미들웨어
builder.Services.AddAuthorization();



// JWT 시크릿키(실서비스는 환경변수 등 안전한 곳에 보관)
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

app.MapUserEndpoints();

app.Run();
