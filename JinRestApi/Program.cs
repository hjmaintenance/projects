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

var app = builder.Build();

if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapUserEndpoints();

app.Run();
