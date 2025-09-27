/*
* 작성자명 : jskim
* 작성일자 : 25-04-14
* 최종수정 : 25-04-15
* 프로시저 : p_get_daily_peak
*/
using DevExpress.Blazor;
using DevExpress.XtraPrinting.Native;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MIT.DataUtil.Common;
using MIT.Razor.Pages;
using MIT.Razor.Pages.Component;
using MIT.Razor.Pages.Component.DataEdits;
using MIT.Razor.Pages.Component.Grid;
using MIT.Razor.Pages.Component.MessageBox;
using MIT.ServiceModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace MIT.UI.UMS
{
    public class UM3140QA1Base : CommonUIComponentBase
    {
        protected IEnumerable<string> Periodes = new[] { "일간", "월간" };
        private string _selectedPeriod = "일간";
        protected string SelectedPeriod
        {
            get { return _selectedPeriod; }
            set
            {
                _selectedPeriod = value;
                Search();
            }
        }


        protected CommonDateEdit YYMM { get; set; }

        public CommCode SelComp { get; set; }
        protected MitCombo2? ComObj { get; set; }
        protected MitTagBox? FDR_Tag { get; set; }


        protected List<CommCode> FDR_DATA { get; set; }
        protected IEnumerable<CommCode> FDR_Tag_Value { get; set; } = new List<CommCode>();
        public string FDR_ID
        {
            get
            {
                return string.Join(",", FDR_Tag_Value?.AsEnumerable().Select(r => r.Name));
            }
            set
            {

            }
        }





        protected List<string> SeriesList { get; set; } = new List<string>(); 
        protected List<PeakData> ChartData { get; set; } = new List<PeakData>();
        protected DxChart Chart { get; set; }











        protected string?P_YYYY{ get; set; }
        protected string?P_MM{ get; set; }
        protected string?P_KIND{ get; set; }
        protected string?P_COM_ID{ get; set; }
        protected string?YYYY{ get; set; }
        protected string?MM{ get; set; }
        protected string?KIND{ get; set; }
        protected string?COM_CODE{ get; set; }
        protected string?COM_NAME{ get; set; }
        protected string?CUST_NAME{ get; set; }
        protected string?CF_DATE{ get; set; }


        
        protected class PeakData
        {
            public string InstrumentId { get; set; }
            public DateTime PeakDate { get; set; }
            public double DailyPeakValue { get; set; }
            public double ExtendedPeakValue { get; set; }
        }

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

                if (USER_TYPE != "H")
                {
                    await SetFDR_DATA(USER_ID);
                }

                //await btn_Search();
                StateHasChanged();
            }
        }

        #region [ 컨트롤 초기 세팅 ]

        private void InitControls()
        {
            
        }
        public async Task SetFDR_DATA(string comName)
        {
            if (QueryService == null)
                return;
            var dataTable = await QueryService.ExecuteDatatableAsync("P_common_code", new Dictionary<string, object?>()
            {   {"CODE", "FDR_TP" },
                {"Etc0",  comName},
                {"Etc1", "ST" },
            });
            List<CommCode> list = new List<CommCode>();
            foreach (DataRow row in dataTable.Rows)
            {
                CommCode code = new CommCode();
                code.Name = row["CM_ID"].ToString();
                code.Value = row["CM_NAME"].ToString();
                list.Add(code);
            }
            FDR_DATA = list;
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
            parameters.Add("P_YYYY", P_YYYY.ToStringTrim());
            parameters.Add("P_MM", P_MM.ToStringTrim());
            parameters.Add("P_KIND", P_KIND.ToStringTrim());
            parameters.Add("P_COM_ID", P_COM_ID.ToStringTrim());
            parameters.Add("YYYY", YYYY.ToStringTrim());
            parameters.Add("MM", MM.ToStringTrim());
            parameters.Add("KIND", KIND.ToStringTrim());
            parameters.Add("COM_CODE", COM_CODE.ToStringTrim());
            parameters.Add("COM_NAME", COM_NAME.ToStringTrim());
            parameters.Add("CUST_NAME", CUST_NAME.ToStringTrim());
            parameters.Add("CF_DATE", CF_DATE.ToStringTrim());
        }

        #endregion [ 사용자 이벤트 함수 ]

        #region [ 사용자 정의 메소드 ]

        // datatable을 받아서 ChartData를 생성하는 메소드
        private void CreateChartData(DataTable datatable)
        {
            
            ChartData = new List<PeakData>();
            SeriesList = new List<string>();

            if (datatable == null || datatable.Rows.Count == 0)
                return;

            foreach (DataRow row in datatable.Rows)
            {
                var peakData = new PeakData
                {
                    InstrumentId = row["InstrumentId"].ToString(),
                    PeakDate = Convert.ToDateTime(row["PeakDate"]),
                    DailyPeakValue = Convert.ToDouble(row["DailyPeakValue"]),
                    ExtendedPeakValue = Convert.ToDouble(row["ExtendedPeakValue"])
                };
                ChartData.Add(peakData);

                // SeriesList에 InstrumentId가 없으면 추가
                if (!SeriesList.Contains(peakData.InstrumentId))
                {
                    SeriesList.Add(peakData.InstrumentId);
                }
            }
        }

        private async Task Search()
        {
            try
            {
                ShowLoadingPanel();
                
                var datasource = await DBSearch();

                CreateChartData(datasource);

                StateHasChanged();
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

            var datatable = await QueryService.ExecuteDatatableAsync("p_get_daily_peak", new Dictionary<string, object?>()
            {
                {"MC_ID", FDR_ID },
                {"YYMM", YYMM.EditValue?.Substring(0,7) },
                {"TYPE", SelectedPeriod == "일간" ? "D" : "M" }
            });

            return datatable;
        }

        #endregion [ 데이터 정의 메소드 ]
    }
}
