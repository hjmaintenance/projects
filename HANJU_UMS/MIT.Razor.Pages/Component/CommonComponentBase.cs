using MIT.DataUtil.Common;
using DevExpress.XtraPrinting.Native.MarkupText;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using MIT.Razor.Pages.Common;
using MIT.Razor.Pages.Service;
using MIT.Razor.Pages.Component.MessageBox;
using MIT.Razor.Pages.Component.LoadingPanel;
using Microsoft.JSInterop;
using MIT.Razor.Pages.Component.Popup;
using System.Data;
using DevExpress.XtraPrinting.Native;
//using DevExpress.DataAccess.Native.Data;
//using DevExpress.DataAccess.DataFederation;
using MIT.ServiceModel;
//using DevExpress.DataAccess.Native.Data;

namespace MIT.Razor.Pages.Component {
  /// <summary>
  /// 공통 ComponentBase 부모 클래스
  /// 주로 필요한 서비스를 공통으로 모은 부모 클래스
  /// </summary>
  public class CommonComponentBase : ComponentBase {
    /// <summary>
    /// WebService에 DB쿼리 보내는 서비스
    /// </summary>
    [Inject] protected IQueryService? QueryService_src { get; set; }
    [Inject] protected AppData appData { get; set; }
    protected CommonComponentBase QueryService { get; set; }
    /// <summary>
    /// 클라이언트 메시지 박스 띄우는 서비스
    /// </summary>
    [Inject] protected ICommonMessageBoxService? MessageBoxService { get; set; }
    /// <summary>
    /// Razor Page Route 정보 및 이동 정보 서비스
    /// </summary>
    [Inject] protected NavigationManager? NavigationManager { get; set; }
    /// <summary>
    /// 계정 관련 서비스
    /// </summary>
    [Inject] protected IAccountService? AccountService { get; set; }
    /// <summary>
    /// 로딩 패널 서비스
    /// </summary>
    [Inject] protected ILoadingPanelService? LoadingPanelService { get; set; }
    /// <summary>
    /// JavaScript 상호작용 관련 서비스
    /// </summary>
    [Inject] protected IJSRuntime? JSRuntime { get; set; }
    /// <summary>
    /// 브라우저에 SessionStorage 관련 서비스
    /// </summary>
    [Inject] protected ISessionStorageService? SessionStorageService { get; set; }
    /// <summary>
    /// 공통 팝업창 관련 서비스
    /// </summary>
    [Inject] protected ICommonPopupService? CommonPopupService { get; set; }

    [Inject] protected IConfigurationService? ConfigurationService { get; set; }


    public CommonComponentBase() {
      QueryService = this;
    }
    protected override void OnInitialized() {
      base.OnInitialized();
    }


    //service repping...

    private async Task setInitSource(Dictionary<string, object?> pp) {
      if(pp == null) pp = new Dictionary<string, object>();
      pp.Add("SS_SOURCE", this.GetType().Name);
    }

    public async Task<DataTable> ExecuteDatatableAsync_fix(string queryName, Dictionary<string, object?> parameters, string prefix = "IN_", string uriName = "BaseAddress") {
      if (QueryService_src == null)        return null;
      await setInitSource(parameters);
      return await QueryService_src.ExecuteDatatableAsync_fix(queryName, parameters, prefix);
    }

    public async Task<DataTable> ExecuteDatatableAsync(string queryName, Dictionary<string, object?> parameters, string uriName = "BaseAddress") {
      if (QueryService_src == null)         return null;
      await setInitSource(parameters);

      return await QueryService_src.ExecuteDatatableAsync(queryName, parameters);
    }


    public async Task ExecuteNonQuery(string queryName, Dictionary<string, object?> parameters) {
      if (QueryService_src == null) return;
      await setInitSource(parameters);

      await QueryService_src.ExecuteNonQuery(queryName, parameters);
    }


    public async Task ExecuteNonQuery(string queryName, DataTable datatable, Dictionary<string, object?> parameters, string uriName = "BaseAddress") {
      if (QueryService_src == null) return;
      await setInitSource(parameters);

      await QueryService_src.ExecuteNonQuery(queryName, datatable, parameters);
    }




    public QueryRequests CreateQueryRequests(string queryName, string[] columnNames, DataTable datatable, Dictionary<string, object?>? values = null) {
     return QueryService_src.CreateQueryRequests( queryName,  columnNames, datatable,  values);
    }

    public async Task ExecuteNonQuery(QueryRequests requests, string uriName = "BaseAddress") {
      if (QueryService_src == null) return ;

      foreach (QueryRequest qr in requests.QueryResponseList) {
        qr.QueryParameters.Add(new QueryParameter() { 
        ParameterName= "SS_SOURCE", ParameterValue= this.GetType().Name, Prefix="IN_"
        });
      }
      await QueryService_src.ExecuteNonQuery( requests,  uriName );
    }

    public async Task ExecuteNonQuery_fix(string queryName, DataTable datatable, Dictionary<string, object?>? values = null, string prefix = "IN_", string uriName = "BaseAddress") {
      if (QueryService_src == null) return;
      if (values != null) {
        values.Add("SS_SOURCE", this.GetType().Name);
      }
      await QueryService_src.ExecuteNonQuery_fix( queryName,  datatable,  values  ,  prefix ,  uriName );
    }


    
    public async Task<DataTable> GetCommonCode(string code, params string[] cm_ids) {
      if (QueryService_src == null) return null;

      var dics = new Dictionary<string, object>(cm_ids.Length + 1)      {
        { "CODE", code.ToUpper() }
      };

      string[] keys = { "CM_ID", "CM_name", "CM_desc", "Etc0" };
      for (int i = 0; i < cm_ids.Length && i < keys.Length; i++) {
        dics[keys[i]] = cm_ids[i];
      }

      return await QueryService.ExecuteDatatableAsync("P_COMMON_CODE", dics);
    }


    /// <summary>
    /// 로딩 Panel 보이기 호출 함수
    /// </summary>
    protected void ShowLoadingPanel() {
      LoadingPanelService?.Show();
    }

    /// <summary>
    /// 로딩 Panel 닫기 호출 함수
    /// </summary>
    protected void CloseLoadingPanel() {
      LoadingPanelService?.Close();
    }


    protected string ReVal(string fix, int i) {
      return fix + ((i > 9) ? (i + "") : ("0" + i));
    }
    protected string ReVal_N(string fix, int i) {
      return fix + i;
    }



    protected string ReVal_SP(string str) {
      if (string.IsNullOrWhiteSpace(str)) return " ";
      else return str;
    }

    /// <summary>
    /// 그리드에서 숫자데이터를 받아서 0 이면 빈문자 표기 .
    /// </summary>
    /// <param name="o"></param>
    /// <param name="size"></param>
    /// <returns></returns>
    protected string ZeroEmp(object o, int size = 0) {
      if (o == null || !double.TryParse(o.ToString(), out double number)) {
        return string.Empty;
      }

      if (number == 0) {
        return string.Empty;
      }

      return number.ToString($"F{size}");
    }




  }
}
