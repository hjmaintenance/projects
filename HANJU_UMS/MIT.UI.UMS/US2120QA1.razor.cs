/*
* 작성자명 : 김지수
* 작성일자 : 25-02-06
* 최종수정 : 25-02-06
* 화면명 : 증기 일별사용량 현황
* 프로시저 : P_HMI_USE_MONTHSTMDAYTOTAL_SELECT01
*/
using MIT.DataUtil.Common;
using MIT.Razor.Pages.Component;
using MIT.Razor.Pages.Component.DataEdits;
using MIT.Razor.Pages.Component.Grid;
using MIT.Razor.Pages.Component.MessageBox;
using System.Data;
using MIT.Razor.Pages.Component.Charts;
using MIT.Razor.Pages.Data;

namespace MIT.UI.UMS
{
    public class US2120QA1Base : CommonUIComponentBase
    {
        protected CommonDateEdit?YYMM{ get; set; }
        protected string?COM_ID{ get; set; }
        protected string?USE_QTY_TYPE{ get; set; }
        protected string? UseQtyTypeLegend { get; set; } = "사용량";
        protected CommonGrid? Grd1 { get; set; }
        protected MitChartLine? Chart1 { get; set; }

        protected CommonTextBox ComText { get; set; }
        protected bool isHanju { get; set; }
        protected override async Task OnInitializedAsync()
        {
            // 랜더링 전에 한주사용자인지 수용가인지 판별
            var user = await base.SessionStorageService.GetItemAsync<User>("user");
            if (user == null) return;

            isHanju = (user.USER_TYPE != "C");
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
            {
                if (!await IsAuthenticatedCheck())
                    return;

                InitControls();

                await btn_Search();
            }
        }

        #region [ 컨트롤 초기 세팅 ]

        private void InitControls()
        {
            if (ComText != null)
            {
                ComText.Text = USER_NAME;
            }
            if (Grd1 == null || Grd1.Grid == null)
                return;
            Grd1.SetCellStyle("DD", "합계,합 계");
            //Grd1.SetNeedColumns(new string[]{"KEY_COLUMN" });
        }

        #endregion [ 컨트롤 초기 세팅 ]

        #region [ 공통 버튼 기능 ]

        protected override async Task Btn_Common_Search_Click()
        {
            await btn_Search();
        }


        #endregion [ 공통 버튼 기능 ]

        #region [ 사용자 버튼 기능 ]

        protected async Task btn_Search()
        {
            await Search();
        }
        #endregion [ 사용자 버튼 기능 ]

        #region [ 사용자 이벤트 함수 ]

        protected void OnInputSearchParameter(Dictionary<string, object?> parameters)
        {
            
        }

        #endregion [ 사용자 이벤트 함수 ]

        #region [ 사용자 정의 메소드 ]

        private async Task Search()
        {
            if (Grd1 == null)
                return;

            try
            {
                ShowLoadingPanel();
                
                var datasource = await DBSearch();

                Grd1.DataSource = datasource;
                // 마지막 행은 합계라서 제외하고 차트 데이터로 사용
                if (datasource != null && datasource.Rows.Count > 0)
                {
                    var filteredDataSource = datasource.Clone();
                    for (int i = 0; i < datasource.Rows.Count - 1; i++)
                    {
                        filteredDataSource.ImportRow(datasource.Rows[i]);
                    }
                    Chart1.DataSource = filteredDataSource;
                }

                await Grd1.PostEditorAsync();
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

        #endregion [ 사용자 정의 메소드 ]

        #region [ 데이터 정의 메소드 ]

        private async Task<DataTable?> DBSearch()
        {
            if (QueryService == null)
                return null;

            var comId = isHanju ? COM_ID : USER_ID;

            var datatable = await QueryService.ExecuteDatatableAsync("P_HMI_USE_MONTHSTMDAYTOTAL_SELECT01", new Dictionary<string, object?>()
                {
                    {"YYMM", YYMM ?.EditValue ?.Substring(0, 7)},
                    {"COM_ID", comId.ToStringTrim() },
                });

            return datatable;
        }

        #endregion [ 데이터 정의 메소드 ]
    }
}
