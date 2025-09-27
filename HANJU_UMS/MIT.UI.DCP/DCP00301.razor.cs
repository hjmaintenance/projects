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
    public class DCP00301Base : CommonUIComponentBase
    {
        protected CommonTextBox? txt_MSG_ID { get; set; }

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

        protected void OnJobStart(GridDataColumnCellDisplayTemplateContext context)
        {
            var item = context.DataItem as DataRowView;

            if (context == null || item == null || item.Row == null)
                return;

            MessageBoxService?.Show("작업 대기열에 추가하시겠습니까?", buttons: CommonMsgButtons.YesNo, CloseCallBack: OnJobStartCallback);
        }

        protected async Task OnJobStartCallback(CommonMsgResult result)
        {
            if (result != CommonMsgResult.Yes)
                return;

            if (Grd1 == null)
                return;
            
            if (await SaveJobAdd(Grd1.GetFocusedRowCellValue("MSG_ID").ToStringTrim()))
            {
                MessageBoxService?.Show("작업 대기열 추가하였습니다.");
                await Search();
            }
            else
            {
                MessageBoxService?.Show("작업 대기열 추가 실패하였습니다.");
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

        private async Task<bool> SaveJobAdd(string MSG_ID)
        {
            try
            {
                ShowLoadingPanel();

                await DBSaveJobAdd(MSG_ID);
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

            return true;
        }

        #endregion [ 사용자 정의 메소드 ]

        #region [ 데이터 정의 메소드 ]

        private async Task<DataTable?> DBSearch()
        {
            if (QueryService == null)
                throw new Exception("QueryService가 null 입니다.");

            var datatable = await QueryService.ExecuteDatatableAsync("SP_DCP00301_GRD1_SELECT", new Dictionary<string, object?>()
                {
                    { "MSG_ID", txt_MSG_ID?.EditValue },
                });
            return datatable;
        }

        private async Task DBSaveJobAdd(string MSG_ID)
        {
            if (QueryService == null)
                throw new Exception("QueryService가 null 입니다.");

            await QueryService.ExecuteNonQuery("SP_DCP00301_GRD1_JOB_ADD_SAVE", new Dictionary<string, object?>() {
                { "MSG_ID", MSG_ID },
                { "REG_ID", USER_ID },
            });
        }

        #endregion [ 데이터 정의 메소드 ]
    }
}

