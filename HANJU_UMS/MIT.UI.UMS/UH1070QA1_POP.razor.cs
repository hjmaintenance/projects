     
using DevExpress.Blazor;
using DevExpress.XtraPrinting.Native;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MIT.DataUtil.Common;
using MIT.Razor.Pages.Component;
using MIT.Razor.Pages.Component.Grid;
using MIT.Razor.Pages.Component.Charts;
using MIT.Razor.Pages.Component.MessageBox;
using MIT.Razor.Pages.Component.DataEdits;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using MIT.ServiceModel;
using MIT.Razor.Pages;
/*
* 작성자명 : quristyle
* 작성일자 : 25-03-14
* 최종수정 : 25-03-14
* 프로시저 : P_HMI_TREND_SELECT
*/
namespace MIT.UI.UMS;


public partial class UH1070QA1_POPBase : CommonPopupComponentBase {
  protected string? P_UTILITY_TYP { get; set; }
  protected string? P_DATE { get; set; }
  protected string? P_COM_ID { get; set; }
  protected string? P_MC_ID { get; set; }
  protected string? GAUGE_DATE { get; set; }
  protected string? GAUGE_MIN { get; set; }
  protected string? MC_ID { get; set; }
  protected string? R_USEQTY { get; set; }
  protected string? R_USEQTY1 { get; set; }


  protected string? _gAUGE_TYPE_Code { get; set; }
  protected string? GAUGE_TYPE_Code {
    get { return _gAUGE_TYPE_Code; }
    set {
      _gAUGE_TYPE_Code = value;
      Search2();
    }
  }
  protected CommonDateEdit? CompareDate { get; set; }


  public string _compareDateVal { get; set; }
  public string CompareDateVal { 

  get {
      return _compareDateVal;
    }
set {
      _compareDateVal = value;
      Search2();
    }
  
  }



  public DxChart _chart { get; set; }

  protected override void OnInitialized() {

  }





  public bool ShowCheckboxes { get; set; } = true;
  public List<KeyValuePair<string, string>> FdrData { get; set; } = new List<KeyValuePair<string, string>>();


  public IEnumerable<KeyValuePair<string, string>> _values { get; set; }
  public IEnumerable<KeyValuePair<string, string>> Values {
    get {
      return _values;
    }
    set {
      _values = value;
      Search2();
    }
  
  }




  // protected override async Task OnInitializedAsync() {
  //   Data = await NwindDataService.GetEmployeesAsync();
  //   Values = Data.Take(2);
  // }







  protected override async Task OnAfterRenderAsync(bool firstRender) {
    await base.OnAfterRenderAsync(firstRender);

    if (firstRender) {
      if (!await IsAuthenticatedCheck())        return;

     await InitControls();

      //await btn_Search();
    }
  }

  // 컨트롤 초기 세팅 

  private async Task InitControls() {

    await SetFdrData("");

    GAUGE_TYPE_Code = "PW";
    CompareDate.EditValue = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
    await Task.Run( async () => {

    Task.Delay(1000);
    GAUGE_TYPE_Code = "PW";
    CompareDate.EditValue = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
      StateHasChanged();

    });
    StateHasChanged();











    await Task.Run(async () => {

      //Task.Delay(5000);
      if( this.RecieveParameter != null) {
        TrendCode tc = this.RecieveParameter as TrendCode;

        CompareDate.EditValue = tc.CompreDate.ToString("yyyy-MM-dd");
        GAUGE_TYPE_Code = tc.GaugeType.ToString();
        SelComp = tc.ComId;
        //MC_ID = tc.FdrId;

        Values = new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>(tc.FdrId, tc.FdrId) };




        // 상단 표기 처리..


        CompareDt = tc.CompreDate.ToString("yyyy-MM-dd");
   TrendName =tc.TrendTypeName;
   CompanyName = tc.ComId;
        FdtType = tc.GaugeTypeName;
        UtilityName = tc.FdrId;


        CompanyName = GetDicValue("company", tc.ComId);
        //CompanyName = GetDicValue("company", tc.ComId);
        //UtilityName = GetDicValue("fdr", tc.FdrId);





        await Search();

        StateHasChanged();

      }

      StateHasChanged();

    });





















  }


  string GetDicValue(string code, string key) {
    string result = "";
    if (appData.GlobalDic.TryGetValue(code, out DataTable OutValue)) {

      foreach (DataRow d in OutValue.Rows) {
        if (d["CM_ID"].ToString() == key) {
          result = d["CM_NAME"].ToString();
          break;
        }
      }
    }
    return result;
  }






  public string CompareDt { get; set; }
  public string TrendName { get; set; }
  public string CompanyName { get; set; }
  public string FdtType { get; set; }
  public string UtilityName { get; set; }







  public bool IsSideView { get {
      return (this.RecieveParameter == null);
    } 
  }








  public string _selComp;
  public string _p_selComp;
  public string SelComp {
    get { return _selComp; }
    set {
      _selComp = value;

      SetFdrData(_selComp);
    }
  }




  protected string? _preferredLanguage { get; set; } = "실사용량(KW)";
  protected string? PreferredLanguage {
    get { return _preferredLanguage; }
    set {
      _preferredLanguage = value;
      Search2();
    }
  }




  //[Parameter] public string PreferredLanguage { get; set; } = "실사용량(KW)";
  protected CommonDateEdit SendingMailDate { get; set; }
  protected IEnumerable<string> Languages = new[] { "실사용량(KW)", "일사용량(KW)", "월사용량(KW)", "당월PEAK(KW)", "당월역율(%)" };


  protected MitCombo CompCm { get; set; }


  public async Task SetFdrData(string etc0) {

    _p_selComp = etc0;
    var dt = await GetCommonCode("fdr", "", "", "",etc0);
    List<KeyValuePair<string, string>> dic = new List<KeyValuePair<string, string>>();
    foreach (DataRow d in dt.Rows) {


      dic.Add(new KeyValuePair<string, string>(d["CM_ID"] + "", d["CM_name"] + ""));

    }
    FdrData = dic;




    StateHasChanged();

  }


  // 컨트롤 초기 세팅 end
}
