using DevExpress.Blazor;
using DevExpress.XtraReports.UI;
using DevExtreme.AspNet.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MIT.DataUtil.Common;
using MIT.Razor.Pages.Component.Grid.Data;
using MIT.Razor.Pages.Component.MessageBox;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using static DevExpress.XtraPrinting.Native.ExportOptionsPropertiesNames;
using Newtonsoft.Json;
using MIT.ServiceModel;
using System.Reflection;
using Microsoft.SqlServer.Server;

namespace MIT.Razor.Pages.Component.Grid {
  public class CommonGridBase : CommonUIComponentBase {
    /// <summary>
    /// Razor Component 데이터 컬럼 셋팅
    /// </summary>
    [Parameter]
    public RenderFragment? DataColumnsTemplate { get; set; }
    [Parameter]
    public RenderFragment? BeginToolbarTemplate { get; set; }
    [Parameter]
    public RenderFragment? AfterToolbarTemplate { get; set; }
    [Parameter]
    public RenderFragment? TotalSummaryTemplate { get; set; }
    //[Parameter]
    //public RenderFragment? BandedHeaderTemplate { get; set; }

    /// <summary>
    /// 그리드 컬럼 셋팅 데이터
    /// </summary>
    [Parameter]
    public ObservableCollection<CommonGridDataColumnAttribute> Columns { get; set; } = new ObservableCollection<CommonGridDataColumnAttribute>();

    /// <summary>
    /// 그리드 체크 버튼 보이기
    /// </summary>
    [Parameter]
    public bool IsCheckBox { get; set; }
    /// <summary>
    /// 그리드 체크 버튼 오른쪽으로 표시
    /// </summary>
    [Parameter]
    public bool IsCheckBoxRightPosition { get; set; } = false;
    /// <summary>
    /// 그리드 컨트롤 전체 가로 크기
    /// </summary>
    [Parameter]
    public int ControlWidth { get; set; } = 600;
    /// <summary>
    /// 그리드 컨트롤 전체 세로 크기
    /// </summary>
    [Parameter]
    public int ControlHeight { get; set; } = 500;
    /// <summary>
    /// 그리드 레이아웃 가로 크기
    /// </summary>
    [Parameter]
    public int GridLayoutWidth { get; set; } = 400;
    /// <summary>
    /// 그리드 레이아웃 세로 크기
    /// </summary>
    [Parameter]
    public int GridLayoutHeight { get; set; } = 364;
    /// <summary>
    /// 실제 그리드 가로 크기
    /// </summary>
    [Parameter]
    public int GridWidth { get; set; } = 399;
    /// <summary>
    /// 실제 그리드 세로 크기
    /// </summary>
    [Parameter]
    public int GridHeight { get; set; } = 356;
    /// <summary>
    /// 그리드 가로 사이즈 조절 자동으로 선택
    /// </summary>
    [Parameter]
    public bool IsLayoutAuto { get; set; } = true;
    /// <summary>
    /// 그리드 CSS 스타일 
    /// </summary>
    [Parameter]
    public string? CssClass { get; set; }
    /// <summary>
    /// 툴바 보이기
    /// </summary>
    [Parameter]
    public bool IsShowToolBar { get; set; } = false;

    /// <summary>
    /// Excel Export 파일명
    /// </summary>
    [Parameter]
    public string ExportFileName { get; set; } = string.Empty;
    /// <summary>
    /// 익스포트 제외 컬럼 지정
    /// </summary>
    [Parameter]
    public string[] ExcludeColumnsOnExport { get; set; } = Array.Empty<string>();

    /// <summary>
    /// 툴바 조회 쿼리명
    /// </summary>
    [Parameter]
    public string SearchQueryName { get; set; } = string.Empty;

    /// <summary>
    /// 툴바 저장 쿼리명
    /// </summary>
    [Parameter]
    public string SaveQueryName { get; set; } = string.Empty;
    /// <summary>
    /// 툴바 삭제 쿼리명
    /// </summary>
    [Parameter]
    public string DeleteQueryName { get; set; } = string.Empty;

    /// <summary>
    /// 툴바 조회 버튼 보이기
    /// </summary>
    [Parameter]
    public bool IsSearchButtonEnabled { get; set; } = false;
    /// <summary>
    /// 툴바 추가 버튼 보이기
    /// </summary>
    [Parameter]
    public bool IsAddButtonEnabled { get; set; } = false;
    /// <summary>
    /// 툴바 저장 버튼 보이기
    /// </summary>
    [Parameter]
    public bool IsSaveButtonEnabled { get; set; } = false;
    /// <summary>
    /// 툴바 삭제 버튼 보이기
    /// </summary>
    [Parameter]
    public bool IsDeleteButtonEnabled { get; set; } = false;
    /// <summary>
    /// 툴바 Excel Export 버튼 보이기
    /// </summary>
    [Parameter] public bool IsExportButtonEnabled { get; set; } = true;
    [Parameter] public bool IsExportPDFEnabled { get; set; } = true;
    [Parameter] public bool IsExportXlsxEnabled { get; set; } = true;
    [Parameter] public bool IsExportDocEnabled { get; set; } = true;
    [Parameter] public bool IsExportImageEnabled { get; set; } = true;


    [Parameter]    public GridEditorRenderMode GrdRenderMode { get; set; } = GridEditorRenderMode.Integrated;


    /// <summary>
    /// 그리드 체크시 멀티 싱글 선택
    /// </summary>
    [Parameter]    public GridSelectionMode SelectionMode { get; set; } = GridSelectionMode.Multiple;

    /// <summary>
    /// 그리드 포커스 체인지 이벤트
    /// </summary>
    [Parameter]
    public EventCallback<GridFocusedRowChangedEventArgs> FocusedRowChanged { get; set; } //
    /// <summary>
    /// 그리드 로우 클릭 이벤트
    /// </summary>
    [Parameter]    public EventCallback<GridRowClickEventArgs> RowClick { get; set; } //



    [Parameter] public EventCallback<object> SelectedChange { get; set; } //


    [Parameter] public bool FocusedRowEnabled { get; set; } = true; //

    



    /// <summary>
    /// 그리드 로우 더블 클릭 이벤트
    /// </summary>
    [Parameter]
    public EventCallback<GridRowClickEventArgs> RowDoubleClick { get; set; } //
    /// <summary>
    /// 그리드 헤더 로우 셀 스타일 적용 이벤트
    /// </summary>
    [Parameter]
    public EventCallback<GridCustomizeElementEventArgs> CustomizeElement { get; set; } //
    /// <summary>
    /// 조회 쿼리 전에 Parameter 셋팅 하는 이벤튼
    /// </summary>
    [Parameter]
    public EventCallback<Dictionary<string, object?>> InputSearchParameter { get; set; } //
    /// <summary>
    /// 그리드 새로운 행추가 전에 로우 셋팅 이벤트
    /// </summary>
    [Parameter]
    public EventCallback<CommonGridInitNewRowEventArgs> InitNewRowChanging { get; set; } //

    /// <summary>
    /// 툴바 조회 버튼 클릭 이벤트
    /// </summary>
    [Parameter]
    public EventCallback SearchButtonClick { get; set; } //
    /// <summary>
    /// 툴바 저장 버튼 클릭 이벤트
    /// </summary>
    [Parameter]
    public EventCallback SaveButtonClick { get; set; } //
    /// <summary>
    /// 툴바 삭제 버튼 클릭 이벤트
    /// </summary>
    [Parameter]
    public EventCallback DeleteButtonClick { get; set; } //

    /// <summary>
    /// 그리드 데이터 셋팅
    /// </summary>
    public DataTable? DataSource {
      get { return _datatable; }
      set {
        InitGridColumnsSetting(value);

        FocusedRowIndex = -1;
        FocusedRow = null;

        _datatable = value;

        StateHasChanged();
      }
    }

    /// <summary>
    /// 그리드 클래스 정보
    /// </summary>
    public DxGrid? Grid { get; protected set; }


    public DxToolbar Toolbar_items { get; protected set; }




    /// <summary>
    /// 체크 필드 고정
    /// </summary>
    protected string CheckedFieldName { get; set; } = "CHK";
    /// <summary>
    /// 그리드에 적용될 데이터 테이블
    /// </summary>
    protected DataTable? _datatable;
    /// <summary>
    /// 필수 컬럼 정보
    /// </summary>
    protected List<string> NeedColumns { get; set; } = new List<string>();
    /// <summary>
    /// 그리드 포커스된 로우 인덱스
    /// </summary>
    protected int FocusedRowIndex { get; set; } = -1;
    /// <summary>
    /// 그리드 포커스된 로우 데이터
    /// </summary>
    protected DataRow? FocusedRow { get; set; }

    #region 그리드 자체 체크 모드
    /// <summary>
    /// 헤더 체크 값 셋팅
    /// </summary>
    protected bool IsAllChecked { get; set; } = false;
    #endregion 그리드 자체 체크 모드





    [Parameter]    public Dictionary<string, string>? DataCellCss { get; set; } = new Dictionary<string, string>();
    [Parameter]    public Dictionary<string, string>? DataRowCss { get; set; } = new Dictionary<string, string>();






    /// <summary>
    /// 그리드 생성시 초기화 함수
    /// </summary>
    protected override void OnInitialized() {
      FocusedRowIndex = -1;
      FocusedRow = null;

     // GrdRenderMode = GridEditorRenderMode.Detached;


    }

    /// <summary>
    /// 컬럼 RepositoryItem 데이터 셋팅
    /// </summary>
    /// <param name="arribute"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    protected RenderFragment? RenderRepositoryItem(CommonGridRepositoryItemAttribute arribute, GridDataColumnCellDisplayTemplateContext context) {
      if (arribute.ObjectType == null)
        return null;

      RenderFragment renderPage = b => {
        int seq = 0;
        b.OpenComponent(seq++, arribute.ObjectType);
        b.AddAttribute(seq++, "CellContext", context);
        b.AddAttribute(seq++, "IsPrimaryKey", arribute.IsPrimaryKey);
        b.AddAttribute(seq++, "AllowEdit", arribute.AllowEdit);
        b.AddAttribute(seq++, "ReadOnly", arribute.ReadOnly);
        b.AddAttribute(seq++, "Enable", arribute.Enable);

        switch (arribute.RepositoryItemType) {
          case RepositoryItemType.TextBox:
            b.AddAttribute(seq++, "IsPassword", arribute.TextBoxAttribute.IsPassword);
            break;
          case RepositoryItemType.ImageComboBox:
            b.AddAttribute(seq++, "DisplayFieldName", arribute.ComboBoxAttribute.DisplayFieldName);
            b.AddAttribute(seq++, "ValueFieldName", arribute.ComboBoxAttribute.ValueFieldName);
            b.AddAttribute(seq++, "ImageFieldName", arribute.ComboBoxAttribute.ImageFieldName);
            b.AddAttribute(seq++, "IsShowEmptyRow", arribute.ComboBoxAttribute.IsShowEmptyRow);
            b.AddAttribute(seq++, "EmptyRowName", arribute.ComboBoxAttribute.EmptyRowName);
            b.AddAttribute(seq++, "ValueChanged", arribute.ComboBoxAttribute.ValueChanged);
            b.AddAttribute(seq++, "DataSource", arribute.ComboBoxAttribute.DataSource);
            break;
          case RepositoryItemType.CheckBox:
            b.AddAttribute(seq++, "ValueChecked", arribute.CheckBoxAttribute.ValueChecked);
            b.AddAttribute(seq++, "ValueUnchecked", arribute.CheckBoxAttribute.ValueUnchecked);
            b.AddAttribute(seq++, "CheckType", arribute.CheckBoxAttribute.CheckType);
            break;
          case RepositoryItemType.DateEdit:
            b.AddAttribute(seq++, "DisplayFormat", arribute.DateEditAttribute.DisplayFormat);
            break;
        }

        foreach (var parameter in arribute.Parameters) {
          b.AddAttribute(seq++, parameter.Key, parameter.Value);
        }

        b.CloseComponent();
      };
      return renderPage;
    }

    /// <summary>
    /// 그리드 데이터 초기 컬럼 셋팅
    /// </summary>
    /// <param name="dt"></param>
    protected void InitGridColumnsSetting(DataTable? dt) {
      if (dt == null)
        return;

      if (!dt.Columns.Contains("GUID"))
        dt.Columns.Add("GUID", typeof(string));
      if (!dt.Columns.Contains("SAVE_YN"))
        dt.Columns.Add("SAVE_YN", typeof(string));
      if (!dt.Columns.Contains(CheckedFieldName))
        dt.Columns.Add(CheckedFieldName, typeof(string));

      foreach (DataRow row in dt.Rows) {
        row["GUID"] = Guid.NewGuid().ToString();
        row["SAVE_YN"] = "Y";
        row[CheckedFieldName] = "N";
      }
    }

    /// <summary>
    /// 그리드 필수 컬럼 셋팅
    /// </summary>
    /// <param name="needColumns"></param>
    public void SetNeedColumns(string[] needColumns) {
      NeedColumns.Clear();
      NeedColumns.AddRange(needColumns);
    }

    /// <summary>
    /// 그리드에서 체크된 로우 데이터
    /// </summary>
    /// <returns></returns>
    public DataRow[]? GetCheckedRows() {
      return DataSource?.GetCheckedRows();
    }

    /// <summary>
    /// 그리드에서 체크된 로우가 있는 체크
    /// </summary>
    /// <returns></returns>
    public bool IsCheckedRows() {
      return DataSource == null ? false : DataSource.IsCheckedRows();
    }

    /// <summary>
    /// 그리드에 데이터가 있는지 여부
    /// </summary>
    /// <returns></returns>
    public bool IsRows() {
      return DataSource == null ? false : DataSource.Rows.Count > 0;
    }

    /// <summary>
    /// 체크된 로우중에 필수 컬럼 값이 입력되었는지 체크
    /// </summary>
    /// <returns></returns>
    public bool IsCheckedNeedColumns() {
      var checkedRows = GetCheckedRows();

      if (checkedRows == null || checkedRows.Length == 0)
        return false;

      bool isNeedChecked = true;

      foreach (DataRow row in checkedRows) {
        row.ClearErrors();

        foreach (string col in NeedColumns) {
          if (row.Table.Columns.IndexOf(col) < 0)
            continue;

          if (string.IsNullOrEmpty(row[col].ToStringTrim())) {
            row.SetColumnError(col, "필수 입력 값입니다.");
            isNeedChecked = false;
          }
        }
      }

      return isNeedChecked;
    }

    /// <summary>
    /// 새로운 로우 복제
    /// </summary>
    /// <returns></returns>
    public DataRow? NewRowClone() {
      if (DataSource == null)
        return null;

      DataRow row = DataSource.NewRow();
      row[CheckedFieldName] = "Y";
      row["SAVE_YN"] = "N";
      row["GUID"] = Guid.NewGuid().ToString();

      return row;
    }

    /// <summary>
    /// 새로운 로우 추가
    /// </summary>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public DataRow? AddNewRow() {
      if (Grid == null)
        throw new Exception("Grid is Null");

      if (_datatable == null)
        throw new Exception("DataSource is Null");

      var row = _datatable.NewRow();
      row["GUID"] = Guid.NewGuid().ToString();
      row["SAVE_YN"] = "N";
      row[CheckedFieldName] = "Y";

      CommonGridInitNewRowEventArgs e = new CommonGridInitNewRowEventArgs();
      e.Row = row;

      InitNewRowChanging.InvokeAsync(e);

      if (e.Cancel == true)
        return null;

      _datatable.Rows.Add(row);

      Grid.SelectDataItem(row);

      return row;
    }

    /// <summary>
    /// 로우 및 필드값에 해당되는 데이터 셋팅
    /// </summary>
    /// <param name="rowIndex"></param>
    /// <param name="fieldName"></param>
    /// <param name="value"></param>
    public void SetRowCellValue(int rowIndex, string fieldName, object value) {
      var item = Grid?.GetDataItem(rowIndex) as DataRowView;

      if (item == null)
        return;

      item[fieldName] = value;
    }

    /// <summary>
    /// 로우 및 필드값에 해당되는 데이터 값 리턴
    /// </summary>
    /// <param name="rowIndex"></param>
    /// <param name="fieldName"></param>
    /// <returns></returns>
    public object? GetRowCellValue(int rowIndex, string fieldName) {
      var item = Grid?.GetDataItem(rowIndex) as DataRowView;

      if (item == null)
        return null;

      return item[fieldName];
    }

    /// <summary>
    /// 로우에 해당되는 데이터 로우 리턴
    /// </summary>
    /// <param name="rowIndex"></param>
    /// <returns></returns>
    public DataRow? GetRowValue(int rowIndex) {
      return (Grid?.GetDataItem(rowIndex) as DataRowView)?.Row;
    }

    /// <summary>
    /// 포커스된 데이터 로우 리턴
    /// </summary>
    /// <returns></returns>
    public DataRow? GetFocusedRowValue() {
      //return Grid?.GetFocusedDataItem() as DataRowView;\
      if (FocusedRowIndex < 0)
        return null;

      return _datatable?.Rows[FocusedRowIndex];
    }

    /// <summary>
    /// 포커스된 해당 필드 데이터 값 리턴
    /// </summary>
    /// <param name="fieldName"></param>
    /// <returns></returns>
    public object? GetFocusedRowCellValue(string fieldName) {
      //var item = Grid?.GetFocusedDataItem() as DataRowView;

      //if (item == null)
      //return null;

      //return item[fieldName];
      if (FocusedRowIndex < 0)
        return null;

      return _datatable?.Rows[FocusedRowIndex][fieldName];
    }

    /// <summary>
    /// 포커스된 로우 및 필드에 데이터 값 셋팅
    /// </summary>
    /// <param name="fieldName"></param>
    /// <param name="value"></param>
    public void SetFocusedRowCellValue(string fieldName, object value) {
      // var item = Grid?.GetFocusedDataItem() as DataRowView;

      // if (item == null)
      //     return;

      // item[fieldName] = value;
      if (FocusedRowIndex < 0 || _datatable == null)
        return;

      _datatable.Rows[FocusedRowIndex][fieldName] = value;
    }

    /// <summary>
    /// 엑셀 Export 
    /// </summary>
    /// <param name="ExportName"></param>
    /// <param name="option"></param>
    /// <returns></returns>
    public async Task ExportXlsx(string ExportName, GridXlExportOptions? option = null) {
      if (Grid != null) {
        await Grid.ExportToXlsxAsync(ExportName, option);
      }
    }

    /// <summary>
    /// 그리드 데이터 저장
    /// </summary>
    public void PostEditor() {
      if (Grid == null)
        return;

      Grid.SaveChangesAsync();
      StateHasChanged();
    }

    /// <summary>
    /// 그리드 데이터 저장
    /// </summary>
    /// <returns></returns>
    public async Task PostEditorAsync() {
      if (Grid == null)
        return;

      await Grid.SaveChangesAsync();
      StateHasChanged();
    }

    /// <summary>
    /// 로우 더블 클릭 이벤트
    /// </summary>
    /// <param name="e"></param>
    protected void OnRowDoubleClick(GridRowClickEventArgs e) {
      RowDoubleClick.InvokeAsync(e);
    }

    /// <summary>
    /// 그리드 스타일 적용 이벤트
    /// </summary>
    /// <param name="e"></param>
    public void OnCustomizeElement(GridCustomizeElementEventArgs e) {
      if (e.ElementType == GridElementType.HeaderCell) {
        if (NeedColumns.Any(w => w.Equals(((IGridDataColumn)e.Column).FieldName))) {
          e.CssClass = "common-grid-need-col-color";
        }
      }
      else if (e.ElementType == GridElementType.DataCell) {

        foreach (var dic in DataCellCss) {

          //e.Column.Name

          if (dic.Key == ((IGridDataColumn)e.Column).FieldName) {
            e.CssClass = dic.Value; //"grid-col-left-line";
            //break;
          }

        }
      }

      else if (e.ElementType == GridElementType.DataRow) {

        foreach (var dic in DataRowCss) {
          if (dic.Key == ((IGridDataColumn)e.Column).FieldName) {
            e.CssClass = dic.Value; //"grid-col-left-line";
            //break;
          }

        }

        if (_cellStyleList.Count > 0) {

          foreach( var _s in _cellStyleList) {


            var chk_str = (e.Grid.GetRowValue(e.VisibleIndex, _s.Key) + "");
            var _strs = _s.Value.Split(',');
            foreach (var str in _strs) {
              if (chk_str == str) {
                //e.Style = "background-color: var(--dxbl-client-component-palette-danger);font-weight:bold;font-size: 1.2rem;";
                e.CssClass += " mit-sum-row "+_s.Design;
                
                break;
              }
            }


          }

        }



      }


      //border-left: solid red !important;

      //Debug.WriteLine("FiledCssClass.Count : " + FiledCssClass.Count);

      //if (FiledCssClass.Count > 0) {

      //foreach ( var dic in FiledCssClass) {

      //Debug.WriteLine("FiledCssClass.dic.Key : " + dic.Key);
      //Debug.WriteLine("FiledCssClass.dic.Value : " + dic.Value);

      //if (dic.Key.Any(w => w.Equals(((IGridDataColumn)e.Column).FieldName))) {
      //e.CssClass = " "+dic.Value+" ";
      //}

      // }

      //}


      CustomizeElement.InvokeAsync(e);
    }

    //private bool isSetCellStyle = false;

    private List<DesignCellInfo> _cellStyleList = new List<DesignCellInfo>();

    //private string _cellStylekey { get; set; } 
    //private string[] _cellStyleValues { get; set; }

    public async Task SetCellStyles(params string[] fnameTarvals) {
      foreach (var ftv in fnameTarvals) {

        var fnms = ftv.Split('^');
        foreach (var fnm in fnms) {
          var fn = fnm.Split("|");
          SetCellStyle(fn[0], fn[1]);
        }
      }
      //await SetCellStyle(string fnameTarvals);
    }

    public async Task SetCellStyle(string fname, string tarVal, string design = "") {
      DesignCellInfo di = new DesignCellInfo() {
      Key = fname,Value = tarVal,Design = design
      };
      _cellStyleList.Add(di);

      //if (Grid == null) return "grid is null";



      //if (isSetCellStyle == true) return "isSetCellStyle is true";



      //_cellStylekey = fname;
      //_cellStyleValues = tarVal.Split(",");

      //string chk = "fname:" + fname + ", tarVal:" + tarVal;
      /*
      Grid.CustomizeElement += new Action<GridCustomizeElementEventArgs>(e => {

        if (e.ElementType == GridElementType.DataRow) {

          var chk_str = (e.Grid.GetRowValue(e.VisibleIndex, _cellStylekey) + "");
          //string[] strs = _cellStyleValues.Split(",");

          //chk += "chk_str:"+ chk_str;

          foreach (var str in _cellStyleValues) {

            //  chk += "..str:" + str;

            if (chk_str == str) {
              //    chk += "..zz_str:" + str;
              //e.CssClass += " mit-grid-highright";
              e.Style = "background-color: var(--dxbl-client-component-palette-danger);";
              //e.CssClass += " " + e.ElementType;
              break;
            }
          }
        }
        CustomizeElement.InvokeAsync(e);
      });

      */
      //isSetCellStyle = true;
      //StateHasChanged();
      //return "isSetCellStyle is setting" + chk;

    }

    /// <summary>
    /// 그리드 포커스 로우 체인지 이벤트
    /// </summary>
    /// <param name="e"></param>
    protected void OnFocusedRowChanged(GridFocusedRowChangedEventArgs e) {
      FocusedRowIndex = e.VisibleIndex;
      FocusedRow = (e.DataItem as DataRowView)?.Row;

      FocusedRowChanged.InvokeAsync(e);
    }

    #region 그리드 자체 체크 모드

    protected void OnAllCheckedChanged(bool value) {
      IsAllChecked = value;

      if (_datatable != null) {
        foreach (DataRow row in _datatable.Rows) {
          row["CHK"] = IsAllChecked ? "Y" : "N";
        }
      }
    }

    protected void OnCheckedChanged(GridDataColumnCellDisplayTemplateContext? context, string value) {
      if (context == null || context.DataItem == null || _datatable == null)
        return;
      var grd = context.Grid as DxGrid;
      var dataItem = context.DataItem as DataRowView;

      if (dataItem == null || grd == null)
        return;

      dataItem[context.DataColumn.FieldName] = value;

      IsAllChecked = !_datatable.AsEnumerable().Any(w => w["CHK"].ToStringTrim().Equals("N"));
    }

    #endregion 그리드 자체 체크 모드

    /// <summary>
    /// Validation 체크 없는 조회 버튼 함수
    /// </summary>
    /// <returns></returns>
    public async Task PerformSearchButtonClick() {
      await Search();
    }

    /// <summary>
    /// Validation 체크 없는 저장 버튼 함수
    /// </summary>
    /// <returns></returns>
    public async Task<bool> PerformSaveButtonClick() {
      return await Save();
    }

    /// <summary>
    /// Validation 체크 없는 삭제 버튼 함수
    /// </summary>
    /// <returns></returns>
    public async Task<bool> PerformDeleteButtonClick() {
      return await Delete();
    }


    /// <summary>
    /// 툴바 추가 버튼 이벤트
    /// </summary>
    /// <param name="e"></param>
    protected void OnAddButtonClick(ToolbarItemClickEventArgs e) {
      if (DataSource == null)
        return;

      AddNewRow();
    }

    /// <summary>
    /// 툴바 조회 버튼 이벤트
    /// </summary>
    /// <param name="e"></param>
    /// <returns></returns>
    protected async Task OnSearchButtonClick(ToolbarItemClickEventArgs e) {
      await OnSearch();
    }

    /// <summary>
    /// 툴바 저장 버튼 이벤트
    /// </summary>
    /// <param name="e"></param>
    /// <returns></returns>
    protected async Task OnSaveButtonClick(ToolbarItemClickEventArgs e) {
      await OnSave();
    }

    /// <summary>
    /// 툴바 삭제 버튼 이벤트
    /// </summary>
    /// <param name="e"></param>
    /// <returns></returns>
    protected async Task OnDeleteButtonClick(ToolbarItemClickEventArgs e) {
      await OnDelete();
    }





    public Stream GetFileStream(string format, ref string kkk) {

      MemoryStream fs = null;
      string a = "0";
      try {

        RptInfo ri = new RptInfo() { Title = "aaaa", Data = _datatable };
        RptInfo[] rpt = new RptInfo[] { ri };

        fs = new MemoryStream();


        var serialize = JsonConvert.SerializeObject(_datatable);

        //var bf = new BinaryFormatter();
        //IFormatter formatter = new BinaryFormatter();
        //bf.Serialize(fs, rpt);

        //var stream = new MemoryStream();
        var writer = new StreamWriter(fs);
        writer.Write(serialize);
        writer.Flush();
        fs.Position = 0;
        //return stream;




      }
      catch (Exception eeeee) {

        a = "5";
        a += eeeee.Message;
      }

      kkk = a;

      //var randomBinaryData = new byte[50 * 1024];
      //var fileStream = new MemoryStream(randomBinaryData);

      return fs;
    }




    [Parameter] public bool Rpt_Landscape { get; set; }
    [Parameter] public string Rpt_Title { get; set; }
    [Parameter] public string Rpt_SubTitle { get; set; }
    [Parameter] public string Rpt_UName { get; set; }


    [Parameter] public bool IsExportAllColumn { get; set; } = false;
    [Parameter] public string SwapColumns { get; set; } = ""; // 그룹화 순서를 위해 앞에 숫자 메긴 컬럼들을 원본 컬럼으로 대체하기위함

    public string GetGrdData(string format, ref string kkk) {


      //JSRuntime.InvokeVoidAsync("alert", "Columns : " + Columns);
      //JSRuntime.InvokeVoidAsync("alert", "Columns Count: " + Columns.Count);
      //var colds = Grid.GetDataColumns();
      //var cols = Grid
      //var colcaps = Grid.GetVisibleColumns();
      Dictionary<string, string> grdCol = new Dictionary<string, string>();
      Dictionary<string, RptCol> grdCol2 = new Dictionary<string,RptCol>();
      
      //JSRuntime.InvokeVoidAsync("alert", "Grid : " + Grid);
      IEnumerable<IGridColumn> columns = null;
      if (IsExportAllColumn) {
        columns = Grid.GetColumns();
      } else {
        columns = Grid.GetVisibleColumns();
      }

      //var columnInfo = columns.Select((column, index) => (1 + index) + " - " + column switch {
      //  IGridDataColumn dataColumn => dataColumn.FieldName,
      //  IGridSelectionColumn => "Selection Column",
      //  IGridCommandColumn => "Command Column",
      //  _ => null
      //});


      //JSRuntime.InvokeVoidAsync("alert", "visible columns : " + columns);
      //JSRuntime.InvokeVoidAsync("alert", "visible columns Count: " + columns.ToList().Count);

      foreach (var c in columns) {

        //JSRuntime.InvokeVoidAsync("alert", "colinfo caption: " + c.Caption);
        //JSRuntime.InvokeVoidAsync("alert", "colinfo IGridDataColumn: " + (c as IGridDataColumn));
        
        //
        var ic = (c as IGridDataColumn);
        if( ic == null) { continue; }


        foreach(var ex in ExcludeColumnsOnExport) {
          if (ex.Contains(c.Name)) { continue; }
        }
        
        var fieldName = ic.FieldName;
        //JSRuntime.InvokeVoidAsync("alert", "colinfo : "+ fieldName + "__"+c.Caption);
        if (!string.IsNullOrEmpty(fieldName) && !string.IsNullOrEmpty(c.Caption)) {
          grdCol.Add(fieldName, c.Caption);
        }
        else if (!string.IsNullOrEmpty(fieldName)) {
          grdCol.Add(fieldName, fieldName);
        }
        // JSRuntime.InvokeVoidAsync("alert", "colinfo caption: " + ic.Caption + " GroupIndex: "+ic.GroupIndex+" SortIndex: "+ic.SortIndex);
      }

      //JSRuntime.InvokeVoidAsync("alert", "visible grdCol : " + grdCol);

      //JSRuntime.InvokeVoidAsync("alert", "visible _datatable : " + _datatable);  

      // Columns2 는 Band 컬럼을 포함한 컬럼 정보
      string[] colSplits = SwapColumns.Split(',', StringSplitOptions.RemoveEmptyEntries);
      Dictionary<string, string> colSwapMap = new Dictionary<string, string>();
      foreach (var colSplit in colSplits)
      {
          var split = colSplit.Split('|');
          if (split.Length == 2)
          {
              colSwapMap.Add(split[0], split[1]);
          }
      }
      grdCol2 = BuildRptColMap(columns, colSwapMap);

      RptInfo ri = new RptInfo() { Columns = grdCol, Columns2 = grdCol2, Data = _datatable };

      ri.Title = Rpt_Title;
      ri.SubTitle = Rpt_SubTitle;
      ri.UName = Rpt_UName;
      ri.FileName = ExportFileName + "_" + DateTime.Now.Ticks.ToString();
      ri.Landscape = Rpt_Landscape ? "Y" : "N";
      ri.Date1 = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
      ri.ExportType = format;

      var serialize = JsonConvert.SerializeObject(ri);
      return serialize;

    }


    /// <summary>
    /// 특정컬럼 제외 데이터 게더링
    /// </summary>
    /// <param name="cols"></param>
    /// <param name="colSwapMap"></param>
    /// <returns></returns>
    /// <exception cref="NotSupportedException"></exception>
    public string GetGrdData(string format, ref string kkk, string[] hidColumns) {


    //JSRuntime.InvokeVoidAsync("alert", "Columns : " + Columns);
    //JSRuntime.InvokeVoidAsync("alert", "Columns Count: " + Columns.Count);
    //var colds = Grid.GetDataColumns();
    //var cols = Grid
    //var colcaps = Grid.GetVisibleColumns();
    Dictionary<string, string> grdCol = new Dictionary<string, string>();
    Dictionary<string, RptCol> grdCol2 = new Dictionary<string, RptCol>();

    //JSRuntime.InvokeVoidAsync("alert", "Grid : " + Grid);
    IEnumerable<IGridColumn> columns = null;
      if (IsExportAllColumn) {
        columns = Grid.GetColumns();
      } else {
        columns = Grid.GetVisibleColumns();
      }

//var columnInfo = columns.Select((column, index) => (1 + index) + " - " + column switch {
//  IGridDataColumn dataColumn => dataColumn.FieldName,
//  IGridSelectionColumn => "Selection Column",
//  IGridCommandColumn => "Command Column",
//  _ => null
//});


//JSRuntime.InvokeVoidAsync("alert", "visible columns : " + columns);
//JSRuntime.InvokeVoidAsync("alert", "visible columns Count: " + columns.ToList().Count);

foreach (var c in columns) {
  //특정필드 비교 제외
  if(hidColumns.Contains(c.Name)) {
          continue;
        }

  //JSRuntime.InvokeVoidAsync("alert", "colinfo caption: " + c.Caption);
  //JSRuntime.InvokeVoidAsync("alert", "colinfo IGridDataColumn: " + (c as IGridDataColumn));

  //
  var ic = (c as IGridDataColumn);
  if (ic == null) { continue; }

  var fieldName = ic.FieldName;
  //JSRuntime.InvokeVoidAsync("alert", "colinfo : "+ fieldName + "__"+c.Caption);
  if (!string.IsNullOrEmpty(fieldName) && !string.IsNullOrEmpty(c.Caption)) {
    grdCol.Add(fieldName, c.Caption);
  }
  else if (!string.IsNullOrEmpty(fieldName)) {
    grdCol.Add(fieldName, fieldName);
  }
  // JSRuntime.InvokeVoidAsync("alert", "colinfo caption: " + ic.Caption + " GroupIndex: "+ic.GroupIndex+" SortIndex: "+ic.SortIndex);
}

//JSRuntime.InvokeVoidAsync("alert", "visible grdCol : " + grdCol);

//JSRuntime.InvokeVoidAsync("alert", "visible _datatable : " + _datatable);  

// Columns2 는 Band 컬럼을 포함한 컬럼 정보
string[] colSplits = SwapColumns.Split(',', StringSplitOptions.RemoveEmptyEntries);
Dictionary<string, string> colSwapMap = new Dictionary<string, string>();
foreach (var colSplit in colSplits) {
  var split = colSplit.Split('|');
  if (split.Length == 2) {
    colSwapMap.Add(split[0], split[1]);
  }
}
grdCol2 = BuildRptColMap(columns, colSwapMap);

RptInfo ri = new RptInfo() { Columns = grdCol, Columns2 = grdCol2, Data = _datatable };

ri.Title = Rpt_Title;
ri.SubTitle = Rpt_SubTitle;
ri.UName = Rpt_UName;
ri.FileName = ExportFileName + "_" + DateTime.Now.Ticks.ToString();
ri.Landscape = Rpt_Landscape ? "Y" : "N";
ri.Date1 = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
ri.ExportType = format;

var serialize = JsonConvert.SerializeObject(ri);
return serialize;

    }



     /* =========================================================
        1. DevExpress 컬럼 배열 → RptCol 딕셔너리
        =========================================================*/
     public static Dictionary<string, RptCol> BuildRptColMap(IEnumerable<IGridColumn> cols, Dictionary<string, string> colSwapMap)
     {
         var obj2Id = new Dictionary<IGridColumn, string>(); // 부모 컬럼를 찾기위한 Id, 객체 키페어
         var map = new Dictionary<string, RptCol>();
         int order = 0;
         int bandIdx = 0;

         
         // ── 1-A. 1차 스캔: Depth 와 Order 까지 채움 ──────
         foreach (var col in cols)
         {
             (string id, string caption, string kind) = col switch
             {
                 DxGridDataColumn dc => (dc.FieldName,
                                         string.IsNullOrEmpty(dc.Caption) ? dc.FieldName : dc.Caption,
                                         "Data"),
                 DxGridBandColumn bc => ($"B_{bandIdx++}",
                                         bc.Caption,
                                         "Band"),
                 _ => throw new NotSupportedException($"Unknown column type: {col.GetType()}")
             };
             // SwapColumns 에서 지정된 컬럼명으로 변경
             if (colSwapMap.ContainsKey(id))
             {
                 id = colSwapMap[id];
                 Console.WriteLine("SwapColumns: " + id);
             }
             // 객체 와 id 매칭
             obj2Id[col] = id;
             //
             map[id] = new RptCol(id, caption, kind, null, order++, 0,
                                    ColSpan: 0, RowSpan: 0); // Col/RowSpan은 다음 단계에서 계산
            }

            // ── parentId, depth 입력 ─────────────────────────────
            foreach (var col in cols)
            {
                string id = obj2Id[col];
                //parentId 설정
                string? parentId = GetParentId(col, obj2Id);

                //depth 설정
                int depth = parentId == null ? 0 : map[parentId].Depth + 1;
                map[id] = map[id] with { ParentId = parentId, Depth = depth };
            }


            // ── span값 입력 ─────────────────────────────
            int maxDepth = map.Values.Max(m => m.Depth);

            int LeafCount(string id) =>
                map[id].Kind == "Data"
                    ? 1
                    : map.Values.Where(m => m.ParentId == id).Sum(m => LeafCount(m.FieldName));
            foreach (var kvp in map.ToList())
            {
                var m = kvp.Value;

                // span 설정
                int colSpan = m.Kind == "Data" ? 1 : LeafCount(m.FieldName);
                int rowSpan = m.Kind == "Data" ? (maxDepth - m.Depth + 1) : 1;

                map[kvp.Key] = m with { ColSpan = colSpan, RowSpan = rowSpan };
            }
            return map;
     }

        /// <summary>
        /// Parent 컬럼의 ID를 구한다.
        /// · Parent 가 DxGrid  → null (루트)
        /// · 그 밖의 IGridColumn → obj2Id 사전에서 ID 찾아 반환
        /// </summary>
        private static string? GetParentId(
            IGridColumn col,
            Dictionary<IGridColumn, string> obj2Id)   // ★ 객체 → ID 자동 매핑
        {
            // DevExpress 내부 비공개 "Parent" 프로퍼티 접근
            var parentProp = col.GetType().GetProperty(
                                 "Parent",
                                 BindingFlags.Instance | BindingFlags.NonPublic);

            if (parentProp?.GetValue(col) is not IGridColumn parent)
                return null;               // Parent 자체가 없음

            if (parent is DxGrid)          // 그리드가 최상위
                return null;

            // 객체 레퍼런스로 바로 ID 조회 (Caption 중복 걱정 X)
            return obj2Id.TryGetValue(parent, out var id) ? id : null;
        }



/*
        public static void CreateReport(XtraReport report, string[] fields) {
      PageHeaderBand pageHeader = new PageHeaderBand() { HeightF = 23, Name = "pageHeaderBand" };
      int tableWidth = (int)(report.PageWidth - report.Margins.Left - report.Margins.Right);
      XRTable headerTable = XRTable.CreateTable(
                            new Rectangle(0,    // rect X
                                          0,          // rect Y
                                          tableWidth, // width
                                          40),        // height
                                          1,          // table row count
                                          0);         // table column count
      headerTable.Borders = DevExpress.XtraPrinting.BorderSide.All;
      headerTable.BackColor = Color.Gainsboro;
      headerTable.Font = new Font("Verdana", 10, FontStyle.Bold);
      headerTable.Rows.FirstRow.Width = tableWidth;
      headerTable.BeginInit();
      foreach (string field in fields) {
        XRTableCell cell = new XRTableCell();
        cell.Width = 100;
        cell.Text = field;
        cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
        headerTable.Rows.FirstRow.Cells.Add(cell);
      }
      headerTable.EndInit();
      headerTable.AdjustSize();
      pageHeader.Controls.Add(headerTable);

      DetailBand detail = new DetailBand() { HeightF = 23, Name = "detailBand" };
      XRTable detailTable = XRTable.CreateTable(
                      new Rectangle(0,    // rect X
                                      0,          // rect Y
                                      tableWidth, // width
                                      40),        // height
                                      1,          // table row count
                                      0);         // table column count

      detailTable.Width = tableWidth;
      detailTable.Rows.FirstRow.Width = tableWidth;
      detailTable.Borders = DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right | DevExpress.XtraPrinting.BorderSide.Bottom;
      detailTable.BeginInit();
      foreach (string field in fields) {
        XRTableCell cell = new XRTableCell();
        ExpressionBinding binding = new ExpressionBinding("BeforePrint", "Text", String.Format("[{0}]", field));
        cell.ExpressionBindings.Add(binding);
        cell.Width = 100;
        cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
        if (field.Contains("Date"))
          cell.TextFormatString = "{0:MM/dd/yyyy}";
        detailTable.Rows.FirstRow.Cells.Add(cell);
      }
      detailTable.Font = new Font("Verdana", 8F);
      detailTable.EndInit();
      detailTable.AdjustSize();
      detail.Controls.Add(detailTable);
      report.Bands.AddRange(new Band[] { detail, pageHeader });
    }

*/


    #region [ 사용자 버튼 정의 ]

    protected async Task OnSearch() {
      if (SearchButtonClick.HasDelegate) {
        await SearchButtonClick.InvokeAsync();
        return;
      }

      if (string.IsNullOrEmpty(SearchQueryName))
        return;

      await Search();
    }

    protected async Task OnSave() {
      if (SaveButtonClick.HasDelegate) {
        await SaveButtonClick.InvokeAsync();
        return;
      }

      if (string.IsNullOrEmpty(SaveQueryName))
        return;

      await PostEditorAsync();

      if (!IsCheckedRows()) {
        MessageBoxService?.Show("선택된 데이터가 없습니다.");
        return;
      }

      if (!IsCheckedNeedColumns()) {
        MessageBoxService?.Show("필수 입력값이 누락되었습니다.");
        return;
      }

      MessageBoxService?.Show("저장하시겠습니까?", buttons: CommonMsgButtons.YesNo, CloseCallBack: SaveCallback);
    }

    protected async Task SaveCallback(CommonMsgResult result) {
      if (result != CommonMsgResult.Yes)
        return;

      if (await Save()) {
        MessageBoxService?.Show("저장하였습니다.");
        await Search();
      }
      else {
        MessageBoxService?.Show("저장에 실패하였습니다.");
      }
    }

    protected async Task OnDelete() {
      if (DeleteButtonClick.HasDelegate) {
        await DeleteButtonClick.InvokeAsync();
        return;
      }

      if (string.IsNullOrEmpty(DeleteQueryName))
        return;

      await PostEditorAsync();

      if (!IsCheckedRows()) {
        MessageBoxService?.Show("선택된 데이터가 없습니다.");
        return;
      }

      MessageBoxService?.Show("삭제하시겠습니까?", buttons: CommonMsgButtons.YesNo, CloseCallBack: DeleteCallback);
    }

    protected async Task DeleteCallback(CommonMsgResult result) {
      if (result != CommonMsgResult.Yes)
        return;

      if (await Delete()) {
        MessageBoxService?.Show("삭제하였습니다.");
        await Search();
      }
      else {
        MessageBoxService?.Show("삭제에 실패하였습니다.");
      }
    }

    #endregion [ 사용자 버튼 정의 ]

    #region [ 사용자 정의 메소드 ]

    private async Task Search() {
      if (string.IsNullOrEmpty(SearchQueryName))
        return;

      try {
        ShowLoadingPanel();

        Dictionary<string, object?> parameters = new Dictionary<string, object?>();

        if (InputSearchParameter.HasDelegate)
          await InputSearchParameter.InvokeAsync(parameters);

        var datasource = await DBSearch(parameters);

        DataSource = datasource;

        await PostEditorAsync();
      }
      catch (Exception ex) {
        CloseLoadingPanel();

        MessageBoxService?.Show(ex.Message, type: CommonMsgBoxIcon.Error);
      }
      finally {
        CloseLoadingPanel();
      }
    }

    private async Task<bool> Save() {
      if (string.IsNullOrEmpty(SaveQueryName))
        return false;

      try {
        ShowLoadingPanel();

        await PostEditorAsync();

        var checkedRows = GetCheckedRows();

        if (checkedRows == null || checkedRows.Length == 0)
          return true;

        await DBSave(checkedRows.CopyToDataTable());

        return true;
      }
      catch (Exception ex) {
        CloseLoadingPanel();
        MessageBoxService?.Show(ex.Message, type: CommonMsgBoxIcon.Error);
        return false;
      }
      finally {
        CloseLoadingPanel();
      }
    }

    private async Task<bool> Delete() {
      if (string.IsNullOrEmpty(DeleteQueryName))
        return false;

      try {
        ShowLoadingPanel();

        await PostEditorAsync();

        var checkedRows = GetCheckedRows();

        if (checkedRows == null || checkedRows.Length == 0)
          return true;

        await DBDelete(checkedRows.CopyToDataTable());

        return true;
      }
      catch (Exception ex) {
        CloseLoadingPanel();
        MessageBoxService?.Show(ex.Message, type: CommonMsgBoxIcon.Error);
        return false;
      }
      finally {
        CloseLoadingPanel();
      }
    }

    #endregion [ 사용자 정의 메소드 ]

    #region [ 데이터 정의 메소드 ]

    private async Task<DataTable?> DBSearch(Dictionary<string, object?> parameters) {
      if (QueryService == null)
        throw new Exception("QueryService가 null입니다.");

      if (string.IsNullOrEmpty(SearchQueryName))
        throw new Exception("SearchQueryName에 값이 없습니다.");

      var datatable = await QueryService.ExecuteDatatableAsync(SearchQueryName, parameters);

      return datatable;
    }

    private async Task DBSave(DataTable datatable) {
      if (QueryService == null)
        throw new Exception("QueryService가 null입니다.");

      if (string.IsNullOrEmpty(SaveQueryName))
        throw new Exception("SaveQueryName에 값이 없습니다.");

      await QueryService.ExecuteNonQuery(SaveQueryName, datatable, new Dictionary<string, object?>()
      {
                { "REG_ID", USER_ID },
            });
    }

    private async Task DBDelete(DataTable datatable) {
      if (QueryService == null)
        throw new Exception("QueryService가 null입니다.");

      if (string.IsNullOrEmpty(DeleteQueryName))
        throw new Exception("DeleteQueryName에 값이 없습니다.");

      await QueryService.ExecuteNonQuery(DeleteQueryName, datatable, new Dictionary<string, object?>()
      {
                { "REG_ID", USER_ID },
            });
    }

    #endregion [ 데이터 정의 메소드 ]
  }


  public class DesignCellInfo() {
    public string Key { get; set; }
    public string Value { get; set; }
    public string Design { get; set; }
  }

}
