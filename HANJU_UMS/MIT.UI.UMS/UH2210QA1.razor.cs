/*
* 작성자명 : quristyle
* 작성일자 : 25-01-22
* 최종수정 : 25-02-03
* 화면명 :용수 일자별 공급현황
* 프로시저 : P_HMI_WATER_DAY_SRV_RST, P_HMI_WATER_DAY_SRV_RST_SELECT02T
*/
using MIT.DataUtil.Common;
using MIT.Razor.Pages;
using MIT.Razor.Pages.Component;
using MIT.Razor.Pages.Component.DataEdits;
using MIT.Razor.Pages.Component.Grid;
using MIT.Razor.Pages.Component.MessageBox;
using System.Data;

namespace MIT.UI.UMS
{
    public class UH2210QA1Base : CommonUIComponentBase
    {
        protected CommonDateEdit? YYMM{ get; set; }
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
            Grd1.SetCellStyle("com_name", "순수계,여과수계");
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

            string procedureName = string.IsNullOrWhiteSpace(COM_ID) ? "P_HMI_WATER_DAY_SRV_RST" : "P_HMI_WATER_DAY_SRV_RST_SELECT02T"; // 업체 선택 안한경우 : 업체 선택한 경우

            var datatable = await QueryService.ExecuteDatatableAsync_fix(procedureName, new Dictionary<string, object?>()
                {
                    {"YYMM", YYMM ?.EditValue?.Substring(0,7) },
                    {"COM_ID", COM_ID.ToStringTrim() },
                }, "P_");

            return datatable;
        }

        #endregion [ 데이터 정의 메소드 ]
    }
}
