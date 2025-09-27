/*
* 작성자명 : 김지수
* 작성일자 : 25-02-07
* 최종수정 : 25-02-07
* 화면명 : 검침표 확인 조회
* 프로시저 : P_HMI_CHECKLIST_SELECT01
*/
using MIT.DataUtil.Common;
using MIT.Razor.Pages.Component;
using MIT.Razor.Pages.Component.DataEdits;
using MIT.Razor.Pages.Component.Grid;
using MIT.Razor.Pages.Component.MessageBox;
using System.Data;

namespace MIT.UI.UMS
{
    public class UM5110QA1Base : CommonUIComponentBase
    {
        protected CommonDateEdit?YYMM{ get; set; }
        protected string? MeterReadingId { get; set; }
        protected string?COM_ID{ get; set; }
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
            var datatable = await QueryService.ExecuteDatatableAsync_fix("P_HMI_CHECKLIST_SELECT01", new Dictionary<string, object?>()
                {
                    {"YYYY", YYMM?.EditValue?.Split("-")[0] },
                    {"MM", YYMM?.EditValue?.Split("-")[1]},
                    {"KIND", MeterReadingId?.ToStringTrim()},
                    {"COM_ID", COM_ID?.ToStringTrim() },
                }, "P_");

            return datatable;
        }

        #endregion [ 데이터 정의 메소드 ]
    }
}

