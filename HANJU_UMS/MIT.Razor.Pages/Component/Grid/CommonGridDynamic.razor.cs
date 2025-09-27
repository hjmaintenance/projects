using DevExpress.Blazor;
using Microsoft.AspNetCore.Components;
using MIT.DataUtil.Common;
using MIT.Razor.Pages.Component.Grid.Data;
using MIT.Razor.Pages.Component.MessageBox;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;

namespace MIT.Razor.Pages.Component.Grid
{
    public class CommonGridDynamicBase : CommonUIComponentBase
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

        /// <summary>
        /// 그리드 컬럼 셋팅 데이터
        /// </summary>
        [Parameter]
        public ObservableCollection<CommonGridDynamicDataColumnAttribute> Columns { get; set; } = new ObservableCollection<CommonGridDynamicDataColumnAttribute>();

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
        /// 그리드 컬럼 너비 타입
        /// </summary>
        [Parameter]
        public WidthType WidthType { get; set; } = WidthType.Rate;
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
        /// 툴바 조회 쿼리명
        /// </summary>
        [Parameter]
        public string SearchQueryName { get; set; } = string.Empty;
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
        /// 툴바 신규 버튼 보이기
        /// </summary>
        [Parameter]
        public bool IsCreateButtonEnabled { get; set; } = false;
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
        /// 조회 쿼리 전에 Parameter 셋팅 하는 이벤튼
        /// </summary>
        [Parameter]
        public EventCallback<Dictionary<string, object?>> InputSearchParameter { get; set; }

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
        /// 툴바 삭제 버튼 클릭 이벤트
        /// </summary>
        [Parameter]
        public EventCallback DeleteButtonClick { get; set; } //
        /// <summary>
        /// 그리드 id
        /// </summary>
        [Parameter]
        public string Id { get; set; } = "";

        /// <summary>
        /// 그리드 데이터 셋팅
        /// </summary>
        public DataTable? DataSource
        {
            get { return _datatable; }
            set
            {
                var totalStopwatch = Stopwatch.StartNew();
                InitGridColumnsSetting(value);
                totalStopwatch.Stop();

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

            Columns.Clear();

            foreach (DataColumn col in dt.Columns)
            {
                var column = new CommonGridDynamicDataColumnAttribute();
                column.FieldName = col.ColumnName;

                if (col.ColumnName.Contains("@@"))
                {
                    // @@로 시작하는 컬럼명 처리
                    column.FieldName = col.ColumnName.Replace("@@", "");
                    column.Caption = col.ColumnName.Replace("@@", "");
                    column.Visible = false;
                }
                else if (col.ColumnName == "GUID" || col.ColumnName == "SAVE_YN" || col.ColumnName == CheckedFieldName)
                {
                    // 기본적으로 추가한 컬럼명 처리
                    column.Visible = false;
                }
                else if (col.ColumnName == "RN" || col.ColumnName == "TC")
                {
                    // 페이징용으로 추가한 컬럼명 처리
                    column.Visible = false;
                }
                else if (int.TryParse(col.ColumnName.Substring(0, 3), out int firstThreeDigits))
                {
                    column.Caption = col.ColumnName.Substring(3);
                    column.Width = (firstThreeDigits % 100) * 10; // 첫 세 자리 숫자의 두 자리를 Width로 설정

                    switch (firstThreeDigits / 100)
                    {
                        case 1:
                            column.TextAlignment = GridTextAlignment.Left;
                            break;
                        case 2:
                            column.TextAlignment = GridTextAlignment.Center;
                            break;
                        case 3:
                            column.TextAlignment = GridTextAlignment.Right;
                            break;
                        default:
                            column.TextAlignment = GridTextAlignment.Auto;
                            break;
                    }
                }
                else
                {
                    // 그 외 일반적인 컬럼명 처리
                    column.Caption = col.Caption;
                }

                Columns.Add(column);
            }
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

        /// <summary>
        /// Validation 체크 없는 조회 버튼 함수
        /// </summary>
        /// <returns></returns>
        public async Task PerformSearchButtonClick()
        {
            await Search();
        }

        /// <summary>
        /// Validation 체크 없는 삭제 버튼 함수
        /// </summary>
        /// <returns></returns>
        public async Task<bool> PerformDeleteButtonClick()
        {
            return await Delete();
        }


        /// <summary>
        /// 툴바 조회 버튼 이벤트
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        protected async Task OnSearchButtonClick(ToolbarItemClickEventArgs e)
        {
            await OnSearch();
        }

        /// <summary>
        /// 툴바 삭제 버튼 이벤트
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        protected async Task OnDeleteButtonClick(ToolbarItemClickEventArgs e)
        {
            await OnDelete();
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
        /// 툴바 엑셀 버튼 이벤트
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        protected async Task OnExportButtonClick(ToolbarItemClickEventArgs e)
        {
            await ExportXlsx(ExportFileName);
        }

        #region [ 사용자 버튼 정의 ]

        protected async Task OnSearch()
        {
            if (SearchButtonClick.HasDelegate)
            {
                await SearchButtonClick.InvokeAsync();
                return;
            }

            if (string.IsNullOrEmpty(SearchQueryName))
                return;

            await Search();
        }

        protected async Task OnDelete()
        {
            if (DeleteButtonClick.HasDelegate)
            {
                await DeleteButtonClick.InvokeAsync();
                return;
            }

            if (string.IsNullOrEmpty(DeleteQueryName))
                return;

            if (!IsCheckedRows())
            {
                MessageBoxService?.Show(CommonMessage.DELETE_EMPTY_DATA);
                return;
            }

            MessageBoxService?.Show(CommonMessage.DELETE_CHECK, Header: CommonMessage.ALRIM,buttons: CommonMsgButtons.YesNo, CloseCallBack: DeleteCallback);
        }

        protected async Task DeleteCallback(CommonMsgResult result)
        {
            if (result != CommonMsgResult.Yes)
                return;

            if (await Delete())
            {
                await Search();
            }
            else
            {
                MessageBoxService?.Show(CommonMessage.DELETE_FAIL);
            }
        }

        #endregion [ 사용자 버튼 정의 ]

        #region [ 사용자 정의 메소드 ]



        private async Task Search()
        {
            if (string.IsNullOrEmpty(SearchQueryName))
                return;

            try
            {
                ShowLoadingPanel();

                Dictionary<string, object?> parameters = new Dictionary<string, object?>();

                if (InputSearchParameter.HasDelegate)
                    await InputSearchParameter.InvokeAsync(parameters);

                var datasource = await DBSearch(parameters);

                DataSource = datasource;
            }
            catch (Exception ex)
            {
                CloseLoadingPanel();

                MessageBoxService?.Show(ex.Message, type: CommonMsgBoxIcon.Error);
            }
            finally
            {
                CloseLoadingPanel();
            }
        }

        private async Task<bool> Delete()
        {
            if (string.IsNullOrEmpty(DeleteQueryName))
                return false;

            try
            {
                ShowLoadingPanel();

                var checkedRows = GetCheckedRows();

                if (checkedRows == null || checkedRows.Length == 0)
                    return true;

                await DBDelete(checkedRows.CopyToDataTable());

                return true;
            }
            catch (Exception ex)
            {
                CloseLoadingPanel();
                MessageBoxService?.Show(ex.Message, type: CommonMsgBoxIcon.Error);
                return false;
            }
            finally
            {
                CloseLoadingPanel();
            }
        }

        #endregion [ 사용자 정의 메소드 ]

        #region [ 데이터 정의 메소드 ]

        private async Task<DataTable?> DBSearch(Dictionary<string, object?> parameters)
        {
            if (QueryService == null)
                throw new Exception("QueryService가 null입니다.");

            if (string.IsNullOrEmpty(SearchQueryName))
                throw new Exception("SearchQueryName에 값이 없습니다.");

            var datatable = await QueryService.ExecuteDatatableAsync(SearchQueryName, parameters);

            return datatable;
        }

        private async Task DBDelete(DataTable datatable)
        {
            if (QueryService == null)
                throw new Exception("QueryService가 null입니다.");

            if (string.IsNullOrEmpty(DeleteQueryName))
                throw new Exception("DeleteQueryName에 값이 없습니다.");

            foreach (DataColumn column in datatable.Columns)
                column.ColumnName = column.ColumnName.Replace("@", "");

            await QueryService.ExecuteNonQuery(DeleteQueryName, datatable, new Dictionary<string, object?>()
            {
                { "REG_ID", USER_ID },
            });
        }

        #endregion [ 데이터 정의 메소드 ]
    }
}

