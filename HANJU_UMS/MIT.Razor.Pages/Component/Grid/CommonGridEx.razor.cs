using DevExpress.Blazor;
using Microsoft.AspNetCore.Components;
using MIT.DataUtil.Common;
using MIT.Razor.Pages.Component.Grid.Data;
using MIT.Razor.Pages.Component.MessageBox;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIT.Razor.Pages.Component.Grid
{
    public class CommonGridExBase : CommonUIComponentBase
    {
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
        public ObservableCollection<CommonGridExDataColumnAttribute> Columns { get; set; } = new ObservableCollection<CommonGridExDataColumnAttribute>();

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
        /// 툴바 조회 버튼 보이기
        /// </summary>
        [Parameter]
        public bool IsSearchButtonEnabled { get; set; } = false;
        /// <summary>
        /// 툴바 신규 버튼 보이기
        /// </summary>
        [Parameter]
        public bool IsCreateButtonEnabled { get; set; } = false;
        /// <summary>
        /// 툴바 등록 버튼 보이기
        /// </summary>
        [Parameter]
        public bool IsUpdateButtonEnabled { get; set; } = false;
        /// <summary>
        /// 툴바 삭제 버튼 보이기
        /// </summary>
        [Parameter]
        public bool IsDeleteButtonEnabled { get; set; } = false;
        /// <summary>
        /// 툴바 Excel Export 버튼 보이기
        /// </summary>
        [Parameter]
        public bool IsExportButtonEnabled { get; set; } = false;

        /// <summary>
        /// 그리드 체크시 멀티 싱글 선택
        /// </summary>
        [Parameter]
        public GridSelectionMode SelectionMode { get; set; } = GridSelectionMode.Multiple;

        /// <summary>
        /// 그리드 포커스 체인지 이벤트
        /// </summary>
        [Parameter]
        public EventCallback<GridFocusedRowChangedEventArgs> FocusedRowChanged { get; set; } //
        /// <summary>
        /// 그리드 로우 클릭 이벤트
        /// </summary>
        [Parameter]
        public EventCallback<GridRowClickEventArgs> RowClick { get; set; } //
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
        /// 툴바 신규 버튼 클릭 이벤트
        /// </summary>
        [Parameter]
        public EventCallback CreateButtonClick { get; set; } //
        /// <summary>
        /// 툴바 등록 버튼 클릭 이벤트
        /// </summary>
        [Parameter]
        public EventCallback UpdateButtonClick { get; set; } //
        /// <summary>
        /// 툴바 삭제 버튼 클릭 이벤트
        /// </summary>
        [Parameter]
        public EventCallback DeleteButtonClick { get; set; } //


        /// <summary>
        /// 그리드 데이터 셋팅
        /// </summary>
        public DataTable? DataSource
        {
            get { return _datatable; }
            set
            {
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

        /// <summary>
        /// 그리드 생성시 초기화 함수
        /// </summary>
        protected override void OnInitialized()
        {
            FocusedRowIndex = -1;
            FocusedRow = null;
        }

        /// <summary>
        /// 그리드 데이터 초기 컬럼 셋팅
        /// </summary>
        /// <param name="dt"></param>
        protected void InitGridColumnsSetting(DataTable? dt)
        {
            if (dt == null)
                return;

            if (!dt.Columns.Contains("GUID"))
                dt.Columns.Add("GUID", typeof(string));
            if (!dt.Columns.Contains("SAVE_YN"))
                dt.Columns.Add("SAVE_YN", typeof(string));
            if (!dt.Columns.Contains(CheckedFieldName))
                dt.Columns.Add(CheckedFieldName, typeof(string));

            foreach (DataRow row in dt.Rows)
            {
                row["GUID"] = Guid.NewGuid().ToString();
                row["SAVE_YN"] = "Y";
                row[CheckedFieldName] = "N";
            }
        }

        /// <summary>
        /// 그리드 필수 컬럼 셋팅
        /// </summary>
        /// <param name="needColumns"></param>
        public void SetNeedColumns(string[] needColumns)
        {
            NeedColumns.Clear();
            NeedColumns.AddRange(needColumns);
        }

        /// <summary>
        /// 그리드에서 체크된 로우 데이터
        /// </summary>
        /// <returns></returns>
        public DataRow[]? GetCheckedRows()
        {
            return DataSource?.GetCheckedRows();
        }

        /// <summary>
        /// 그리드에서 체크된 로우가 있는 체크
        /// </summary>
        /// <returns></returns>
        public bool IsCheckedRows()
        {
            return DataSource == null ? false : DataSource.IsCheckedRows();
        }

        /// <summary>
        /// 체크된 로우중에 필수 컬럼 값이 입력되었는지 체크
        /// </summary>
        /// <returns></returns>
        public bool IsCheckedNeedColumns()
        {
            var checkedRows = GetCheckedRows();

            if (checkedRows == null || checkedRows.Length == 0)
                return false;

            bool isNeedChecked = true;

            foreach (DataRow row in checkedRows)
            {
                row.ClearErrors();

                foreach (string col in NeedColumns)
                {
                    if (row.Table.Columns.IndexOf(col) < 0)
                        continue;

                    if (string.IsNullOrEmpty(row[col].ToStringTrim()))
                    {
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
        public DataRow? NewRowClone()
        {
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
        public DataRow? AddNewRow()
        {
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

        public DataRow? AddNewRow(DataRow? newRow)
        {
            if (Grid == null)
                throw new Exception("Grid is Null");

            if (_datatable == null)
                throw new Exception("DataSource is Null");

            if (newRow == null)
                throw new Exception("row is Null");

            newRow["GUID"] = Guid.NewGuid().ToString();
            newRow["SAVE_YN"] = "N";
            newRow[CheckedFieldName] = "Y";

            CommonGridInitNewRowEventArgs e = new CommonGridInitNewRowEventArgs();
            e.Row = newRow;

            InitNewRowChanging.InvokeAsync(e);

            if (e.Cancel == true)
                return null;

            _datatable.Rows.Add(newRow);

            Grid.SelectDataItem(newRow);

            return newRow;
        }

        /// <summary>
        /// 로우 및 필드값에 해당되는 데이터 셋팅
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        public void SetRowCellValue(int rowIndex, string fieldName, object value)
        {
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
        public object? GetRowCellValue(int rowIndex, string fieldName)
        {
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
        public DataRow? GetRowValue(int rowIndex)
        {
            return (Grid?.GetDataItem(rowIndex) as DataRowView)?.Row;
        }

        /// <summary>
        /// 포커스된 데이터 로우 리턴
        /// </summary>
        /// <returns></returns>
        public DataRow? GetFocusedRowValue()
        {
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
        public object? GetFocusedRowCellValue(string fieldName)
        {
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
        public void SetFocusedRowCellValue(string fieldName, object value)
        {
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
        public async Task ExportXlsx(string ExportName, GridXlExportOptions? option = null)
        {
            if (Grid != null)
                await Grid.ExportToXlsxAsync(ExportName, option);
        }

        /// <summary>
        /// 그리드 데이터 저장
        /// </summary>
        public void PostEditor()
        {
            if (Grid == null)
                return;

            Grid.SaveChangesAsync();
            StateHasChanged();
        }

        /// <summary>
        /// 그리드 데이터 저장
        /// </summary>
        /// <returns></returns>
        public async Task PostEditorAsync()
        {
            if (Grid == null)
                return;

            await Grid.SaveChangesAsync();
            StateHasChanged();
        }

        /// <summary>
        /// 로우 더블 클릭 이벤트
        /// </summary>
        /// <param name="e"></param>
        protected void OnRowDoubleClick(GridRowClickEventArgs e)
        {
            RowDoubleClick.InvokeAsync(e);
        }

        /// <summary>
        /// 그리드 스타일 적용 이벤트
        /// </summary>
        /// <param name="e"></param>
        protected void OnCustomizeElement(GridCustomizeElementEventArgs e)
        {
            if (e.ElementType == GridElementType.HeaderCell)
            {
                if (NeedColumns.Any(w => w.Equals(((IGridDataColumn)e.Column).FieldName)))
                {
                    e.CssClass = "common-grid-need-col-color";
                }
            }

            CustomizeElement.InvokeAsync(e);
        }

        /// <summary>
        /// 그리드 포커스 로우 체인지 이벤트
        /// </summary>
        /// <param name="e"></param>
        protected void OnFocusedRowChanged(GridFocusedRowChangedEventArgs e)
        {
            FocusedRowIndex = e.VisibleIndex;
            FocusedRow = (e.DataItem as DataRowView)?.Row;

            FocusedRowChanged.InvokeAsync(e);
        }

        #region 그리드 자체 체크 모드

        protected void OnAllCheckedChanged(bool value)
        {
            IsAllChecked = value;

            if (_datatable != null)
            {
                foreach (DataRow row in _datatable.Rows)
                {
                    row["CHK"] = IsAllChecked ? "Y" : "N";
                }
            }
        }

        protected void OnCheckedChanged(GridDataColumnCellDisplayTemplateContext? context, string value)
        {
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

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="e"></param>
        //protected void OnInitButtonClick(ToolbarItemClickEventArgs e)
        //{
        //    DataSource = null;
        //}

        /// <summary>
        /// 툴바 조회 버튼 이벤트
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        protected async Task OnSearchButtonClick(ToolbarItemClickEventArgs e)
        {
            await SearchButtonClick.InvokeAsync();
        }

        /// <summary>
        /// 툴바 신규 버튼 이벤트
        /// </summary>
        /// <param name="e"></param>
        protected async Task OnCreateButtonClick(ToolbarItemClickEventArgs e)
        {
            await CreateButtonClick.InvokeAsync();
        }

        /// <summary>
        /// 툴바 등록 버튼 이벤트
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        protected async Task OnUpdateButtonClick(ToolbarItemClickEventArgs e)
        {
            await UpdateButtonClick.InvokeAsync();
        }

        /// <summary>
        /// 툴바 삭제 버튼 이벤트
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        protected async Task OnDeleteButtonClick(ToolbarItemClickEventArgs e)
        {
            await DeleteButtonClick.InvokeAsync();
        }

        /// <summary>
        /// 툴바 엑셀 버튼 이벤트
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        protected async Task OnExportButtonClick(ToolbarItemClickEventArgs e)
        {
            await ExportXlsx(ExportFileName);
        }


    }
}
