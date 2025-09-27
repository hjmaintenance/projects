/*
* 작성자명 : 김지수
* 작성일자 : 25-01-22
* 최종수정 : 25-01-22
* 화면명 : 업체별 순시량 모니터링 현황
* 프로시저 : P_HMI_ELEC_REAL_USE_QTY_MNTR
*/
using DevExpress.Blazor;
using MIT.Razor.Pages.Component;
using MIT.Razor.Pages.Component.DataEdits;
using MIT.Razor.Pages.Component.Grid;
using MIT.Razor.Pages.Component.MessageBox;
using System.Data;

namespace MIT.UI.UMS
{
    public class UH1030QA1Base : CommonUIComponentBase
    {
        protected string?COM_ID{ get; set; }
        protected CommonDateEdit? CompDate { get; set; }
        protected DateTime SearchedDate = DateTime.Now.AddDays(-1);

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
            Grd1.SetCellStyle("COM_NAME", "2_합 계");
            Grd1.Grid.CustomizeCellDisplayText = CustomizeCellDisplayText;
            //Grd1.SetNeedColumns(new string[]{"KEY_COLUMN" });
        }
        protected void CustomizeCellDisplayText(GridCustomizeCellDisplayTextEventArgs e)
        {
            if (e.FieldName == "COM_NAME")
            {
                e.DisplayText = e.Value.ToString()?.Split("_")[1];
            }
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
            parameters.Add("COM_ID", "%");
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
                SearchedDate = CompDate != null ?DateTime.ParseExact(CompDate.EditValue,CompDate.DisplayFormat,null) : DateTime.Now.AddDays(-1);

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

            var datatable = await QueryService.ExecuteDatatableAsync("P_HMI_ELEC_REAL_USE_QTY_MNTR", new Dictionary<string, object?>()
                {
                    {"COM_ID", '%' },
                    {"DATE", CompDate.EditValue }
                });

            return datatable;
        }
        #endregion [ 데이터 정의 메소드 ]
    }
}
