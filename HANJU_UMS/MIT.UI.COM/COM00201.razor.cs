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

namespace MIT.UI.COM
{
    public class COM00201Base : CommonUIComponentBase
    {
        protected CommonTextBox? Txt_CUST_NAME { get; set; }
        protected CommonCodeImageComboBox? Cbo_GUBUN { get; set; }

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
                "CUST_CODE", "CUST_DIV"
            });

            if (Cbo_GUBUN != null)
                await Cbo_GUBUN.LoadData();

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
            if (Grd1 == null)
                return;

            await Grd1.PostEditorAsync();

            if (!Grd1.IsCheckedRows())
            {
                MessageBoxService?.Show("선택된 데이터가 없습니다.");
                return;
            }

            if (!Grd1.IsCheckedNeedColumns())
            {
                MessageBoxService?.Show("필수 입력값이 누락되었습니다.");
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

        protected override async Task Btn_Common_Delete_Click()
        {
            if (Grd1 == null)
                return;

            await Grd1.PostEditorAsync();

            if (!Grd1.IsCheckedRows())
            {
                MessageBoxService?.Show("선택된 데이터가 없습니다.");
                return;
            }

            MessageBoxService?.Show("삭제하시겠습니까?", buttons: CommonMsgButtons.YesNo, CloseCallBack: DeleteCallback);
        }

        protected async Task DeleteCallback(CommonMsgResult result)
        {
            if (result != CommonMsgResult.Yes)
                return;

            if (await Delete())
            {
                MessageBoxService?.Show("삭제하였습니다.");
                await Search();
            }
            else
            {
                MessageBoxService?.Show("삭제에 실패하였습니다.");
            }
        }

        #endregion [ 공통 버튼 기능 ]

        #region [사용자 정의 버튼 ]

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

        private async Task<bool> Save()
        {
            if (Grd1 == null)
                return false;

            try
            {
                ShowLoadingPanel();
                await Grd1.PostEditorAsync();

                var checkedRows = Grd1.GetCheckedRows();

                if (checkedRows == null || checkedRows.Length == 0)
                    return false;

                await DBSave(checkedRows.CopyToDataTable());

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

        private async Task<bool> Delete()
        {
            if (Grd1 == null)
                return false;

            try
            {
                ShowLoadingPanel();

                await Grd1.PostEditorAsync();

                var checkedRows = Grd1.GetCheckedRows();

                if (checkedRows == null || checkedRows.Length == 0)
                    return false;

                await DBDelete(checkedRows.CopyToDataTable());
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

            var datatable = await QueryService.ExecuteDatatableAsync("SP_COM00201_GRD1_SELECT", new Dictionary<string, object?>()
                {
                    { "CUST_NAME", Txt_CUST_NAME?.Text },
                    { "CUST_DIV", Cbo_GUBUN?.EditValue },
                });
            return datatable;
        }

        private async Task DBSave(DataTable datatable)
        {
            if (QueryService == null)
                throw new Exception("QueryService가 null 입니다.");

            await QueryService.ExecuteNonQuery("SP_COM00201_GRD1_SAVE", datatable, new Dictionary<string, object?>()
                {
                    { "REG_ID", USER_ID },
                });
        }

        private async Task DBDelete(DataTable datatable)
        {
            if (QueryService == null)
                throw new Exception("QueryService가 null 입니다.");

            await QueryService.ExecuteNonQuery("SP_COM00201_GRD1_DELETE", datatable, new Dictionary<string, object?>()
                {
                    { "REG_ID", USER_ID },
                });
        }

        #endregion [ 데이터 정의 메소드 ]
    }
}

