     
using DevExpress.Blazor;
using DevExpress.XtraPrinting.Native;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MIT.DataUtil.Common;
using MIT.Razor.Pages.Component;
using MIT.Razor.Pages.Component.Charts;
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
/*
* 작성자명 : quristyle
* 작성일자 : 25-02-05
* 최종수정 : 25-02-05
* 프로시저 : P_HMI_USE_DAYWATTIME_SELECT01
*/
namespace MIT.UI.UMS {
  public partial class US2210QA1Base : CommonUIComponentBase {
    protected CommonDateEdit? DATE { get; set; }
    protected string? COM_ID { get; set; }


    protected CommonGrid? Grd1 { get; set; }
    protected bool VisibleFRDW { get; set; } = true;
    protected bool VisibleFRFW { get; set; } = true;
    protected MitChartLine? Chart1 { get; set; }
        protected CommonTextBox ComText { get; set; }
        protected bool isHanju { get; set; }
        protected override async Task OnInitializedAsync()
        {
            // 랜더링 전에 한주사용자인지 수용가인지 판별
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
            Grd1.SetCellStyle("HH", "합계,합 계");
            //Grd1.SetNeedColumns(new string[]{"KEY_COLUMN" });
    }

        #endregion [ 컨트롤 초기 세팅 ]
    }
}
