using DevExpress.Blazor;
using MIT.DataUtil.Common;
using MIT.Razor.Pages.Component;
using MIT.Razor.Pages.Component.DataEdits;
using MIT.Razor.Pages.Component.Grid;
using MIT.UI.LIB.DataEdits;
using System.Runtime.InteropServices;

namespace MIT.UI.DCP
{
    public class CPC00007Base : CommonUIComponentBase
    {

        protected CommonCustSearchLookup? cbo_CUST_CODE { get; set; }

        protected CommonDateEdit? Dte_FromDate { get; set; }
        protected CommonDateEdit? Dte_ToDate { get; set; }

        protected CommonGridDynamic? Grd1 { get; set; }


        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
            {
                if (!await IsAuthenticatedCheck())
                    return;

                await InitControls();

                await Btn_Common_Search_Click();
            }
        }

        #region [ 컨트롤 초기 세팅 ]

        protected async Task InitControls()
        {
            if (cbo_CUST_CODE != null)
                await cbo_CUST_CODE.LoadData();

            Dte_FromDate?.SetToday();
            Dte_ToDate?.SetOffsetMonthDate(1);

            await InvokeAsync(StateHasChanged);
        }

        #endregion [ 컨트롤 초기 세팅 ]

        #region [ 공통 버튼 기능 ]

        /// <summary>
        /// 공통 조회 버튼 이벤트
        /// </summary>
        /// <returns></returns>
        protected override async Task Btn_Common_Search_Click()
        {
            if (Grd1 != null)
                await Grd1.PerformSearchButtonClick();
        }

        #endregion [ 공통 버튼 기능 ]

        #region [ 사용자 이벤트 함수 ]

        /// <summary>
        /// 그리드1 조회 전에 조회에 필요한 DB 파라메터 값 셋팅
        /// </summary>
        /// <param name="parameters"></param>
        protected void Grd1_OnInputSearchParameter(Dictionary<string, object?> parameters)
        {
            parameters.Add("CUST_ID", cbo_CUST_CODE?.EditValue);
            parameters.Add("FR_DT", Dte_FromDate?.EditValue);
            parameters.Add("TO_DT", Dte_ToDate?.EditValue);
        }
      
        #endregion [ 사용자 이벤트 함수 ]

    }
}

