/*
* 작성자명 : 김지수
* 작성일자 : 25-02-04
* 최종수정 : 25-02-04
* 화면명 :시간대별 전기공급 현황
* 프로시저 : P_HMI_ELEC_COM_SERV_HOUR_RST, P_HMI_ELSE_COM_SERV_HOUR_COM_RST
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
    public class UH2060QA1Base : CommonUIComponentBase
    {
        protected CommonDateEdit?DATE{ get; set; }
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
            Grd1.SetCellStyle("FDR_NM","합계");
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

            string procedurename = string.IsNullOrWhiteSpace(COM_ID) ? "P_HMI_ELEC_COM_SERV_HOUR_RST" : "P_HMI_ELSE_COM_SERV_HOUR_COM_RST";

            var datatable = await QueryService.ExecuteDatatableAsync_fix(procedurename, new Dictionary<string, object?>()
                {
                    {"DATE", DATE?.EditValue },
                    {"COM_ID", COM_ID.ToStringTrim() }
                },"P_");

            return datatable;
        }


        #endregion [ 데이터 정의 메소드 ]
    }
}
