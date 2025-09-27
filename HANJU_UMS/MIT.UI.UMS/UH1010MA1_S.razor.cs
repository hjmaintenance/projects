using MIT.Razor.Pages.Component;
using MIT.Razor.Pages.Component.Grid;
using MIT.Razor.Pages.Component.MessageBox;
using System.Data;

namespace MIT.UI.UMS;

public class UH1010MA1_SBase : CommonUIComponentBase,IDisposable {
  protected CommonGrid? Grd1 { get; set; } // 실시간 Utility 총 공급량
  protected CommonGrid? Grd2 { get; set; } // 공장별 공급량
  protected CommonGrid? Grd4 { get; set; } // 회사별 공급량

  protected DataRow? currentRow;   // 수전/발전

    static private Timer? _refreshTimer;
    private readonly int _refreshInterval = 30; // 30초

    protected override async Task OnInitializedAsync() {
    //InitControls();



  }


  protected override async Task OnAfterRenderAsync(bool firstRender) {
    if (firstRender) {
      if (!await IsAuthenticatedCheck())        return;

      await Search();

            // 타이머 설정 & 시작
            if (_refreshTimer == null)
            {
                _refreshTimer = new Timer(async _ =>
                {

                  if (IsActive) { 
                    // 확장된 탭들 데이터 갱신
                    await Search();

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

    protected override async Task Btn_Common_Search_Click() {
    if (!await IsAuthenticatedCheck())      return;

    await Search();
  }

  protected async Task SetCurrentRow() {
    try {
      DataTable? table = await SearchDBFMSRealGen();

      // 테이블에 최소 1개 행이 있다면 첫 번째 행을 currentRow에 저장
      if (table?.Rows.Count > 0) {

        currentRow = table.Rows[0];
      }
    }
    catch (Exception ex) {
      MessageBoxService?.Show(ex.Message, type: CommonMsgBoxIcon.Error);
    }
  }

  protected async Task Search() {
    // 반복코드를 줄이기 위해 Dictionary로 묶어서 처리
    Dictionary<CommonGrid?, Func<Task<DataTable?>>> GrdFuncDict = new Dictionary<CommonGrid?, Func<Task<DataTable?>>>{
              { Grd1, SearchDBTotalSupply},
              { Grd2, SearchDBFactorySupply},
              { Grd4, SearchDBCompanySupply }
          };

    try {
      ShowLoadingPanel();

      foreach (CommonGrid grd in GrdFuncDict.Keys) {
        if (grd == null)
          continue;

        var dataTable = await GrdFuncDict[grd]();
        if (dataTable == null)
          return;
        grd.DataSource = dataTable;
      }

      await SetCurrentRow();

      StateHasChanged();
    }
    catch (Exception ex) {
      CloseLoadingPanel();
      MessageBoxService?.Show(ex.Message, type: CommonMsgBoxIcon.Error);
    }
    finally {
      CloseLoadingPanel();

    }
  }

    protected async Task btn_Save_Maintenance_YN(string value, string com_id = "", string mc_id = "", string gauge_section = "")
    {
        Save_Maintenance_YN(value, com_id, mc_id, gauge_section);

    }






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

    private async Task<DataTable?> SearchDBTotalSupply() {
    if (QueryService == null)
      throw new Exception("QueryService가 null 입니다.");

    var datatable = await QueryService.ExecuteDatatableAsync("P_HMI_REAL_ELEC_TOT_SPPLY_QTY", new Dictionary<string, object?>()
    {
              { "COM_ID", "%"},
          });

    return datatable;
  }

  private async Task<DataTable?> SearchDBFactorySupply() {
    if (QueryService == null)
      throw new Exception("QueryService가 null 입니다.");

    var datatable = await QueryService.ExecuteDatatableAsync("P_HMI_REAL_ELEC_FACT_SPPLY_QTY", new Dictionary<string, object?>()
    {
              { "P_COM_ID", "%"},
          });

    return datatable;
  }

  private async Task<DataTable?> SearchDBFMSRealGen() {
    if (QueryService == null)
      throw new Exception("QueryService가 null 입니다.");

    var datatable = await QueryService.ExecuteDatatableAsync("P_HMI_FMS_REAL_GEN_RST", (Dictionary<string, object?>)null);

    return datatable;
  }

  private async Task<DataTable?> SearchDBCompanySupply() {
    if (QueryService == null)
      throw new Exception("QueryService가 null 입니다.");

    var datatable = await QueryService.ExecuteDatatableAsync("P_HMI_REAL_ELEC_COMP_SPPLY_QTY", new Dictionary<string, object?>()    {
              { "COM_ID", "%"},
          });

    return datatable;
  }
}
