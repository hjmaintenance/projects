using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using JinRestApi.Data;
using JinRestApi.Endpoints;

var builder = WebApplication.CreateBuilder(args);


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                       ?? Environment.GetEnvironmentVariable("Help_JSINI");

builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));

// RabbitMQ 연결을 Singleton으로 등록
try
{
    var rabbitMqHostName = builder.Configuration["RabbitMQ:HostName"] ?? "localhost";
    IConnectionFactory factory = new ConnectionFactory() { HostName = rabbitMqHostName, DispatchConsumersAsync = true };
    var connection = factory.CreateConnection();
    builder.Services.AddSingleton(connection);
}
catch (RabbitMQ.Client.Exceptions.BrokerUnreachableException ex)
{
    // 시작 시 RabbitMQ에 연결할 수 없는 경우, 심각한 오류로 간주하고 애플리케이션을 시작하지 않습니다.
    // 이는 'fail-fast' 접근 방식으로, 서비스가 불완전한 상태로 실행되는 것을 방지합니다.
    // 콘솔에 오류를 기록하고 예외를 다시 던져서 앱 시작을 중단합니다.
    Console.WriteLine($"[ERROR] RabbitMQ connection failed: {ex.Message}. Application is shutting down.");
    throw;
}

// IHttpContextAccessor를 등록하여 서비스 내에서 HttpContext에 접근할 수 있도록 합니다.
builder.Services.AddHttpContextAccessor();

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

if (app.Environment.IsDevelopment())
{
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
