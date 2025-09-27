/*
* 작성자명 : 김지수
* 작성일자 : 25-02-21
* 최종수정 : 25-03-07
* 화면명 : 수용가 상세 조회
* 프로시저 : P_HMI_COM_ELEC_SRV_RST
*/

using MIT.DataUtil.Common;
using MIT.Razor.Pages.Component;
using MIT.Razor.Pages.Component.Charts;
using MIT.Razor.Pages.Component.DataEdits;
using MIT.Razor.Pages.Component.Grid;
using MIT.Razor.Pages.Component.MessageBox;
using System.Data;

namespace MIT.UI.UMS
{
    public class UM1030QA1Base : CommonUIComponentBase
    {
        protected string?COM_ID{ get; set; }
        protected CommonDateEdit? YYMM{ get; set; }


        protected CommonGrid? Grd1 { get; set; }
        protected CommonGrid? Grd2 { get; set; }
        protected CommonGrid? Grd3 { get; set; }
        protected CommonGrid? Grd4 { get; set; }
        protected MitChartLine? Chart1 { get; set; }
        protected override void OnInitialized()
        {
            
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
            if (Grd4 == null || Grd4.Grid == null)
                return;
            Grd4.SetCellStyle("MM","합 계");
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
            parameters.Add("COM_ID", COM_ID.ToStringTrim());
        }

        #endregion [ 사용자 이벤트 함수 ]

        #region [ 사용자 정의 메소드 ]

        private async Task Search()
        {
            

            try
            {
                ShowLoadingPanel();
                var DBSearchList = new List<(CommonGrid, Func<Task<DataTable?>>)>()
                {
                    (Grd1, DBSearchElec),
                    (Grd2, DBSearchSteam),
                    (Grd3, DBSearchWater),
                    (Grd4, DBSearchSale),
                };

                foreach (var (grid, dbSearch) in DBSearchList)
                {
                    if (grid == null)
                        continue;
                    grid.DataSource = await dbSearch();
                    await grid.PostEditorAsync();
                }
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

        private async Task<DataTable?> DBSearchElec()
        {
            if (QueryService == null)
                return null;

            var datatable = await QueryService.ExecuteDatatableAsync("P_HMI_COM_ELEC_SRV_RST", new Dictionary<string, object?>()
                {
                    {"COM_ID", COM_ID.ToStringTrim() },
                    {"YYYY_MM", YYMM ?.EditValue ?.Substring(0, 7) },
                });

            return datatable;
        }

        private async Task<DataTable?> DBSearchSteam()
        {
            if (QueryService == null)
                return null;

            var datatable = await QueryService.ExecuteDatatableAsync("P_HMI_COM_STM_SRV_RST", new Dictionary<string, object?>()
                {
                    {"COM_ID", COM_ID.ToStringTrim() },
                    {"YYYY_MM", YYMM?.EditValue ?.Substring(0, 7) },
                });

            return datatable;
        }

        private async Task<DataTable?> DBSearchWater()
        {
            if (QueryService == null)
                return null;

            var datatable = await QueryService.ExecuteDatatableAsync("P_HMI_COM_WTR_SRV_RST", new Dictionary<string, object?>()
                {
                    {"COM_ID", COM_ID.ToStringTrim() },
                    {"YYYY_MM", YYMM?.EditValue ?.Substring(0, 7) },
                });

            return datatable;
        }

        private async Task<DataTable?> DBSearchSale()
        {
            if (QueryService == null)
                return null;

            var datatable = await QueryService.ExecuteDatatableAsync("P_HMI_MNTH_SRV_SAL_RST", new Dictionary<string, object?>()
                {
                    {"COM_ID", COM_ID.ToStringTrim() },
                    {"YYYYMM", YYMM?.EditValue ?.Substring(0, 7) },
                });

            return datatable;
        }

        #endregion [ 데이터 정의 메소드 ]
    }
}
