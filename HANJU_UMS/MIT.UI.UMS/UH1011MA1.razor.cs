/*
*  작성자명 : jskim
    * 작성일자 : 25-04-16
    * 최종수정 : 25-04-16
    * 화명명 : 종합 모니터링 화면(전기,증기,용수)
    * 프로시저 : P_HMI_REAL_ELEC_COMP_SPPLY_QTY, P_HMI_COMP_STM_REAL_SERV_QTY, P_HMI_WATER_DW_MACH_MNTR_SELECT01, P_HMI_WATER_FW_MACH_MNTR_SELECT01
*/
using MIT.Razor.Pages.Component;
using MIT.Razor.Pages.Component.Grid;
using MIT.Razor.Pages.Component.MessageBox;
using System.Data;

namespace MIT.UI.UMS
{
    public class UH1011MA1Base : CommonUIComponentBase, IDisposable
    {
        protected CommonGrid? Grd1 { get; set; }
        protected CommonGrid? Grd2 { get; set; }
        protected CommonGrid? Grd3 { get; set; }
        protected CommonGrid? Grd4 { get; set; }
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

                        if (IsActive) { 
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
            if (!(Grd1 == null || Grd1.Grid == null)) {
              Grd1.SetCellStyle("COM_MC", "입주사계");
              Grd1.SetCellStyle("COM_MC", "제염소계", "row2");
              Grd1.SetCellStyle("COM_MC", "합계", "row3");
            }

            //if (!(Grd2 == null || Grd2.Grid == null)) {
            //    Grd2.SetCellStyle("COM_NAME", "전체합계,합계,합 계");
            //    Grd2.SetCellStyle("COM_NAME", "제 염", "row3");
            //    Grd2.SetCellStyle("COM_NAME", "단지내부", "row4");
            //    Grd2.SetCellStyle("COM_NAME", "단지외부", "row2");
            //    Grd2.SetCellStyle("COM_NAME", "입주사계", "row2");
            //}
            //if (!(Grd3 == null || Grd3.Grid == null))
            //{
            //    Grd3.SetCellStyle("COM_MC", "순수계");
            //    Grd3.SetCellStyle("COM_MC", "제염 응축수", "row3");
            //}

            //if (!(Grd4 == null || Grd4.Grid == null))
            //{
            //    Grd4.SetCellStyle("COM_MC", "입주사계", "row2");
            //    Grd4.SetCellStyle("COM_MC", "여과수 계", "row3");
            //    Grd4.SetCellStyle("COM_MC", "제염소계");
            //}
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
            Dictionary<CommonGrid?, Func<Task<DataTable?>>> GrdFuncDict = new Dictionary<CommonGrid?, Func<Task<DataTable?>>>
            {
                { Grd1, DBSearchAll }
            };

            try
            {
                ShowLoadingPanel();
                foreach (var grd in GrdFuncDict.Keys)
                {
                    if (grd == null)
                        continue;

                    var dataTable = await GrdFuncDict[grd]();
                    if (dataTable == null)
                        continue;

                    grd.DataSource = dataTable;
                    await grd.PostEditorAsync();
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
        protected async Task btn_Save_Maintenance_YN(string value, string com_id = "", string mc_id = "", string gauge_section = "")
        {
            if ("ST" == gauge_section)
            {
                try
                {
                    ShowLoadingPanel();

                    var dataTable = await DBSearchAll();


                    foreach (DataRow dr in dataTable.Rows)
                    {
                        if (dr["COM_ID"].ToString() == com_id)
                        {
                            dr["Maintenance_YN"] = value;
                        }
                    }

                    Grd2.DataSource = dataTable;
                    await Grd2.PostEditorAsync();
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

            Save_Maintenance_YN(value, com_id, mc_id, gauge_section);

        }




        #endregion [ 사용자 정의 메소드 ]

        #region [ 데이터 정의 메소드 ]
        private async Task<bool> Save_Maintenance_YN(string value, string com_id = "", string mc_id = "", string gauge_section = "")
        {
            try
            {
                ShowLoadingPanel();


                if (QueryService != null)
                {

                    if (mc_id != "")
                    {
                        await QueryService.ExecuteNonQuery("p_update_Maintenance_YN_mc", new Dictionary<string, object?>()
                        {
                            { "MC_ID", mc_id },
                            { "MAINTENANCE_YN", value }
                        });
                    }
                    else
                    {
                        await QueryService.ExecuteNonQuery("p_update_Maintenance_YN_com", new Dictionary<string, object?>()
                        {
                            { "COM_ID", com_id },
                            { "GAUGE_SECTION", gauge_section },
                            { "MAINTENANCE_YN", value }
                        });
                    }
                }
                else return false;



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

        private async Task<DataTable?> DBSearchAll() 
        {
          if (QueryService == null)
            throw new Exception("QueryService가 null 입니다.");

          var datatable = await QueryService.ExecuteDatatableAsync("P_HMI_REAL_MONI_ALL", new Dictionary<string, object?>()    {
                  { "COM_ID", "%"},
              });

          return datatable;
        }
        private async Task<DataTable?> DBSearchELEC()
        {
            if (QueryService == null)
                throw new Exception("QueryService가 null 입니다.");

            var datatable = await QueryService.ExecuteDatatableAsync("P_HMI_REAL_ELEC_COMP_SPPLY_QTY", new Dictionary<string, object?>()    {
              { "COM_ID", "%"},
          });

            return datatable;
        }
        private async Task<DataTable?> DBSearchSTM()
        {
            if (QueryService == null)
                return null;

            var datatable = await QueryService.ExecuteDatatableAsync_fix("P_HMI_COMP_STM_REAL_SERV_QTY", new Dictionary<string, object?>()
                {
                    {"COM_ID", "%"},
                }, "P_");

            return datatable;
        }
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

        #endregion [ 데이터 정의 메소드 ]
    }
}
