using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace MIT.UI.Main.MainFrame {
  /// <summary>
  /// 메인메뉴에서 선택한 프로그램 UI 추가/ 포커스 변경 이벤트 
  /// MainFrame에서 사용
  /// 사용자 사용금지
  /// </summary>
  public interface IMainFrameSenderService {
    /// <summary>
    /// 프로그램 UI 추가 이벤트 셋팅
    /// MainFrame.razor에서 사용
    /// 사용자 사용금지
    /// </summary>
    event Action<MainFrameData>? OnOpenPage;
    /// <summary>
    /// 프로그램 UI 페이지가 포커스 변경되었을때 이벤트 함수 호출
    /// MainFrame.razor에서 사용
    /// 사용자 사용금지
    /// </summary>
    /// <param name="page"></param>
    void PageFocusedChanged(MainFrameData? page);
  }

  /// <summary>
  /// 메인메뉴에서 선택한 프로그램 UI 추가/삭제를 위한 DI 서비스 인터페이스
  /// </summary>
  public interface IMainFrameService {
    /// <summary>
    /// 사용자가 지정한 프로그램 UI 페이지로 포커스 변경 되었을때 호출 이벤트 셋팅
    /// </summary>
    event Action<MainFrameData?> PageChanged;

    /// <summary>
    /// 프로그램 UI 페이지 열기 함수
    /// </summary>
    /// <param name="PGM_ID"></param>
    /// <param name="PGM_CLASS"></param>
    /// <param name="PGM_PATH"></param>
    /// <param name="MENU_ID"></param>
    /// <param name="MENU_NAME"></param>
    void OpenPage(string? PGM_ID, string? PGM_CLASS, string? PGM_PATH, string? MENU_ID, string? MENU_NAME);
  }

  /// <summary>
  /// 메인메뉴에서 선택한 프로그램 UI 추가/삭제를 위한 DI 서비스 구현 클래스
  /// </summary>
  public class MainFrameService : IMainFrameService, IMainFrameSenderService {
    /// <summary>
    /// 프로그램 UI 추가 이벤트
    /// </summary>
    public event Action<MainFrameData>? OnOpenPage;
    /// <summary>
    /// 사용자가 지정한 프로그램 UI 페이지로 포커스 변경 이벤트
    /// </summary>
    public event Action<MainFrameData?>? PageChanged;

   // private static readonly Dictionary<string, Assembly> _assemblyCache = new();

    // 멀티스레드 안전한 캐시
    //private static readonly ConcurrentDictionary<string, Assembly> _assemblyCache = new();

    /// <summary>
    /// 프로그램 UI 페이지 열기 함수
    /// </summary>
    /// <param name="PGM_ID"></param>
    /// <param name="PGM_CLASS"></param>
    /// <param name="PGM_PATH"></param>
    /// <param name="MENU_ID"></param>
    /// <param name="MENU_NAME"></param>
    public void OpenPage(string? PGM_ID, string? PGM_CLASS, string? PGM_PATH, string? MENU_ID, string? MENU_NAME) {


      //JSRuntime?.InvokeVoidAsync("alert", PGM_PATH);

      //JS.InvokeVoidAsync("alert", "SizeSwitcherItem_Click!" );
      
     
          Console.WriteLine($"OpenPage PGM_ID: {PGM_ID}");
          Console.WriteLine($"OpenPage PGM_CLASS: {PGM_CLASS}");
          Console.WriteLine($"OpenPage PGM_PATH: {PGM_PATH}");
          Console.WriteLine($"OpenPage MENU_ID: {MENU_ID}");
          Console.WriteLine($"OpenPage MENU_NAME: {MENU_NAME}");



      if (string.IsNullOrEmpty(PGM_CLASS) || string.IsNullOrEmpty(PGM_PATH)) return;


      //if (!_assemblyCache.TryGetValue(PGM_PATH, out var assem)) {
      //  assem = Assembly.Load(PGM_PATH);
      //  _assemblyCache[PGM_PATH] = assem;
      //}

      //var assem = _assemblyCache.GetOrAdd(PGM_PATH, path => Assembly.Load(path));

      var classType = Type.GetType($"{PGM_CLASS}, {PGM_PATH}");// assem.GetType(PGM_CLASS);
      if (classType == null) {
          Console.WriteLine($"OpenPage 널이라서 나감???: {MENU_NAME}");
        return;
      }
          Console.WriteLine($"OpenPage 발견함: {MENU_NAME}");

      //var assem = Assembly.Load(PGM_PATH);
      //var classType = assem.GetType(PGM_CLASS);

      MainFrameData data = new MainFrameData() {

        ClassType = classType,
        PGM_ID = PGM_ID,
        PGM_CLASS = PGM_CLASS,
        PGM_PATH = PGM_PATH,
        MENU_ID = MENU_ID,
        MENU_NAME = MENU_NAME,
      };

      OnOpenPage?.Invoke(data);
    }

    /// <summary>
    /// 사용자가 지정한 프로그램 UI 페이지로 포커스 변경 함수
    /// </summary>
    /// <param name="page"></param>
    public void PageFocusedChanged(MainFrameData? page) {
      //return;
      if (PageChanged != null) {
        PageChanged(page);
      }
    }
  }
}
