using MIT.Launcher;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.Authorization;
using MIT.Razor.Pages.Service;
using MIT.Razor.Pages.Component.MessageBox;
using MIT.UI.Main.MainFrame;
using System.Net;
using MIT.Razor.Pages.Component.LoadingPanel;
using DevExpress.PivotGrid.Design;
using System.ComponentModel.Design;
using MIT.Razor.Pages.Component.Popup;
using System.Net.NetworkInformation;
using MIT.Razor.Pages.Common;
//using MIT.Devexp.ThemeSwitcher;
//using MIT.Devexp;

//  웹 어셈블리 어플리케이션/서비스 작성하기 위한 WebAssemblyHostBuilder 생성
var builder = WebAssemblyHostBuilder.CreateDefault(args);
// html에서 app id 찾아 App 클래스 정보 셋팅


builder.RootComponents.Add<App>("#app");
//builder.RootComponents.Add<App>("body");
// html에서 head 정보 끝에 HeadOutlet 클래스 정보 셋팅
builder.RootComponents.Add<HeadOutlet>("head::after");

// HttpClient 서비스 등록
builder.Services.AddSingleton(sp => { return new HttpClient(); });

// HttpService 서비스 등록
builder.Services.AddSingleton<IHttpService, HttpService>();
// QueryService 서비스 등록
builder.Services.AddSingleton<IQueryService, QueryService>();
// SessionStorageService 서비스 등록
builder.Services.AddSingleton<ISessionStorageService, SessionStorageService>();
// AccountService 서비스 등록
builder.Services.AddSingleton<IAccountService, AccountService>();

builder.Services.AddSingleton<IConfigurationService, ConfigurationService>();

// MessageBoxService 서비스 등록
builder.Services.AddSingleton<ICommonMessageBoxService, CommonMessageBoxService>();
// MainFrameService 서비스 등록
builder.Services.AddSingleton<IMainFrameService, MainFrameService>();
// LoadingPanelService 서비스 등록
builder.Services.AddSingleton<ILoadingPanelService, LoadingPanelService>();
// CommonPopupService 서비스 등록
builder.Services.AddSingleton<ICommonPopupService, CommonPopupService>();


builder.Services.AddScoped<AppData>();
//builder.Services.AddScoped<ThemeService>();

// Authorization 관련 서비스 등록
builder.Services.AddAuthorizationCore();



// Devexpress FrameWork 관련 서비스 등록
builder.Services.AddDevExpressBlazor(options => {
    options.BootstrapVersion = DevExpress.Blazor.BootstrapVersion.v5;
    options.SizeMode = DevExpress.Blazor.SizeMode.Small;
});

// 유저정보 권한 체크 서비스 등록
builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();




var app = builder.Build();
//app.UseCors("DevCors");

var configService = app.Services.GetRequiredService<IConfigurationService>();

#if DEBUG
configService.Set("DefaultRoute", "");
#else
    configService?.Set("DefaultRoute", "/web");
#endif

// HttpService Http 연결 주소 셋팅
var httpService = app.Services.GetService<IHttpService>();
//(httpService as HttpService)?.AddUri("BaseAddress", "http://localhost:5276/api/"); // 개발
//(httpService as HttpService)?.AddUri("BaseAddress", "http://192.168.0.246:5276/api/"); // 개발
//(httpService as HttpService)?.AddUri("BaseAddress", "https://nums.api.hanjucorp.co.kr/api/"); // 운영
(httpService as HttpService)?.AddUri("BaseAddress", "http://localhost:7000/api/"); // 개발
//(httpService as HttpService)?.AddUri("BaseAddress", "https://localhost:7210/"); // 로컬 샘플 테스트 api
//(httpService as HttpService)?.AddUri("BaseAddress", "http://localhost:9000/api/"); // 로컬
//(httpService as HttpService)?.AddUri("BaseAddress", "http://mitddns02.iptime.org:50010/api/"); // 로컬
//(httpService as HttpService)?.AddUri("BaseAddress", "http://222.104.110.43:50010/api/"); // 운영



// 앱 실행
await app.RunAsync();
