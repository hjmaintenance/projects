/*
* 작성자명 : 김지수
* 작성일자 : 25-01-31
* 최종수정 : 25-02-05
* 화면명 : 증기 실시간 회사별 공급 현황
* 프로시저 : P_HMI_COMP_STM_REAL_SERV_QTY, P_HMI_COMP_STM_REAL_PRESS_QTY, P_HMI_FR_STM_SERV_REAL_QTY, P_HMI_UTILITY_PRD_SRV_REAL_QTY
*/
using MIT.Razor.Pages.Component;
using MIT.Razor.Pages.Component.Grid;
using MIT.Razor.Pages.Component.MessageBox;
using System.Data;

namespace MIT.UI.UMS
{
    public class UH2100QA1Base : CommonUIComponentBase
    {
        protected CommonGrid? Grd1 { get; set; }
        protected CommonGrid? Grd2 { get; set; }
        protected DataRow? UtilityDataRow { get; set; }
        protected DataRow? PressSupDataRow { get; set; }

        static private Timer? _refreshTimer;
        private readonly int _refreshInterval = 30; // 30초

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
                if (_refreshTimer == null)
                {
                    _refreshTimer = new Timer(async _ =>
                    {

                        if (IsActive)
                        {

                            // 확장된 탭들 데이터 갱신
                            await btn_Search();

                            // UI 수동 갱신
                            //InvokeAsync(StateHasChanged);
                        }
                    }, null, TimeSpan.FromSeconds(_refreshInterval), TimeSpan.FromSeconds(_refreshInterval));
                }
            }
        }

        public void Dispose()
        {
            // 타이머 정리
            if (_refreshTimer != null)
            {
                _refreshTimer.Dispose();
                _refreshTimer = null;
            }
        }

        #region [ 컨트롤 초기 세팅 ]

        private void InitControls()
        {
            if (Grd1 == null || Grd1.Grid == null)
                return;
            Grd1.SetCellStyle("COM_NAME", "전체합계,합계,합 계");
            Grd1.SetCellStyle("COM_NAME", "제 염", "row3");
            Grd1.SetCellStyle("COM_NAME", "단지내부", "row4");
            Grd1.SetCellStyle("COM_NAME", "단지외부", "row2");
            Grd1.SetCellStyle("COM_NAME", "입주사계", "row2");
            Grd2.SetCellStyle("COM_NAME", "잉여증기계");
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
            try
            {
                ShowLoadingPanel();

                Dictionary<CommonGrid, Func<Task<DataTable?>>> GrdFuncDict = new Dictionary<CommonGrid, Func<Task<DataTable?>>>
                {
                    { Grd1, DBSearchComp },
                    { Grd2, DBSearchFR }
                };

                foreach ((CommonGrid? grd, Func<Task<DataTable?>> dbSearch) in GrdFuncDict)
                {
                    if (grd == null)
                        continue;

                    var datatable = await dbSearch();
                    if (datatable == null)
                        continue;

                    grd.DataSource = datatable;
                }

                // 유틸리티 생산 공급
                var utilityDataTable = await DBSearchUtility();

                if (utilityDataTable?.Rows.Count > 0)
                    UtilityDataRow = utilityDataTable.Rows[0];

                // 압력별 공급량
                var pressDataTable = await DBSearchPress();

                if (pressDataTable?.Rows.Count > 0)
                    PressSupDataRow = pressDataTable.Rows[0];

                StateHasChanged();
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

        private async Task<DataTable?> DBSearchComp()
        {
            if (QueryService == null)
                return null;

            var datatable = await QueryService.ExecuteDatatableAsync_fix("P_HMI_COMP_STM_REAL_SERV_QTY", new Dictionary<string, object?>()
                {
                    {"COM_ID", "%"},
                },"P_");

            return datatable;
        }

        private async Task<DataTable?> DBSearchUtility()
        {
            if (QueryService == null)
                return null;

            var datatable = await QueryService.ExecuteDatatableAsync("P_HMI_UTILITY_PRD_SRV_REAL_QTY", new Dictionary<string, object?>()
                {
                    {"COM_ID", "%"},
                });

            return datatable;
        }
        private async Task<DataTable?> DBSearchPress()
        {
            if (QueryService == null)
                return null;

            var datatable = await QueryService.ExecuteDatatableAsync_fix("P_HMI_COMP_STM_REAL_PRESS_QTY", new Dictionary<string, object?>()
                {
                    {"COM_ID", "%"},
                }, "P_");

            return datatable;
        }
        private async Task<DataTable?> DBSearchFR()
        {
            if (QueryService == null)
                return null;

            var datatable = await QueryService.ExecuteDatatableAsync_fix("P_HMI_FR_STM_SERV_REAL_QTY", new Dictionary<string, object?>()
                {
                    {"COM_ID", "%"},
                }, "P_");

            return datatable;
        }
        #endregion [ 데이터 정의 메소드 ]
    }
}
