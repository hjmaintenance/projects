using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIT.Razor.Pages.Common {
  /// <summary>
  /// 메인 메뉴 버튼 권한에 필요한 정보 클래스
  /// </summary>
  public class MainCommonButtonData {
    /// <summary>
    /// 조회 버튼 권한 유무
    /// </summary>
    public bool IsButtonSearch { get; set; } = false;
    //public bool IsButtonAdd { get; init; } = false;
    /// <summary>
    /// 저장 버튼 권한 유무
    /// </summary>
    public bool IsButtonSave { get; set; } = false;
    /// <summary>
    /// 삭제 버튼 권한 유무
    /// </summary>
    public bool IsButtonDelete { get; set; } = false;
    /// <summary>
    /// 출력 버튼 권한 유무
    /// </summary>
    public bool IsButtonPrint { get; set; } = false;
    public bool IsRoleCehck { get; set; } = false;
    public string MENU_ID { get; set; } = string.Empty;
    /// <summary>
    /// 검색 버튼 클릭 이벤트
    /// </summary>
    public EventCallback SearchButtonClick { get; set; }
    /// <summary>
    /// 저장 버튼 클릭 이벤트
    /// </summary>
    public EventCallback SaveButtonClick { get; set; }
    /// <summary>
    /// 삭제 버튼 클릭 이벤트
    /// </summary>
    public EventCallback DeleteButtonClick { get; set; }
    /// <summary>
    /// 출력 버튼 클릭 이벤트
    /// </summary>
    public EventCallback PrintButtonClick { get; set; }
    /// <summary>
    /// 닫기 버튼 클릭 이벤트
    /// </summary>
    public EventCallback CloseButtonClick { get; set; }
    /// <summary>
    /// 즐겨찾기 버튼 클릭 이벤트
    /// </summary>
    public EventCallback FavoritesButtonClick { get; set; }
  }
}
