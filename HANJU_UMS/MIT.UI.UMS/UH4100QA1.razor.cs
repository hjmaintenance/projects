/*
* 작성자명 : 김지수
* 작성일자 : 25-01-24
* 최종수정 : 25-01-24
* 프로시저 : P_HMI_FMS_HOUR_WTR_GEN_RST, P_HMI_FMS_HOUR_GEM_RST, P_HMI_FMS_HOUR_BOILER_RST
*/
using MIT.DataUtil.Common;
using MIT.Razor.Pages.Component;
using MIT.Razor.Pages.Component.DataEdits;
using MIT.Razor.Pages.Component.Grid;
using MIT.Razor.Pages.Component.MessageBox;
using System.Data;

namespace MIT.UI.UMS
{
    public class UH4100QA1Base : CommonUIComponentBase
    {

        protected CommonDateEdit?DATE{ get; set; }
        protected CommonTimeEdit?STIME{ get; set; }
        protected CommonTimeEdit?ETIME { get; set; }

        protected IEnumerable<string> ArrayGenType = new[] { "수전", "발전", "보일러" };
        protected string SelectedGenType { get; set; } = "수전";


        protected CommonGrid? Grd1 { get; set; }
        protected CommonGrid? Grd2 { get; set; }
        protected CommonGrid? Grd3 { get; set; }

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
            if (Grd2 == null || Grd2.Grid == null)
                return;
            if (Grd3 == null || Grd3.Grid == null)
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
            parameters.Add("DATE", DATE?.EditValue);
            parameters.Add("STIME", STIME?.EditValue);
            parameters.Add("ETIME", ETIME?.EditValue);
        }

        #endregion [ 사용자 이벤트 함수 ]

        #region [ 사용자 정의 메소드 ]

        private async Task Search()
        {
            try
            {
                ShowLoadingPanel();

                switch (SelectedGenType)
                {
                    case "수전":
                        if (Grd1 == null)
                            return;
                        Grd1.DataSource = await DBSearchWater();
                        await Grd1.PostEditorAsync();
                        break;
                    case "발전":
                        if (Grd2 == null)
                            return;
                        Grd2.DataSource = await DBSearchElec();
                        await Grd2.PostEditorAsync();
                        break;
                    case "보일러":
                        if (Grd3 == null)
                            return;
                        Grd3.DataSource = await DBSearchBoiler();
                        await Grd3.PostEditorAsync();
                        break;
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

        private async Task<DataTable?> DBSearchWater()
        {
            if (QueryService == null)
                return null;


            var datatable = await QueryService.ExecuteDatatableAsync_fix("P_HMI_FMS_HOUR_WTR_GEN_RST", new Dictionary<string, object?>()
                {
                    {"DATE", DATE?.EditValue },
                    {"STIME", STIME ?.EditValue },
                    {"ETIME", ETIME ?.EditValue },
                }, "P_");

            return datatable;
        }

        private async Task<DataTable?> DBSearchElec()
        {
            if (QueryService == null)
                return null;


            var datatable = await QueryService.ExecuteDatatableAsync_fix("P_HMI_FMS_HOUR_GEM_RST", new Dictionary<string, object?>()
                {
                    {"DATE", DATE?.EditValue },
                    {"STIME", STIME ?.EditValue },
                    {"ETIME", ETIME ?.EditValue },
                }, "P_");

            return datatable;
        }

        private async Task<DataTable?> DBSearchBoiler()
        {
            if (QueryService == null)
                return null;


            var datatable = await QueryService.ExecuteDatatableAsync_fix("P_HMI_FMS_HOUR_BOILER_RST", new Dictionary<string, object?>()
                {
                    {"DATE", DATE ?.EditValue },
                    {"STIME", STIME ?.EditValue },
                    {"ETIME", ETIME ?.EditValue },
                }, "P_");

            return datatable;
        }

        #endregion [ 데이터 정의 메소드 ]
    }
}
