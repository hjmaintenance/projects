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
    public class DCP00104Base : CommonUIComponentBase
    {
        protected MsgIDImageComboBox? cbo_MSG_ID { get; set; }

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
            if (cbo_MSG_ID != null)
                await cbo_MSG_ID.LoadData();


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

        /// <summary>
        /// 툴바 조회 버튼 이벤트
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        protected async Task OnCustChkYnUpdateButtonClick(ToolbarItemClickEventArgs e)
        {
            if (Grd1 == null)
                return;

            await Grd1.PostEditorAsync();

            if (!Grd1.IsCheckedRows())
            {
                MessageBoxService?.Show("선택된 데이터가 없습니다.");
                return;
            }

            MessageBoxService?.Show("업체 컬럼 지정하시겠습니까?", buttons: CommonMsgButtons.YesNo, CloseCallBack: SaveCustChkYnUpdateCallback);
        }

        protected async Task SaveCustChkYnUpdateCallback(CommonMsgResult result)
        {
            if (result != CommonMsgResult.Yes)
                return;

            if (await SaveCustChkYn())
            {
                MessageBoxService?.Show("업체 컬럼을 지정하였습니다.");
                await Search();
            }
            else
            {
                MessageBoxService?.Show("업체 컬럼 지정에 실패하였습니다.");
            }
        }

        /// <summary>
        /// 툴바 조회 버튼 이벤트
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        protected async Task OnCustChkYnDeleteButtonClick(ToolbarItemClickEventArgs e)
        {
            if (Grd1 == null)
                return;

            await Grd1.PostEditorAsync();

            if (!Grd1.IsRows())
            {
                MessageBoxService?.Show("데이터가 없습니다.");
                return;
            }

            MessageBoxService?.Show("업체 컬럼을 삭제하시겠습니까?", buttons: CommonMsgButtons.YesNo, CloseCallBack: DeleteCustChkYnUpdateCallback);
        }

        protected async Task DeleteCustChkYnUpdateCallback(CommonMsgResult result)
        {
            if (result != CommonMsgResult.Yes)
                return;

            if (await DeleteCustChkYn())
            {
                MessageBoxService?.Show("업체 컬럼을 삭제하였습니다.");
                await Search();
            }
            else
            {
                MessageBoxService?.Show("업체 컬럼 삭제에 실패하였습니다.");
            }
        }

        #endregion [사용자 정의 버튼 ]

        #region [Validation]

        #endregion [Validation]

        #region [사용자 정의 이벤트]

        #endregion [사용자 정의 이벤트]


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

        private async Task<bool> SaveCustChkYn()
        {
            if (Grd1 == null)
                return false;

            await Grd1.PostEditorAsync();

            try
            {
                ShowLoadingPanel();

                var checkedRows = Grd1.GetCheckedRows();

                if (checkedRows != null && checkedRows.Length > 0)
                {
                    string MSG_ID = checkedRows[0]["MSG_ID"].ToStringTrim();
                    string COL_IDS = checkedRows[0]["COL_ID"].ToStringTrim();

                    await DBSaveCustChkYn(MSG_ID, COL_IDS);
                }
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

        private async Task<bool> DeleteCustChkYn()
        {
            if (Grd1 == null)
                return false;

            await Grd1.PostEditorAsync();

            try
            {
                ShowLoadingPanel();

                await DBDeleteCustChkYn(cbo_MSG_ID?.EditValue.ToStringTrim());

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

            var datatable = await QueryService.ExecuteDatatableAsync("SP_DCP00104_GRD1_SELECT", new Dictionary<string, object?>()
                {
                    { "MSG_ID", cbo_MSG_ID?.EditValue },
                });
            return datatable;
        }

        private async Task DBSaveCustChkYn(string MSG_ID, string COL_ID)
        {
            if (QueryService == null)
                throw new Exception("QueryService가 null 입니다.");

            await QueryService.ExecuteNonQuery("SP_DCP00104_GRD1_CUST_CHK_YN_SAVE", new Dictionary<string, object?>()
            {
                { "MSG_ID", MSG_ID },
                { "COL_ID", COL_ID },
                { "REG_ID", USER_ID },
            });
        }

        private async Task DBDeleteCustChkYn(string? MSG_ID)
        {
            if (QueryService == null)
                throw new Exception("QueryService가 null 입니다.");

            await QueryService.ExecuteNonQuery("SP_DCP00104_GRD1_CUST_CHK_YN_DELETE", new Dictionary<string, object?>()
            {
                { "MSG_ID", MSG_ID },
                { "REG_ID", USER_ID },
            });
        }

        #endregion [ 데이터 정의 메소드 ]
    }
}

