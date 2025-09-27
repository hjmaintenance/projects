     
using DevExpress.Blazor;
using DevExpress.XtraPrinting.Native;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MIT.DataUtil.Common;
using MIT.Razor.Pages.Component;
using MIT.Razor.Pages.Component.Grid;
using MIT.Razor.Pages.Component.Charts;
using MIT.Razor.Pages.Component.MessageBox;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using MIT.Razor.Pages.Component.DataEdits;
/*
* 작성자명 : quristyle
* 작성일자 : 25-02-10
* 최종수정 : 25-02-10
* 프로시저 : p_log_search
*/
namespace MIT.UI.UMS
{
    public partial class ZSYSLOG0001Base : CommonUIComponentBase
    {
        protected string?SRCH{ get; set; }
        protected string?IsSuccess{ get; set; }


    protected string? Danger { get; set; }
    
        protected string?Msg{ get; set; }
        protected string?ReturnCnt{ get; set; }
        protected string?log_key{ get; set; }
        protected string?proc_nm{ get; set; }
        protected string?prams{ get; set; }
        protected string?Message{ get; set; }
        protected CommonDateEdit s_dt{ get; set; }
        protected CommonDateEdit E_DT{ get; set; }


        protected CommonGrid? Grd1 { get; set; }
        protected DataTable ChartDt1 { get; set; } = new DataTable();

        protected override void OnInitialized()
        {
            
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
            if (Grd1 == null || Grd1.Grid == null)
                return;

            //Grd1.SetNeedColumns(new string[]{"KEY_COLUMN" });
        }

        #endregion [ 컨트롤 초기 세팅 ]
    }
}
