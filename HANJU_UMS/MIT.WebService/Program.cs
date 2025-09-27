using MIT.WebService.Services.Database;
using MIT.WebService.Middlewares;
using Newtonsoft.Json;
using MIT.WebService.Services;
using Newtonsoft.Json.Serialization;
using MIT.ServiceModel;
//using MIT.Devexp.ThemeSwitcher;
//using MIT.Devexp;
using Microsoft.AspNetCore.HttpOverrides;
using MIT.Razor.Pages.Common;

// 웹 어플리케이션/서비스 작성하기 위한 WebApplicationBuilder 생성
var builder = WebApplication.CreateBuilder(args);



// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();
builder.Services.AddMvc();
builder.Services.AddDevExpressBlazor(options => {
  options.BootstrapVersion = DevExpress.Blazor.BootstrapVersion.v5;
  options.SizeMode = DevExpress.Blazor.SizeMode.Small;
});

builder.Services.AddRazorComponents().AddInteractiveServerComponents();
builder.Services.AddHttpContextAccessor();
//builder.Services.AddScoped<ThemeService>();

builder.Services.AddScoped<AppData>();


//builder.Services.AddSingleton<ThemeService>();
//builder.WebHost.UseWebRoot("wwwroot");
builder.WebHost.UseStaticWebAssets();



// 컨트롤러 서비스 추가 및 Message Json 인코딩 서비스 추가
builder.Services.AddControllers().AddNewtonsoftJson(o =>
{
    o.SerializerSettings.Formatting = Formatting.Indented; // 들여쓰기
    o.SerializerSettings.NullValueHandling = NullValueHandling.Include; // Null 값 포함
    o.SerializerSettings.ContractResolver = new DefaultContractResolver(); // Json key 정보 대소문자 그대로 변환
    o.SerializerSettings.Converters.Add(new HttpJsonConverter()); // DataSet 정보 Custom Json 변환
});

// HttpContext 서비스 추가
builder.Services.AddHttpContextAccessor();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

// API 주석 설명 
builder.Services.AddEndpointsApiExplorer();
// Swagger 서비스 추가
builder.Services.AddSwaggerGen();

builder.Services.AddMvc();


#region 유저 서비스 등록 

// Mssql 데이터 베이스 서비스 추가
builder.Services.AddTransient<MSSQLDatabaseService>();
// Util 서비스 추가
builder.Services.AddScoped<IUtilService, UtilService>();
// Jwtoken 인증 서비스 추가
builder.Services.AddScoped<IJwtService, JwtService>();
// 유저 관련 서비스 추가
builder.Services.AddScoped<IAccountService, AccountService>();

#endregion 유저 서비스 등록 


// 등록정보 빌드
var app = builder.Build();

// Debug시 SwaggerUI 호출가능
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Swagger 사용
    app.UseSwagger();
    app.UseSwaggerUI();
}

#region Cors 정보 등록
// Cors 정보 등록
app.UseCors(cors => cors
               //.WithOrigins("http://mitddns02.iptime.org:15000") // 운영 Origins
               .WithOrigins("http://localhost:44401") // 개발 Origins
               //.WithOrigins("http://localhost:5000")
               //.WithOrigins("https://localhost:5001")
               .AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader()
               //.WithMethods("POST")
               //.AllowCredentials()
               //.WithHeaders("Content-Type", "Content-Length", "Accept-Encoding", "Connection", "Accept", "User-Agent", "Host", "Authorization")
           );
#endregion Cors 정보 등록

#region 사용자 미들웨어 등록

// Jwtoken 인증 미들웨어 사용 등록
app.UseMiddleware<JWTokenMiddleware>();

#endregion 사용자 미들웨어 등록



// address middlerware
app.UseForwardedHeaders(new ForwardedHeadersOptions {
  ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});
// address middlerware end






//app.UseHttpsRedirection();

//app.UseAuthorization();
// 컨트롤러 Endpoint 셋팅
app.MapControllers();
// 앱실행
app.Run();

