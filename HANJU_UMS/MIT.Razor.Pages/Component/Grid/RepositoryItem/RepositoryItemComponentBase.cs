using MIT.DataUtil.Common;
using DevExpress.Blazor;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIT.Razor.Pages.Component.Grid.RepositoryItem {
  /// <summary>
  /// RepositoryItem 관련 기본 상속 클래스
  /// </summary>
  public class RepositoryItemComponentBase : CommonComponentBase {
    /// <summary>
    /// 그리드 컬럼 셀에 해당하는 클래스 정보
    /// </summary>
    [Parameter]
    public GridDataColumnCellDisplayTemplateContext? CellContext { get; set; }

    /// <summary>
    /// PrimaryKey 설정 여부
    /// PrimaryKey 설정시 SAVE_YN에서 'N'인 데이터는 편집 가능하고 'Y'인데이터는 편집 불가능
    /// </summary>
    [Parameter]
    public bool IsPrimaryKey { get; set; } = false;
    /// <summary>
    /// 편집 가능 유무
    /// </summary>
    [Parameter]
    public bool AllowEdit { get; set; } = true;
    /// <summary>
    /// 읽기 전용 유무 (Editor모양 유지)
    /// </summary>
    [Parameter]
    public bool ReadOnly { get; set; } = false;
    /// <summary>
    /// 편집 가능 유무 (Editor모양 유지)
    /// </summary>
    [Parameter]
    public bool Enable { get; set; } = true;

    /// <summary>
    /// 편집 가능 한지 체크
    /// </summary>
    /// <returns></returns>
    protected bool IsEditCheck() {
      if (CellContext == null)        return false;

      var focusRowIndex = CellContext.Grid.GetFocusedRowIndex();

      var isEditor = CellContext.VisibleIndex == focusRowIndex;
      var result = isEditor && ((IsPrimaryKey && CellContext.GetRowValue("SAVE_YN").ToStringTrim().Equals("N")) || (!IsPrimaryKey && AllowEdit));

      return result;
    }

    protected bool IsCheckValue() {
      bool result = false;
      var bobj = CellContext?.GetRowValue(CellContext.DataColumn.FieldName);
      if (bobj.GetType() == typeof(bool)) {
        result = (bool)bobj;
      }
      else {
        var bstr = bobj.ToStringTrim().ToLower();
        if(bstr == "y" || bstr == "true" || bstr == "check") {
          result = true;
        }
      }
      return result;
    }
  }
}
