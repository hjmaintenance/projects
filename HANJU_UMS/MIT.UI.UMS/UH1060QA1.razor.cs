/*
* 작성자명 : 김지수
* 작성일자 : 25-01-23
* 최종수정 : 25-01-23
* 화명명 : 용수 계량기별 모니터링 현황
* 프로시저 : P_HMI_WATER_DW_MACH_MNTR_SELECT01, P_HMI_WATER_FW_MACH_MNTR_SELECT01
*/
using MIT.Razor.Pages.Component;
using MIT.Razor.Pages.Component.Grid;
using MIT.Razor.Pages.Component.MessageBox;
using System.Data;

namespace MIT.UI.UMS
{
    public class UH1060QA1Base : CommonUIComponentBase, IDisposable
    {
        protected CommonGrid? Grd1 { get; set; }
        
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
                // 타이머 설정 & 시작
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
            if (!(Grd1 == null || Grd1.Grid == null))
            {
                Grd1.SetCellStyle("COM_MC", "입주사계");
                Grd1.SetCellStyle("COM_MC", "제염소계", "row2");
                Grd1.SetCellStyle("COM_MC", "합계", "row3");
            }
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

                Grd1.DataSource = await DBSearchDWFW();

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

        private async Task<DataTable?> DBSearchDW()
        {
            if (QueryService == null)
                return null;

            var datatable = await QueryService.ExecuteDatatableAsync("P_HMI_WATER_DW_MACH_MNTR_SELECT01", new Dictionary<string, object?>()
                {
                    {"COM_ID", "%" },
                });

            return datatable;
        }

        private async Task<DataTable?> DBSearchFW()
        {
            if (QueryService == null)
                return null;

            var datatable = await QueryService.ExecuteDatatableAsync("P_HMI_WATER_FW_MACH_MNTR_SELECT01", new Dictionary<string, object?>()
                {
                    {"COM_ID", "%" },
                });

            return datatable;
        }

        private async Task<DataTable?> DBSearchDWFW() {
          if (QueryService == null)
            return null;

          var datatable = await QueryService.ExecuteDatatableAsync("P_HMI_WATER_DWFW_MACH_MNTR_SELECT01_V01", new Dictionary<string, object?>()
              {
                  {"COM_ID", "%" },
              });

          return datatable;
        }

    #endregion [ 데이터 정의 메소드 ]
  }
}
