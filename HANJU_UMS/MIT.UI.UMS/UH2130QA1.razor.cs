/*
* 작성자명 : 김지수
* 작성일자 : 25-01-23
* 최종수정 : 25-02-03
* 화면명 : 증기 일자별 공급현황
* 프로시저 : P_HMI_STM_MONTH_SRV_RST
*/
using DevExpress.Blazor;
using MIT.DataUtil.Common;
using MIT.Razor.Pages;
using MIT.Razor.Pages.Component;
using MIT.Razor.Pages.Component.DataEdits;
using MIT.Razor.Pages.Component.Grid;
using MIT.Razor.Pages.Component.MessageBox;
using System.Data;

namespace MIT.UI.UMS
{
    public class UH2130QA1Base : CommonUIComponentBase
    {
        protected CommonDateEdit? YYMM{ get; set;}
        protected string? SearchedYYMM { get; set; }
        protected IEnumerable<CommCode> ComValues { get; set; } = new List<CommCode>();
        public string COM_ID
        {
            get
            {
                return string.Join(",", ComValues?.AsEnumerable().Select(r => r.Name));
            }
            set
            {

            }
        }
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
            Grd1.SetCellStyle("com_name", "합계");
            Grd1.SetCellStyle("mc_id", "소계", "row2");
            Grd1.Grid.CustomizeCellDisplayText = CustomizeCellDisplayText;

            //Grd1.SetNeedColumns(new string[]{"KEY_COLUMN" });
        }
        protected void CustomizeCellDisplayText(GridCustomizeCellDisplayTextEventArgs e)
        {
            if (e.FieldName == "com_name2")
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
            parameters.Add("COM_ID", COM_ID.ToStringTrim());
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

                // db조회에 성공하면 일자 칼럼 변경을 위해 조회 월 저장
                if (datasource?.Rows.Count > 0)
                {
                    SearchedYYMM = YYMM?.EditValue ?? SearchedYYMM;
                }

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

            //string procedureName = "P_HMI_STM_MONTH_SRV_RST";

            //if (!string.IsNullOrEmpty(COM_ID)) //.IsNullOrEmpty("xxxxxxxxxxxxxxx"))
            //    procedureName = "P_HMI_STM_MONTH_SRV_RST_SELECT02";

            var datatable = await QueryService.ExecuteDatatableAsync_fix("P_HMI_STM_MONTH_SRV_RST", new Dictionary<string, object?>()
                {
                    {"YYMM", YYMM ?.EditValue ?.Substring(0, 7) },
                    {"COM_ID", COM_ID.ToStringTrim() },
                }, "P_");

            return datatable;
        }

        #endregion [ 데이터 정의 메소드 ]
    }
}
