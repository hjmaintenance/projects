/*
* 작성자명 : 김지수
* 작성일자 : 25-02-07
* 최종수정 : 25-02-07
* 화면명 : 전기 월별 수용량 현황(회사별)
* 프로시저 : P_HMI_USE_YEARELCREC_SELECT01
*/
using MIT.DataUtil.Common;
using MIT.Razor.Pages.Component;
using MIT.Razor.Pages.Component.DataEdits;
using MIT.Razor.Pages.Component.Grid;
using MIT.Razor.Pages.Component.MessageBox;
using System.Data;

namespace MIT.UI.UMS
{
    public class UM2010QA1Base : CommonUIComponentBase
    {
        protected CommonDateEdit? YYYY{ get; set; }
        protected CommonGrid? Grd1 { get; set; }

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
            Grd1.SetCellStyle("COM_NAME", "합계");
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
            //parameters.Add("YYYY", YYYY.ToStringTrim());
            //parameters.Add("COM_ID", COM_ID.ToStringTrim());
            //parameters.Add("COM_NAME", COM_NAME.ToStringTrim());
            //parameters.Add("S01", S01.ToStringTrim());
            //parameters.Add("S02", S02.ToStringTrim());
            //parameters.Add("S03", S03.ToStringTrim());
            //parameters.Add("S04", S04.ToStringTrim());
            //parameters.Add("S05", S05.ToStringTrim());
            //parameters.Add("S06", S06.ToStringTrim());
            //parameters.Add("S07", S07.ToStringTrim());
            //parameters.Add("S08", S08.ToStringTrim());
            //parameters.Add("S09", S09.ToStringTrim());
            //parameters.Add("S10", S10.ToStringTrim());
            //parameters.Add("S11", S11.ToStringTrim());
            //parameters.Add("S12", S12.ToStringTrim());
            //parameters.Add("ST", ST.ToStringTrim());
        }

        #endregion [ 사용자 이벤트 함수 ]

        #region [ 사용자 정의 메소드 ]

        private async Task Search()
        {
            if (Grd1 == null)
                return;

            try
            {
                ShowLoadingPanel();
                
                var datasource = await DBSearch();

                Grd1.DataSource = datasource;

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

        #endregion [ 사용자 정의 메소드 ]

        #region [ 데이터 정의 메소드 ]

        private async Task<DataTable?> DBSearch()
        {
            if (QueryService == null)
                return null;

            var datatable = await QueryService.ExecuteDatatableAsync("P_HMI_USE_YEARELCREC_SELECT01", new Dictionary<string, object?>()
                {
                    {"YYYY", YYYY?.EditValue },
                    {"COM_ID", "%"},
                });

            return datatable;
        }

        #endregion [ 데이터 정의 메소드 ]
    }
}
