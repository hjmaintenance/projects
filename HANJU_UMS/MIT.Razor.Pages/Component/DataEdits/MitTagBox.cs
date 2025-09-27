using DevExpress.Blazor;
using Microsoft.AspNetCore.Components;
using MIT.Razor.Pages.Common;
using MIT.Razor.Pages.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIT.Razor.Pages.Component.DataEdits;
public class MitTagBox : DxTagBox<CommCode, CommCode> {

  [Inject] protected AppData? _appData { get; set; }
  /*

  <DxTagBox 
    Data = "@Data"
        TextFieldName="@nameof(Employee.Text)"
        @bind-Values="@Values"
        InputId="tbOverview" />


@code {
  IEnumerable<Employee> Data { get; set; }
  IEnumerable<Employee> Values { get; set; }
  protected override async Task OnInitializedAsync() {
    Data = await NwindDataService.GetEmployeesAsync();
    Values = Data.Take(1);
  }
}

  */

  protected override void OnInitialized() {
    base.OnInitialized();
    //InputId = "tbOverview";
    TextFieldName = "Value";
    Data = comm_dt;

    //InputId = Values;
  }




  public override async Task SetParametersAsync(ParameterView parameters) {
    base.SetParametersAsync(parameters);



    if (_prev_codeId != _codeId || _prev_etc0 != _etc0) {
      _prev_codeId = _codeId;
      _prev_etc0 = _etc0;
      await SetData();
    }

  }













  // IEnumerable<CommCode> Data { get; set; }
  // IEnumerable<CommCode> Values { get; set; } = new List<CommCode>();

  //string Value { get; set; }

  string _valStrs { get; set; } = "";
  [Parameter]
  public string ValStrs {
    get {
      var abc = "adfadfaf," + string.Join(",", Values?.AsEnumerable().Select(r => r.Name));

      StateHasChanged();
      return abc;
    }
    set {
      _valStrs = value;
      TbTextChanged.InvokeAsync(value);
    }
  }



  [Parameter]
  public EventCallback<string> TbTextChanged { get; set; }


























  protected override Task OnInitializedAsync() {
    return base.OnInitializedAsync();
  }

  [Inject]
  protected IQueryService? QueryService { get; set; }
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

  [Parameter]
  public string CmId { get; set; }




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







  [Parameter]
  public string CmName { get; set; }
  [Parameter]
  public string CmDesc { get; set; }


  [Parameter] public bool IsDataFixed { get; set; } = true;




  bool isLoad = false;
  public async Task SetData() {
    if (isLoad && IsDataFixed) return;
    //SelectedItem = null;
    isLoad = true;



    DataTable dt = await getCommonCode(_codeId);
    comm_dt.Clear();
    if (dt == null)
      return;

    foreach (DataRow row in dt.Rows) {
      comm_dt.Add(new CommCode() { Name = row["CM_ID"] + "", Value = row["CM_NAME"] + "" });
    }
    StateHasChanged();
  }







  public async Task<DataTable> getCommonCode(string code, params string[] cm_ids) {

    DataTable result = null;
    if (string.IsNullOrEmpty(Etc0) && _appData.GlobalDic.ContainsKey(code)) {

      if (_appData.GlobalDic.TryGetValue(code, out DataTable OutValue)) {
        result = OutValue;
      }

    }
    else if (!string.IsNullOrEmpty(Etc0) && _appData.GlobalDic.ContainsKey(code + "_" + Etc0.Trim())) {

      if (_appData.GlobalDic.TryGetValue(code + "_" + Etc0.Trim(), out DataTable OutValue)) {
        result = OutValue;
      }

    }

    if (result == null) {
      result =
     await QueryService.ExecuteDatatableAsync("P_COMMON_CODE", new Dictionary<string, object?>()        {
                { "CODE", code.ToUpper() },
                { "CM_ID", CmId },
                { "CM_NAME", CmName },
            { "CM_DESC", CmDesc },
            { "Etc0", Etc0 }

          });
      if (string.IsNullOrEmpty(Etc0)) {
        _appData.GlobalDic.Add(code, result);
      }
      else {
        _appData.GlobalDic.Add(code + "_" + Etc0.Trim(), result);
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






















