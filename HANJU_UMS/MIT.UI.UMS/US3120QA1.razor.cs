/*
* 작성자명 : 김지수
* 작성일자 : 25-03-10
* 최종수정 : 25-03-10
* 프로시저 : P_HMI_CHARGE_MONTHDETAIL_SELECT01
*/     
using DevExpress.Blazor;
using DevExpress.XtraPrinting.Native;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MIT.DataUtil.Common;
using MIT.Razor.Pages.Component;
using MIT.Razor.Pages.Component.DataEdits;
using MIT.Razor.Pages.Component.Grid;
using MIT.Razor.Pages.Component.MessageBox;
using MIT.Razor.Pages.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace MIT.UI.UMS
{
    public class US3120QA1Base : CommonUIComponentBase
    {
        protected CommonDateEdit? YYYY { get; set; }
        protected string COM_ID { get; set; }
        protected CommonGrid? Grd1 { get; set; }
        protected CommonGrid? Grd2 { get; set; }
        protected CommonTextBox ComText { get; set; }

        protected bool isHanju { get; set; }
        protected override async Task OnInitializedAsync()
        {
            var user = await base.SessionStorageService.GetItemAsync<User>("user");
            if (user == null) return;

            isHanju = (user.USER_TYPE != "C");
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
            {
                if (!await IsAuthenticatedCheck())
                    return;

                InitControls();

                await btn_Search();
            }
        }

        #region [ 컨트롤 초기 세팅 ]

        private void InitControls()
        {
            if (ComText != null)
            {
                ComText.Text = USER_NAME;
            }
            if (Grd1 == null || Grd1.Grid == null)
                return;

            //Grd1.SetNeedColumns(new string[]{"KEY_COLUMN" });
        }

        #endregion [ 컨트롤 초기 세팅 ]

        #region [ 공통 버튼 기능 ]

        protected override async Task Btn_Common_Search_Click()
        {
            await btn_Search();
        }

        #endregion [ 공통 버튼 기능 ]

        #region [ 사용자 버튼 기능 ]

        protected async Task btn_Search()
        {
            await Search();
        }

        #endregion [ 사용자 버튼 기능 ]

        #region [ 사용자 이벤트 함수 ]

        protected void OnInputSearchParameter(Dictionary<string, object?> parameters)
        {

        }

        #endregion [ 사용자 이벤트 함수 ]

        #region [ 사용자 정의 메소드 ]

        private async Task Search()
        {

            try
            {
                ShowLoadingPanel();

                if (Grd1 != null)
                {
                    var datasource = await DBSearchIntermediate();
                    Grd1.DataSource = datasource;
                    await Grd1.PostEditorAsync();
                }

                if (Grd2 != null)
                {
                    var datasource = await DBSearchRegular();
                    Grd2.DataSource = datasource;
                    await Grd2.PostEditorAsync();
                }
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

        #endregion [ 사용자 정의 메소드 ]

        #region [ 데이터 정의 메소드 ]

        private async Task<DataTable?> DBSearchIntermediate()
        {
            if (QueryService == null)
                return null;

            var comId = isHanju ? COM_ID : USER_ID;

            var datatable = await QueryService.ExecuteDatatableAsync("P_HMI_CHARGE_MONTH_SELECT02_01", new Dictionary<string, object?>()
                {
                    {"YYYY", YYYY?.EditValue },
                    {"COM_ID", comId.ToStringTrim() },
                });

            return datatable;
        }

        private async Task<DataTable?> DBSearchRegular()
        {
            if (QueryService == null)
                return null;

            var comId = isHanju ? COM_ID : USER_ID;

            var datatable = await QueryService.ExecuteDatatableAsync("P_HMI_CHARGE_MONTH_SELECT02_02", new Dictionary<string, object?>()
                {
                    {"YYYY", YYYY?.EditValue },
                    {"COM_ID", comId.ToStringTrim() },
                });

            return datatable;
        }

        #endregion [ 데이터 정의 메소드 ]
    }
}
