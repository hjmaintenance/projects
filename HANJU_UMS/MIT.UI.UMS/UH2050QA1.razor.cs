/*
* 작성자명 : 김지수
* 작성일자 : 25-02-04
* 최종수정 : 25-02-04
* 화면명: 5분주기 전기 공급 현황
* 프로시저 : P_HMI_ELEC_SERV_THREE_TIME_RST, P_HMI_ELEC_SERV_THREE_TIME_RST2
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
    public class UH2050QA1Base : CommonUIComponentBase
    {
        protected CommonDateEdit? DATE { get; set; }
        protected CommonTimeEdit? STIME { get; set; }
        protected string SearchedHour { get; set; } = "00";
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
            parameters.Add("DATE", DATE?.EditValue);
            parameters.Add("STIME", STIME?.EditValue);
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

                // db조회에 성공하면 시간 칼럼 변경을 위해 조회 시간 저장
                if (datasource?.Rows.Count > 0)
                {
                    SearchedHour = STIME?.EditValue ?? SearchedHour;
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

            //string procedureName = string.IsNullOrWhiteSpace(COM_ID) ? "P_HMI_ELEC_SERV_THREE_TIME_RST" : "P_HMI_ELEC_SERV_THREE_TIME_RST2"; // 업체 선택 안한경우 : 업체 선택한 경우

            var datatable = await QueryService.ExecuteDatatableAsync_fix("P_HMI_ELEC_SERV_THREE_TIME_RST", new Dictionary<string, object?>()
                {
                    {"DATE", DATE?.EditValue },
                    {"COM_ID", COM_ID.ToStringTrim() },
                    {"STIME", STIME?.EditValue },
                    
                }, "P_");

            return datatable;
        }

        #endregion [ 데이터 정의 메소드 ]
    }
}
