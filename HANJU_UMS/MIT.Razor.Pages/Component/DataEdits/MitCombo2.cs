using DevExpress.Blazor;
using DevExpress.CodeParser;
using DevExpress.Data.Mask.Internal;
//using DevExpress.DataAccess.Native.ObjectBinding;
using Microsoft.AspNetCore.Components;
using MIT.Razor.Pages.Common;
using MIT.Razor.Pages.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MIT.Razor.Pages.Component.DataEdits;

public class MitCombo2 : DxComboBox<CommCode, CommCode> {

  [Inject] protected IQueryService? QueryService { get; set; }

  [Inject] protected AppData? _appData { get; set; }

  public List<CommCode> comm_dt { get; set; } = new List<CommCode>();



  private string _codeId;
  string _prev_codeId;
  [Parameter]
  public string CodeId {
    get {
      return _codeId;
    }
    set {
      if (_codeId != value) {
        _codeId = value;
      }
    }
  }

  [Parameter] public string CmId { get; set; }
  private string _etc0;
  string _prev_etc0;

  [Parameter]
  public string Etc0 {
    get { return _etc0; }
    set {
      if (_etc0 != value) {
        _etc0 = value;
         SetData();
      }
    }
  }


  private string _etc1;
  string _prev_etc1;

  [Parameter]
  public string Etc1 {
    get { return _etc1; }
    set {
      if (_etc1 != value) {
        _etc1 = value;
        SetData();
      }
    }
  }




  [Parameter] public string CmName { get; set; }

  [Parameter] public string CmDesc { get; set; }

  [Parameter] public bool IsAll { get; set; } = false;
  [Parameter] public bool IsDataFixed { get; set; } = true;

  protected override void OnInitialized() {
    base.OnInitialized();
    //this.Value = CmId;
    //this.Text = CmName;
    if (IsAll) {
      base.NullText = "ALL";
    }
    base.TextFieldName = "Value";
    base.ValueFieldName = "Name";
    base.Data = comm_dt;
  }



  protected override async Task OnAfterRenderAsync(bool firstRender) {
    await base.OnAfterRenderAsync(firstRender);

    if (firstRender) {


      //StateHasChanged();

    }
  }




  public override async Task SetParametersAsync(ParameterView parameters) {
    base.SetParametersAsync(parameters);

    if (_prev_codeId != _codeId || _prev_etc0 != _etc0) {
      _prev_codeId = _codeId;
      _prev_etc0 = _etc0;
      await SetData();
    }

  }

  protected override async Task OnInitializedAsync() {
    base.OnInitializedAsync();
  }



  bool isLoad = false;
  public async Task SetData() {
    if (isLoad && IsDataFixed ) return;
    //SelectedItem = null;
    isLoad = true;

    DataTable dt = await getCommonCode(_codeId);
    comm_dt.Clear();
    if (dt == null) {
      return;
    }

    if (IsAll) {
      comm_dt.Add(new CommCode {
        Name = "",
        Value = "ALL"
      });
    }

    foreach (DataRow row in dt.Rows) {
      comm_dt.Add(new CommCode {
        Name = (row["CM_ID"]?.ToString() ?? ""),
        Value = (row["CM_NAME"]?.ToString() ?? ""),
        Desc = (row["CM_DESC"]?.ToString() ?? ""),
        Others = GetDic(row)
      });
    }

    if (!IsAll && comm_dt != null && comm_dt.Count > 0) {



      var nameVal = comm_dt.FirstOrDefault<CommCode>();

     await ValueChanged.InvokeAsync(nameVal);



    }
    isLoad = false;
    StateHasChanged();
  }


  



  Dictionary<string,string> GetDic(DataRow dr) {
    Dictionary<string, string> dic = new Dictionary<string, string>();

    dr.Table.Columns.Cast<DataColumn>().ToList().ForEach(c => {
        dic.Add(c.ColumnName, dr[c].ToString() );
    });

    return dic;
  }


  /// <summary>
  /// 임시로 사용.
  /// </summary>
  public async Task<bool> SetFirstRowSelect() {
    bool result = false;
    if (comm_dt != null && comm_dt.Count > 0) {

      result = true;
    }
    StateHasChanged();
    return result;
  }


  /// <summary>
  /// 임시로 사용.
  /// </summary>
  public async Task<CommCode> GetFirstRowSelect() {
    return comm_dt.FirstOrDefault<CommCode>();
  }

  public async Task<DataTable> getCommonCode(string code, params string[] cm_ids) {

    DataTable result = null;
    if ( string.IsNullOrEmpty(Etc0) &&  _appData.GlobalDic.ContainsKey(code) ) {

      if(  _appData.GlobalDic.TryGetValue(code, out DataTable OutValue)) {
        result = OutValue;
      }

    }
    else if (!string.IsNullOrEmpty(Etc0) && !string.IsNullOrEmpty(Etc1) && _appData.GlobalDic.ContainsKey(code + "_" + Etc0.Trim() + "_" + Etc1.Trim())) {

      //if (_appData.GlobalDic.TryGetValue(code + "_" + Etc0.Trim() + "_" + Etc1.Trim(), out DataTable OutValue)) {
      //  result = OutValue;
      //}

    }
    else if (!string.IsNullOrEmpty(Etc0) && _appData.GlobalDic.ContainsKey(code + "_" + Etc0.Trim())) {

      //if (_appData.GlobalDic.TryGetValue(code + "_" + Etc0.Trim(), out DataTable OutValue)) {
      //  result = OutValue;
      //}

    }

    if (result == null) {
      result =
     await QueryService.ExecuteDatatableAsync("P_COMMON_CODE", new Dictionary<string, object>    {
            { "CODE", code.ToUpper() },
            { "CM_ID", CmId },
            { "CM_NAME", CmName },
            { "CM_DESC", CmDesc },
            { "Etc0", Etc0 },
            { "Etc1", Etc1 },
        });
      if (string.IsNullOrEmpty(Etc0)) {
        _appData.GlobalDic[code]= result;
      }
      else {
        if (string.IsNullOrEmpty(Etc1)) {
          //_appData.GlobalDic[code + "_" + Etc0.Trim()] =  result;
        }
        else {
          //_appData.GlobalDic[code + "_" + Etc0.Trim() + "_" + Etc1.Trim()] = result;
        }

      }

    }

    return result;

      //return await QueryService.ExecuteDatatableAsync("P_COMMON_CODE", new Dictionary<string, object>    {
      //      { "CODE", code.ToUpper() },
      //      { "CM_ID", CmId },
      //      { "CM_NAME", CmName },
      //      { "CM_DESC", CmDesc },
      //      { "Etc0", Etc0 }
      //  });
  }
}