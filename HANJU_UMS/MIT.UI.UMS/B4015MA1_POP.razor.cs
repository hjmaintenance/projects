/*
* 작성자명 : quristyle
* 작성일자 : 25-02-03
* 최종수정 : 25-02-03
* 프로시저 : P_HMI_COMMON_COMINFO_SELECT
*/     
using DevExpress.Blazor;
using DevExpress.XtraPrinting;
using DevExpress.XtraPrinting.Native;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using MIT.DataUtil.Common;
using MIT.Razor.Pages.Component;
using MIT.Razor.Pages.Component.Grid;
using MIT.Razor.Pages.Component.MessageBox;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using MIT.Razor.Pages;
using System.Xml;
using MIT.Razor.Pages.Common;

namespace MIT.UI.UMS {
  public partial class B4015MA1_POPBase : CommonPopupComponentBase {

    protected string? COM_ID { get; set; }
    protected string? COM_PASS { get; set; }
    protected string? COM_NAME { get; set; }
    protected string? COM_DIV { get; set; }
    protected string? COM_DIV_NAME { get; set; }
    protected string? COM_EMAIL1 { get; set; }
    protected string? COM_EMAIL2 { get; set; }
    protected string? COM_EMAIL3 { get; set; }
    protected string? COM_EMAIL4 { get; set; }
    protected string? COM_EMAIL5 { get; set; }
    protected string? COM_NUM { get; set; }
    protected string? COM_CHIEF { get; set; }
    protected string? COM_KIND { get; set; }
    protected string? COM_COND { get; set; }
    protected string? COM_ITEM { get; set; }
    protected string? COM_PHONE { get; set; }
    protected string? COM_FAX { get; set; }
    protected string? COM_ZIP1 { get; set; }
    protected string? COM_ZIP2 { get; set; }
    protected string? COM_ADDR { get; set; }
    public string? COMMENT { get; set; }
    protected string? COM_AREA { get; set; }
    protected string? COM_CITY { get; set; }
    protected string? COM_LANDING { get; set; }
    protected string? COM_LANDING_ETC { get; set; }
    protected string? COM_ETC { get; set; }
    protected string? USE_YN { get; set; }
    protected string? USE_YN2 { get; set; }
    protected string? USE_YN3 { get; set; }
    protected string? BY_X01 { get; set; }
    protected string? BY_X02 { get; set; }
    protected string? DATE_ELI { get; set; }
    protected string? SET_APP { get; set; }
    protected string? COM_NUM_S { get; set; }
    protected string? COM_CODE { get; set; }
    protected string? COM_CODE1 { get; set; }
    protected string? ORDERBY { get; set; }
    protected string? MID_CHECK_YN { get; set; }
    protected string? REGULAT_CHECK_YN { get; set; }
    protected string? ORDERBY_STM { get; set; }
    protected string? ORDERBY_WTR { get; set; }
    protected string? IN_OUT_TYP { get; set; }



    [Parameter]    public List<CommCode> comm_dt { get; set; } = new List<CommCode>();





    protected override async void OnInitialized() {
    }

    protected override async Task OnAfterRenderAsync(bool firstRender) {
      await base.OnAfterRenderAsync(firstRender);

      if (firstRender) {
        if (!await IsAuthenticatedCheck())
          return;

        InitControls();

        //await btn_Search();

      }


    }

    protected override async Task OnInitializedAsync() {

      var dr = (DataRow)PopupSetting.Parameter;
      SelDR = (DataRow)PopupSetting.Parameter;

      COM_ID = dr["COM_ID"] + "";
      COM_PASS = dr["COM_PASS"] + "";
      COM_NAME = dr["COM_NAME"] + "";
      COM_DIV = dr["COM_DIV"] + "";
      COM_DIV_NAME = dr["COM_DIV_NAME"] + "";
      COM_EMAIL1 = dr["COM_EMAIL1"] + "";
      COM_EMAIL2 = dr["COM_EMAIL2"] + "";
      COM_EMAIL3 = dr["COM_EMAIL3"] + "";
      COM_EMAIL4 = dr["COM_EMAIL4"] + "";
      COM_EMAIL5 = dr["COM_EMAIL5"] + "";
      COM_NUM = dr["COM_NUM"] + "";
      COM_CHIEF = dr["COM_CHIEF"] + "";
      COM_KIND = dr["COM_KIND"] + "";
      COM_COND = dr["COM_COND"] + "";
      COM_ITEM = dr["COM_ITEM"] + "";
      COM_PHONE = dr["COM_PHONE"] + "";
      COM_FAX = dr["COM_FAX"] + "";
      COM_ZIP1 = dr["COM_ZIP1"] + "";
      COM_ZIP2 = dr["COM_ZIP2"] + "";
      COM_ADDR = dr["COM_ADDR"] + "";
      COM_AREA = dr["COM_AREA"] + "";
      COM_CITY = dr["COM_CITY"] + "";
      COM_LANDING = dr["COM_LANDING"] + "";
      COM_LANDING_ETC = dr["COM_LANDING_ETC"] + "";
      COM_ETC = dr["COM_ETC"] + "";
      USE_YN = dr["USE_YN"] + "";
      if (string.IsNullOrEmpty(USE_YN)) {
        USE_YN = "Y";
      }
      //USE_YN2 = dr["USE_YN2"] + "";
      //USE_YN3 = dr["USE_YN3"] + "";
      BY_X01 = dr["BY_X01"] + "";
      BY_X02 = dr["BY_X02"] + "";
      DATE_ELI = dr["DATE_ELI"] + "";
      SET_APP = dr["SET_APP"] + "";
      COM_NUM_S = dr["COM_NUM_S"] + "";
      COM_CODE = dr["COM_CODE"] + "";
      COM_CODE1 = dr["COM_CODE1"] + "";
      ORDERBY = dr["ORDERBY"] + "";
      MID_CHECK_YN = dr["MID_CHECK_YN"] + "";
      REGULAT_CHECK_YN = dr["REGULAT_CHECK_YN"] + "";
      ORDERBY_STM = dr["ORDERBY_STM"] + "";
      ORDERBY_WTR = dr["ORDERBY_WTR"] + "";
      IN_OUT_TYP = dr["IN_OUT_TYP"] + "";

      COMMENT = dr["COMMENT"] + "";

      StateHasChanged();

    }

    public DataRow SelDR { get; set; }





    private void InitControls() {

    }


    protected override async Task Btn_Common_Save_Click() {
      await btn_Save();
    }




    protected async Task btn_Save() {
      //SelDR["SAVE_YN"] = "N";
      //SelDR["CHK"] = "Y";


      if (!string.IsNullOrEmpty(COM_PASS)) {
        SelDR["COM_PASS"] = EncryptHelper.EncryptSHA512(COM_PASS);
      }
      if(string.IsNullOrEmpty(SelDR["COM_ID"]+"")) {
        SelDR["COM_ID"] = COM_ID;
      }
      SelDR["COM_NAME"] = COM_NAME;
      SelDR["COM_DIV"] = COM_DIV;
      SelDR["COM_DIV_NAME"] = COM_DIV_NAME;
      SelDR["COM_EMAIL1"] = COM_EMAIL1;
      SelDR["COM_EMAIL2"] = COM_EMAIL2;
      SelDR["COM_EMAIL3"] = COM_EMAIL3;
      SelDR["COM_EMAIL4"] = COM_EMAIL4;
      SelDR["COM_EMAIL5"] = COM_EMAIL5;
      SelDR["COM_NUM"] = COM_NUM;
      SelDR["COM_CHIEF"] = COM_CHIEF;
      SelDR["COM_KIND"] = COM_KIND;
      SelDR["COM_COND"] = COM_COND;
      SelDR["COM_ITEM"] = COM_ITEM;
      SelDR["COM_PHONE"] = COM_PHONE;
      SelDR["COM_FAX"] = COM_FAX;
      SelDR["COM_ZIP1"] = COM_ZIP1;
      SelDR["COM_ZIP2"] = COM_ZIP2;
      SelDR["COM_ADDR"] = COM_ADDR;
      SelDR["COM_AREA"] = COM_AREA;
      SelDR["COM_CITY"] = COM_CITY;
      SelDR["COM_LANDING"] = COM_LANDING;
      SelDR["COM_LANDING_ETC"] = COM_LANDING_ETC;
      SelDR["COM_ETC"] = COM_ETC;
      SelDR["USE_YN"] = USE_YN;
      SelDR["BY_X01"] = BY_X01;
      SelDR["BY_X02"] = BY_X02;
      SelDR["DATE_ELI"] = DATE_ELI;
      SelDR["SET_APP"] = SET_APP;
      SelDR["COM_NUM_S"] = COM_NUM_S;

      SelDR["COMMENT"] = COMMENT;









      MessageBoxService?.Show("저장하시겠습니까?", buttons: CommonMsgButtons.YesNo, CloseCallBack: SaveCallback);
    }

    protected async Task SaveCallback(CommonMsgResult result) {
      if (result != CommonMsgResult.Yes)
        return;

      if (await Save()) {
        MessageBoxService?.Show("저장하였습니다.");
        PopupClose();
      }
    }

    
    private async Task<bool> Save() {
      try {
        ShowLoadingPanel();

        await DBSave( );

        return true;
      }
      catch (Exception ex) {
        CloseLoadingPanel();
        MessageBoxService?.Show(ex.Message, type: CommonMsgBoxIcon.Error);
        return false;
      }
      finally {
        CloseLoadingPanel();
      }
    }



    private async Task DBSave() {
      if (QueryService == null)        return;


      SelDR["SAVE_YN"] = "N";
      SelDR["CHK"] = "Y";

      //DataTable dt = SelDR.Table;

      DataTable dt = SelDR.Table.Clone();
      dt.Rows.Add(SelDR.ItemArray);


      Dictionary<string, object> dic = new Dictionary<string, object?>()                {
                { "REG_ID", USER_ID },
                };

      await QueryService.ExecuteNonQuery_fix("P_HMI_COMMON_COMINFO_SAVE", dt, dic, "P_");
    }

  


  }





}
