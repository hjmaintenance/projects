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
using MIT.Razor.Pages.Component.DataEdits;
using MIT.Razor.Pages.Service;
using MIT.ServiceModel;

namespace MIT.UI.UMS {
  public partial class Z9010MA1_POPBase : CommonPopupComponentBase {

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



    protected string? START_INDEX { get; set; }
    protected string? PAGE_SIZE { get; set; }
    protected string? BOARD_ID { get; set; }
    protected string? BOARD_NUM { get; set; }
    protected string? S_TITLE { get; set; }
    protected string? S_USER_NAME { get; set; }
    protected string? BO_USER_ID { get; set; }
    protected string? BO_USER_NAME { get; set; }
    protected string? BOARD_TITLE { get; set; }
    protected string? BOARD_CONTENT { get; set; }



    protected DxHtmlEditor BoardContCtl { get; set; }



    protected string? FILE_SIZE { get; set; }
    protected string? FILE_NAME { get; set; }
    protected string? FILE_ID { get; set; }
    protected string? READ_NUM { get; set; }
    protected string? THREAD_NUM { get; set; }
    protected string? REF_LEVEL { get; set; }
    protected string? HTML_YN { get; set; }
    protected string? USER_EMAIL { get; set; }
    protected string? DATE_WRITE { get; set; }
    protected string? ROW_INDEX { get; set; }


    protected  string FILE_REAL_NAME = "";
    protected  string FILE_GID = "";



    bool isNewBoard = false;

    protected CommonCheckBox USE_POP { get; set; }
    protected CommonDateEdit POP_EXPIRE_DATE { get; set; }


    protected DxUpload FileUploadComp { get; set; }


    [Parameter] public List<CommCode> comm_dt { get; set; } = new List<CommCode>();





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


    }

    public DataRow SelDR { get; set; }





    private async Task InitControls() {

      var dr = (DataRow)PopupSetting.Parameter;
      SelDR = (DataRow)PopupSetting.Parameter;


      BOARD_ID = dr["BOARD_ID"] + "";

      if (string.IsNullOrEmpty(BOARD_ID)) { // 신규
        isNewBoard = true;
        BO_USER_NAME = USER_NAME;
        BO_USER_ID = USER_ID;
        USE_POP.EditValue = "N";
                POP_EXPIRE_DATE.ReadOnly = true;
                BOARD_ID = "Notice";
        FILE_GID = DateTime.Now.Ticks.ToString() + "_G";

      } else { // 기존

        BO_USER_NAME = dr["USER_NAME"] + "";
        BO_USER_ID = dr["USER_ID"] + "";
        USER_EMAIL = dr["USER_EMAIL"] + "";
        BOARD_TITLE = dr["BOARD_TITLE"] + "";
        BOARD_CONTENT = dr["BOARD_CONTENT"] + "";
        USE_POP.EditValue = (dr["USE_POP"] + "" == "Y") ? "Y" : "N";
                POP_EXPIRE_DATE.ReadOnly = dr["USE_POP"] + "" == "N";
        if (!string.IsNullOrEmpty(dr["POP_EXPIRE_DATE"]+ "")) POP_EXPIRE_DATE.EditValue = dr["POP_EXPIRE_DATE"]+"";
        FILE_GID = dr["FILE_ID"] + "";

        if (string.IsNullOrEmpty(FILE_GID)) { // 신규
          FILE_GID = DateTime.Now.Ticks.ToString() + "_A";
        } else {


          filelist = new List<UploadResult>();
          var dt = await QueryService.ExecuteDatatableAsync("SP_GetFilesByFgid", new Dictionary<string, object?>() {
                    {"FILE_GID", FILE_GID }
                });

          if (dt != null) {
            foreach(DataRow d in dt.Rows) {
              filelist.Add(
                
                new UploadResult() {
                  FileName = d["FILE_NAME"] + "",
                  FileSize = d["FILE_SIZE"] + "",
                  File_Gid = d["FILE_GID"] + "",
                  File_id = d["FILE_ID"] + ""
                }
                
                );
            }
          }

            //filelist


          }

      }

      StateHasChanged();
    }




    public List<UploadResult> filelist = new List<UploadResult>();


    protected override async Task Btn_Common_Save_Click() {
      await btn_Save();
    }



    public string htmlCont = "";

    public void ContChange(string str) {
      BOARD_CONTENT = str;
    }

    protected async Task btn_Save() {


      SelDR["USER_NAME"] = BO_USER_NAME;
      SelDR["USER_ID"] = BO_USER_ID;

      SelDR["USER_EMAIL"] = USER_EMAIL;
      SelDR["BOARD_ID"] = BOARD_ID;
      SelDR["BOARD_TITLE"] = BOARD_TITLE;
      SelDR["BOARD_CONTENT"] = BOARD_CONTENT;
      SelDR["USE_POP"] = USE_POP.EditValue;
            SelDR["POP_EXPIRE_DATE"] = POP_EXPIRE_DATE.EditValue;
            SelDR["FILE_ID"] = FILE_GID + "";

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

        await DBSave();

        return true;
      } catch (Exception ex) {
        CloseLoadingPanel();
        MessageBoxService?.Show(ex.Message, type: CommonMsgBoxIcon.Error);
        return false;
      } finally {
        CloseLoadingPanel();
      }
    }



    private async Task DBSave() {
      if (QueryService == null) return;

      DataTable dt = SelDR.Table.Clone();
      dt.Rows.Add(SelDR.ItemArray);

      Dictionary<string, object> dic = new Dictionary<string, object?>() {
        { "FILE_GID", FILE_GID }
      };

      string proc_name = "P_HMI_COMMUNITY_BOARDINFO_UPDATE01";
      string fix_name = "";
      if (isNewBoard) {
        proc_name = "P_HMI_COMMUNITY_BOARDINFO_INSERT01";
      } else {
        proc_name = "P_HMI_COMMUNITY_BOARDINFO_UPDATE01";
        fix_name = "IN_";
      }
      await QueryService.ExecuteNonQuery_fix(proc_name, dt, dic, fix_name);




    }




  }



}