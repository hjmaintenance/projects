/*
 * 작성자명 : 김지수
 * 작성일자 : 25-01-31
 * 최종수정 : 25-02-05
 * 화면명 : 증기 실시간 압력별 공급현황
 *  프로시저 :   P_HMI_STM_PRI_SRV_REAL_QTY, P_HMI_STM_HIGH_PRSS_SRV_REAL_QTY, P_HMI_STM_MID_PRSS_SRV_REAL_QTY,
 *              P_HMI_STM_HANJU_SRV_REAL_QTY, P_HMI_STM_LOW_PRSS_SRV_REAL_QTY, P_HMI_STM_OUTER_SRV_REAL_QTY,
 *              P_HMI_STM_LOTTE_CNTR_SRV_REAL_QTY, P_HMI_STM_SK_GIO_SRV_REAL_QTY, P_HMI_STM_STAC_SRV_REAL_QTY,
 *              P_HMI_STM_FR_SRV_REAL_QTY , P_HMI_STM_SRV_NET_REAL_QTY, P_HMI_STM_LOTTE_SRV_REAL_QTY,
 *              P_HMI_STM_WHANWHOA_SRV_REAL_QTY, P_HMI_UTILITY_PRD_SRV_REAL_QTY
 */
using MIT.Razor.Pages.Component;
using MIT.Razor.Pages.Component.Grid;
using MIT.Razor.Pages.Component.MessageBox;
using System.Data;

namespace MIT.UI.UMS
{
    public class UH2070QA1Base : CommonUIComponentBase,IDisposable
    {
        protected GridData[] GrdDataSet { get; } = Enumerable.Range(0, 13).Select(_ => new GridData()).ToArray();
        protected DataRow? SumDataRow { get; set; }     // 합계
        protected DataRow? UtilityDataRow { get; set; } // 유틸리티 생산 공급
        protected object? NetTotalFlowRate { get; set; } // 증기공급망계통 합계
        static private Timer? _refreshTimer;
        private readonly int _refreshInterval = 30; // 30초
        protected class GridData
        {
            public CommonGrid? Grd { get; set; }
            public Func<Task<DataTable?>> DBSearch { get; set; }
            public object? TotalFlowRate { get; set; }
            public object? TotalFlowRate2 { get; set; }
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
            InitGrdDataSet();
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
            parameters.Add("COM_ID","%");
        }

        #endregion [ 사용자 이벤트 함수 ]

        #region [ 사용자 정의 메소드 ]

        protected void InitGrdDataSet()
        {
            try
            {
                Func<Task<DataTable?>>[] dbSearchArray = {
                    DBSearchPRI,
                    DBSearchHighPress,
                    DBSearchMidPress,
                    DBSearchLowPress,
                    DBSearchHanjuSalt,
                    DBSearchOuter,
                    DBSearchLotteCtrl,
                    DBSearchSKGio,
                    DBSearchSTAC,
                    DBSearchLotte,
                    DBSearchHANWHA,
                    DBSearchBasf,
                    DBSearchFR
                };
                for (int i = 0; i < GrdDataSet.Length; i++)
                {
                    GrdDataSet[i].DBSearch = dbSearchArray[i];
                }
            }
            catch (Exception ex)
            {
                MessageBoxService?.Show(ex.Message, type: CommonMsgBoxIcon.Error);
            }
        }

        private async Task Search()
        {
            try
            {
                ShowLoadingPanel();

                // 그리드 요소들
                foreach (GridData grdData in GrdDataSet)
                {
                    if (grdData.Grd == null)
                        continue;

                    var dbSearch = grdData.DBSearch;

                    var datatable = await dbSearch();
                    if (datatable == null) 
                        continue;

                    grdData.TotalFlowRate = datatable.Rows[0][datatable.Columns.Count - 1]; // 유량 합계 (첫행 마지막열)
                    grdData.TotalFlowRate2 = datatable.Rows[datatable.Rows.Count - 1]["R_USEQTY"]; //마지막행 총합계

                    grdData.Grd.DataSource = datatable;
          
                    
                }
                
                
                // 합계
                var sumDataTable = await DBSearchSTMTotal();
                if (sumDataTable?.Rows.Count > 0)
                    SumDataRow = sumDataTable.Rows[0];

                // 유틸리티 생산 공급
                var utilityDataTable = await DBSearchUtility();
                if (utilityDataTable?.Rows.Count > 0)
                    UtilityDataRow = utilityDataTable.Rows[0];

                // 증기 공급망 계통 합계
                var netDataTable = await DBSearchNet();
                if (netDataTable?.Rows.Count > 0)
                    NetTotalFlowRate = netDataTable.Rows[0][netDataTable.Columns.Count - 1]; // 유량 합계 (첫행 마지막열)

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

        private async Task<DataTable?> DBSearchPRI()
        {
            if (QueryService == null)
                return null;

            var datatable = await QueryService.ExecuteDatatableAsync("P_HMI_STM_PRI_SRV_REAL_QTY", new Dictionary<string, object?>()
                {
                    {"COM_ID", "%" },
                });

            return datatable;
        }

        private async Task<DataTable?> DBSearchHighPress()
        {
            if (QueryService == null)
                return null;

            var datatable = await QueryService.ExecuteDatatableAsync("P_HMI_STM_HIGH_PRSS_SRV_REAL_QTY", new Dictionary<string, object?>()
                {
                    {"COM_ID", "%" },
                });

            return datatable;
        }

        private async Task<DataTable?> DBSearchMidPress()
        {
            if (QueryService == null)
                return null;

            var datatable = await QueryService.ExecuteDatatableAsync("P_HMI_STM_MID_PRSS_SRV_REAL_QTY", new Dictionary<string, object?>()
                {
                    {"COM_ID", "%" },
                });

            return datatable;
        }

        private async Task<DataTable?> DBSearchHanjuSalt()
        {
            if (QueryService == null)
                return null;

            var datatable = await QueryService.ExecuteDatatableAsync("P_HMI_STM_HANJU_SRV_REAL_QTY", new Dictionary<string, object?>()
                {
                    {"COM_ID", "%" },
                });

            return datatable;
        }

        private async Task<DataTable?> DBSearchLowPress()
        {
            if (QueryService == null)
                return null;

            var datatable = await QueryService.ExecuteDatatableAsync("P_HMI_STM_LOW_PRSS_SRV_REAL_QTY", new Dictionary<string, object?>()
                {
                    {"COM_ID", "%" },
                });

            return datatable;
        }

        private async Task<DataTable?> DBSearchSTMTotal()
        {
            if (QueryService == null)
                return null;

            var datatable = await QueryService.ExecuteDatatableAsync("P_HMI_STM_SUM_SRB_REAL_QTY", new Dictionary<string, object?>()
            {
                {"COM_ID", "%" },
            });

            return datatable;
        }

        private async Task<DataTable?> DBSearchOuter()
        {
            if (QueryService == null)
                return null;

            var datatable = await QueryService.ExecuteDatatableAsync("P_HMI_STM_OUTER_SRV_REAL_QTY", new Dictionary<string, object?>()
                {
                    {"COM_ID", "%" },
                });

            return datatable;
        }

        private async Task<DataTable?> DBSearchLotteCtrl()
        {
            if (QueryService == null)
                return null;

            var datatable = await QueryService.ExecuteDatatableAsync("P_HMI_STM_LOTTE_CNTR_SRV_REAL_QTY", new Dictionary<string, object?>()
                {
                    {"COM_ID", "%" },
                });

            return datatable;
        }

        private async Task<DataTable?> DBSearchSKGio()
        {
            if (QueryService == null)
                return null;

            var datatable = await QueryService.ExecuteDatatableAsync("P_HMI_STM_SK_GIO_SRV_REAL_QTY", new Dictionary<string, object?>()
                {
                    {"COM_ID", "%" },
                });

            return datatable;
        }

        private async Task<DataTable?> DBSearchSTAC()
        {
            if (QueryService == null)
                return null;

            var datatable = await QueryService.ExecuteDatatableAsync("P_HMI_STM_STAC_SRV_REAL_QTY", new Dictionary<string, object?>()
                {
                    {"COM_ID", "%" },
                });

            return datatable;
        }

        private async Task<DataTable?> DBSearchFR()
        {
            if (QueryService == null)
                return null;

            var datatable = await QueryService.ExecuteDatatableAsync("P_HMI_STM_FR_SRV_REAL_QTY", new Dictionary<string, object?>()
                {
                    {"COM_ID", "%" },
                });

            return datatable;
        }
        private async Task<DataTable?> DBSearchNet()
        {
            if (QueryService == null)
                return null;

            var datatable = await QueryService.ExecuteDatatableAsync("P_HMI_STM_SRV_NET_REAL_QTY", new Dictionary<string, object?>()
                {
                    {"COM_ID", "%" },
                });

            return datatable;
        }

        private async Task<DataTable?> DBSearchLotte()
        {
            if (QueryService == null)
                return null;

            var datatable = await QueryService.ExecuteDatatableAsync("P_HMI_STM_LOTTE_SRV_REAL_QTY", new Dictionary<string, object?>()
                {
                    {"COM_ID", "%" },
                });

            return datatable;
        }

        private async Task<DataTable?> DBSearchHANWHA()
        {
            if (QueryService == null)
                return null;

            var datatable = await QueryService.ExecuteDatatableAsync("P_HMI_STM_WHANWHOA_SRV_REAL_QTY", new Dictionary<string, object?>()
                {
                    {"COM_ID", "%" },
                });

            return datatable;
        }

    private async Task<DataTable?> DBSearchBasf() {
      if (QueryService == null)
        return null;

      var datatable = await QueryService.ExecuteDatatableAsync("P_HMI_STM_BASF_SRV_REAL_QTY", new Dictionary<string, object?>()
          {
                    {"COM_ID", "%" },
                });

      return datatable;
    }

    private async Task<DataTable?> DBSearchUtility()
        {
            if (QueryService == null)
                return null;

            var datatable = await QueryService.ExecuteDatatableAsync("P_HMI_UTILITY_PRD_SRV_REAL_QTY", new Dictionary<string, object?>()
                {
                    {"COM_ID", "%" },
                });

            return datatable;
        }

        #endregion [ 데이터 정의 메소드 ]
    }
}
