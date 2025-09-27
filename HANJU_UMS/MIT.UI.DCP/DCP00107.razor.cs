using DevExpress.Blazor;
using DevExpress.Blazor.Internal;
using DevExpress.Utils.Filtering.Internal;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MIT.DataUtil.Common;
using MIT.Razor.Pages.Common;
using MIT.Razor.Pages.Component;
using MIT.Razor.Pages.Component.DataEdits;
using MIT.Razor.Pages.Component.DataEdits.Data;
using MIT.Razor.Pages.Component.Grid;
using MIT.Razor.Pages.Component.Grid.Data;
using MIT.Razor.Pages.Component.Grid.RepositoryItem;
using MIT.Razor.Pages.Component.MessageBox;
using MIT.ServiceModel;
using MIT.UI.LIB.DataEdits;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIT.UI.DCP
{
    public class DCP00107Base : CommonUIComponentBase
    {
        protected MsgIDImageComboBox? cbo_MSG_ID { get; set; }
        protected CommonCodeImageComboBox? cbo_RS_TYPE { get; set; }

        protected CommonGrid? Grd1 { get; set; }
        protected CommonGrid? Grd2 { get; set; }

        private DataTable _deleteCustDT = new DataTable();
        private DataTable _saveCustDT = new DataTable();

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
            {
                if (!await IsAuthenticatedCheck())
                    return;

                await InitControls();

                await Search();
            }
        }

        #region [ 컨트롤 초기 세팅 ]

        protected async Task InitControls()
        {
            _deleteCustDT.Columns.Clear();
            _deleteCustDT.Columns.Add("CUST_CODE");
            _saveCustDT.Columns.Clear();
            _saveCustDT.Columns.Add("CUST_CODE");

            // 그리드 필수 컬럼 셋팅
            Grd1?.SetNeedColumns(new string[] {
                "CUST_CODE"
            });

            Grd2?.SetNeedColumns(new string[] {
                "CUST_CODE"
            });

            if (cbo_MSG_ID != null)
                await cbo_MSG_ID.LoadData();
            if (cbo_RS_TYPE != null)
                await cbo_RS_TYPE.LoadData();

            await InvokeAsync(StateHasChanged);
        }

        #endregion [ 컨트롤 초기 세팅 ]

        #region [ 공통 버튼 기능 ]

        protected override async Task Btn_Common_Search_Click()
        {
            await Search();
        }

        protected override async Task Btn_Common_Save_Click()
        {
            if (Grd1 == null || Grd2 == null)
                return;

            await Grd1.PostEditorAsync();
            await Grd2.PostEditorAsync();

            if (_deleteCustDT.Rows.Count == 0 && _saveCustDT.Rows.Count == 0)
            {
                MessageBoxService?.Show("변경된 데이터가 없습니다.");
                return;
            }

            MessageBoxService?.Show("저장하시겠습니까?", buttons: CommonMsgButtons.YesNo, CloseCallBack: SaveCallback);
        }

        protected async Task SaveCallback(CommonMsgResult result)
        {
            if (result != CommonMsgResult.Yes)
                return;

            if (await Save())
            {
                MessageBoxService?.Show("저장하였습니다.");
                await Search();
            }
            else
            {
                MessageBoxService?.Show("저장에 실패하였습니다.");
            }
        }

        #endregion [ 공통 버튼 기능 ]

        #region [사용자 정의 버튼 ]

        #endregion [사용자 정의 버튼 ]

        #region [Validation]

        #endregion [Validation]

        #region [사용자 정의 이벤트]

        protected void btn_Cust_Move_Left_Click(MouseEventArgs e)
        {
            if (Grd1 == null || Grd2 == null)
                return;

            var checkedRows = Grd2.GetCheckedRows();

            if (checkedRows == null || checkedRows.Length == 0)
            {
                MessageBoxService?.Show("선택된 데이터가 없습니다.");
                return;
            }

            foreach (DataRow row in checkedRows)
            {
                var CUST_CODE = row["CUST_CODE"].ToStringTrim();
                var CUST_NAME = row["CUST_NAME"].ToStringTrim();

                if (_deleteCustDT.AsEnumerable().Any(s => s["CUST_CODE"].ToStringTrim().Equals(CUST_CODE)))
                    continue;

                DataRow? newRow = Grd1.AddNewRow();
                if (newRow == null)
                {
                    MessageBoxService?.Show("업체추가에 실패하였습니다.");
                    return;
                }
                newRow["CHK"] = "N";
                newRow["CUST_CODE"] = CUST_CODE;
                newRow["CUST_NAME"] = CUST_NAME;

                DataRow deleteRow = _deleteCustDT.NewRow();
                deleteRow["CUST_CODE"] = CUST_CODE;
                _deleteCustDT.Rows.Add(deleteRow);

                var saveRow = _saveCustDT.AsEnumerable().FirstOrDefault(s => s["CUST_CODE"].ToStringTrim().Equals(CUST_CODE));
                if (saveRow != null)
                    _saveCustDT.Rows.Remove(saveRow);

                row.Delete();
            }
        }

        protected void btn_Cust_Move_Right_Click(MouseEventArgs e)
        {
            if (Grd1 == null || Grd2 == null)
                return;

            var checkedRows = Grd1.GetCheckedRows();

            if (checkedRows == null || checkedRows.Length == 0)
            {
                MessageBoxService?.Show("선택된 데이터가 없습니다.");
                return;
            }

            foreach (DataRow row in checkedRows)
            {
                var CUST_CODE = row["CUST_CODE"].ToStringTrim();
                var CUST_NAME = row["CUST_NAME"].ToStringTrim();

                if (_saveCustDT.AsEnumerable().Any(s => s["CUST_CODE"].ToStringTrim().Equals(CUST_CODE)))
                    continue;

                DataRow? newRow = Grd2.AddNewRow();
                if (newRow == null)
                {
                    MessageBoxService?.Show("업체추가에 실패하였습니다.");
                    return;
                }
                newRow["CHK"] = "N";
                newRow["CUST_CODE"] = CUST_CODE;
                newRow["CUST_NAME"] = CUST_NAME;

                DataRow saveRow = _saveCustDT.NewRow();
                saveRow["CUST_CODE"] = CUST_CODE;
                _saveCustDT.Rows.Add(saveRow);

                var deleteRow = _deleteCustDT.AsEnumerable().FirstOrDefault(s => s["CUST_CODE"].ToStringTrim().Equals(CUST_CODE));
                if (deleteRow != null)
                    _deleteCustDT.Rows.Remove(deleteRow);

                row.Delete();
            }
        }

        #endregion [사용자 정의 이벤트]


        #region [ 사용자 정의 메소드 ]

        private async Task Search()
        {
            if (Grd1 == null || Grd2 == null)
                return;

            try
            {
                ShowLoadingPanel();
                var datatable = await DBSearch();

                Grd1.DataSource = datatable;

                await Grd1.PostEditorAsync();

                var datatable2 = await DBSearchDetail();

                Grd2.DataSource = datatable2;

                await Grd2.PostEditorAsync();

                _deleteCustDT.Clear();
                _saveCustDT.Clear();
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

        private async Task<bool> Save()
        {
            if (Grd1 == null || Grd2 == null)
                return false;


            await Grd1.PostEditorAsync();
            await Grd2.PostEditorAsync();

            try
            {
                ShowLoadingPanel();

                await DBSaveDetail();

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

        #endregion [ 사용자 정의 메소드 ]

        #region [ 데이터 정의 메소드 ]

        private async Task<DataTable?> DBSearch()
        {
            if (QueryService == null)
                throw new Exception("QueryService가 null 입니다.");

            var datatable = await QueryService.ExecuteDatatableAsync("SP_DCP00107_GRD1_SELECT", new Dictionary<string, object?>()
                {
                    { "MSG_ID", cbo_MSG_ID?.EditValue },
                    { "RS_TYPE", cbo_RS_TYPE?.EditValue },
                });
            return datatable;
        }

        private async Task<DataTable?> DBSearchDetail()
        {
            if (QueryService == null)
                throw new Exception("QueryService가 null 입니다.");

            var datatable = await QueryService.ExecuteDatatableAsync("SP_DCP00107_GRD2_SELECT", new Dictionary<string, object?>()
                {
                    { "MSG_ID", cbo_MSG_ID?.EditValue },
                    { "RS_TYPE", cbo_RS_TYPE?.EditValue },
                });
            return datatable;
        }

        private async Task DBSaveDetail()
        {
            if (QueryService == null)
                throw new Exception("QueryService가 null 입니다.");

            var requests = new QueryRequests();

            var deleteRequests = QueryService?.CreateQueryRequests("SP_DCP00107_GRD2_DELETE", new string[] { "CUST_CODE" }, _deleteCustDT, new Dictionary<string, object?>() {
                    { "MSG_ID", cbo_MSG_ID?.EditValue },
                    { "RS_TYPE", cbo_RS_TYPE?.EditValue },
                    { "REG_ID", USER_ID },
                });

            requests.Add(deleteRequests);

            var saveRequests = QueryService?.CreateQueryRequests("SP_DCP00107_GRD2_SAVE", new string[] { "CUST_CODE" }, _saveCustDT, new Dictionary<string, object?>() {
                    { "MSG_ID", cbo_MSG_ID?.EditValue },
                    { "RS_TYPE", cbo_RS_TYPE?.EditValue },
                    { "REG_ID", USER_ID },
                });

            requests.Add(saveRequests);

            if(QueryService != null)
                await QueryService.ExecuteNonQuery(requests);
        }

        #endregion [ 데이터 정의 메소드 ]
    }
}

