using DevExpress.Blazor;
using DevExpress.Blazor.Internal;
using Microsoft.AspNetCore.Components;
using MIT.DataUtil.Common;
using MIT.Razor.Pages.Common;
using MIT.Razor.Pages.Component;
using MIT.Razor.Pages.Component.DataEdits;
using MIT.Razor.Pages.Component.DataEdits.Data;
using MIT.Razor.Pages.Component.Grid;
using MIT.Razor.Pages.Component.Grid.Data;
using MIT.Razor.Pages.Component.Grid.RepositoryItem;
using MIT.Razor.Pages.Component.MessageBox;
using MIT.UI.LIB.DataEdits;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIT.UI.DCP
{
    public class DCP00101Base : CommonUIComponentBase
    {
        protected CommonCustSearchLookup? cbo_CUST_CODE { get; set; }

        protected CommonGrid? Grd1 { get; set; }

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
            // 그리드 필수 컬럼 셋팅
            Grd1?.SetNeedColumns(new string[] { 
                "CUST_CODE"
            });

            if (cbo_CUST_CODE != null)
                await cbo_CUST_CODE.LoadData();

            await InvokeAsync(StateHasChanged);
        }

        #endregion [ 컨트롤 초기 세팅 ]

        #region [ 공통 버튼 기능 ]

        protected override async Task Btn_Common_Search_Click()
        {
            await Search();
        }

        #endregion [ 공통 버튼 기능 ]

        #region [사용자 정의 버튼 ]

        protected void OnApiKeyReissuance(GridDataColumnCellDisplayTemplateContext context)
        {
            var item = context.DataItem as DataRowView;

            if (context == null || item == null || item.Row == null)
                return;

            MessageBoxService?.Show("API Key 발급하시겠습니까?", buttons: CommonMsgButtons.YesNo, CloseCallBack: OnApiKeyReissuanceCallback);
        }

        protected async Task OnApiKeyReissuanceCallback(CommonMsgResult result)
        {
            if (result != CommonMsgResult.Yes)
                return;

            if (Grd1 == null)
                return;

            var CUST_CODE = Grd1.GetFocusedRowCellValue("CUST_CODE").ToStringTrim();

            if (await SaveApiKeyIssued(CUST_CODE))
            {
                MessageBoxService?.Show("API Key 발급하였습니다.");
                await Search();
            }
            else
            {
                MessageBoxService?.Show("저장에 실패하였습니다.");
            }
        }

        #endregion [사용자 정의 버튼 ]


        #region [ 사용자 정의 메소드 ]

        private async Task Search()
        {
            if (Grd1 == null)
                return;

            try
            {
                ShowLoadingPanel();
                var datatable = await DBSearch();

                Grd1.DataSource = datatable;

                await Grd1.PostEditorAsync();
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

        private async Task<bool> SaveApiKeyIssued(string CUST_CODE)
        {
            if (Grd1 == null)
                return false;

            try
            {
                ShowLoadingPanel();
                await Grd1.PostEditorAsync();
                
                await DBSaveApiKeyIssued(CUST_CODE);

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

            var datatable = await QueryService.ExecuteDatatableAsync("SP_DCP00101_GRD1_SELECT", new Dictionary<string, object?>()
                {
                    { "CUST_CODE", cbo_CUST_CODE?.EditValue },
                    { "REG_ID", this.USER_ID },
                });
            return datatable;
        }

        private async Task DBSaveApiKeyIssued(string CUST_CODE)
        {
            if (QueryService == null)
                throw new Exception("QueryService가 null 입니다.");

            await QueryService.ExecuteNonQuery("SP_DCP00101_API_KEY_ISSUED_SAVE", new Dictionary<string, object?>()
            {
                { "CUST_CODE", CUST_CODE },
                { "REG_ID", USER_ID },
            });
        }

        #endregion [ 데이터 정의 메소드 ]
    }
}

