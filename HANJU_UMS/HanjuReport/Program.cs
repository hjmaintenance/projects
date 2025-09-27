using DevExpress.Blazor.Reporting;
using HanjuReport;
using HanjuReport.Helpers;
using HanjuReport.Service;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using DevExpress.AspNetCore.Reporting;
//using Microsoft.EntityFrameworkCore;
using DevExpress.XtraReports.Web.Extensions;
using DevExpress.Security.Resources;
using DevExpress.XtraCharts;
using Microsoft.Extensions.Logging;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddDevExpressBlazor();
builder.Services.AddDevExpressServerSideBlazorReportViewer();
builder.Services.AddSingleton<RptService>();
//builder.Services.AddSingleton<RequestDelegate>();
//builder.Services.AddTransient<ExportMiddleware>();

builder.Services.AddTransient<ExportMiddleware>();
builder.Services.Configure<DevExpress.Blazor.Configuration.GlobalOptions>(options => {
    options.BootstrapVersion = DevExpress.Blazor.BootstrapVersion.v5;
});


builder.Services.AddDevExpressBlazorReporting();
builder.Services.AddScoped<ReportStorageWebExtension, CustomReportStorageWebExtension>();
builder.Services.ConfigureReportingServices(configurator => {
  configurator.ConfigureReportDesigner(designerConfigurator => {
  });
  configurator.ConfigureWebDocumentViewer(viewerConfigurator => {
    viewerConfigurator.UseCachedReportSourceBuilder();
    //viewerConfigurator.RegisterConnectionProviderFactory<CustomSqlDataConnectionProviderFactory>();
  });
  configurator.UseAsyncEngine();
});



builder.WebHost.UseWebRoot("wwwroot");
builder.WebHost.UseStaticWebAssets();

builder.Services.AddCors(options => {
  options.AddDefaultPolicy(builder =>
  {
    builder.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader();
  });
});




// Add session services
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options => {
  options.IdleTimeout = TimeSpan.FromMinutes(30);
  options.Cookie.HttpOnly = true;
  options.Cookie.IsEssential = true;
});


builder.Services.AddTransient<ExportMiddleware>();

//builder.Services.AddAntiforgery(options => options.SuppressXFrameOptionsHeader = true);

//log추가
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
//builder.Logging.AddFile("Logs/app-{Date}.log"); //  파일 로깅 추가

var app = builder.Build();



app.Use(async (context, next) => {
  context.Request.EnableBuffering(); // 요청 본문을 여러 번 읽을 수 있도록 활성화
  await next();
});

app.Use(async (context, next) => {
  await next();
});


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}



app.UseCors(); // Enable CORS


//app.MapPost("/documentviewer", (HttpContext context) => {
//  return Results.Ok("폼 요청 처리 완료");
//});



app.UseSession(); // Enable session

//app.UseMiddleware(typeof(ExportMiddleware));



//app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseMiddleware<ExportMiddleware>();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();