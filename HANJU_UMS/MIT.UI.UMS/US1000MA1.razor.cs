/*
* 작성자명 : 김지수
* 작성일자 : 25-02-05
* 최종수정 : 25-02-05
* 프로시저 : P_HMI_COMMUNITY_BOARDLIST_SELECT01, P_HMI_USE_NOWLIST02STM_SELECT01, P_HMI_USE_NOWLIST02ELC_SELECT01, 
*            P_HMI_USE_NOWLIST02WAR_SELECT01, P_HMI_USE_NOWLIST02WAR_SELECT02, P_HMI_USE_NOWLIST02WAR_FR_SELECT01
*/
using MIT.Razor.Pages.Component;
using MIT.Razor.Pages.Component.Grid;
using MIT.Razor.Pages.Component.MessageBox;
using MIT.Razor.Pages.Component.Popup;
using System.Data;
using System.Threading.Tasks;

namespace MIT.UI.UMS
{
    public class US1000MA1Base : CommonUIComponentBase,IDisposable
    {
        static private Timer? _refreshTimer;
        private readonly int _refreshInterval = 30; // 30초
        protected CommonGrid? Grd1 { get; set; }
        protected CommonGrid? Grd2 { get; set; }
        protected CommonGrid? Grd3 { get; set; }
        protected CommonGrid? Grd4 { get; set; }
        protected CommonGrid? Grd5 { get; set; }


    protected MitNotiBoardSingle? NotiPop { get; set; }





        protected Dictionary<CommonGrid, Func<Task<DataTable?>>> GridFuncDict { get; set; } = new Dictionary<CommonGrid, Func<Task<DataTable?>>>();


        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
            {
                if (!await IsAuthenticatedCheck())
                    return;

                GridFuncDict = new Dictionary<CommonGrid, Func<Task<DataTable?>>>()
                {
                    { Grd1, DBSearchNotice},
                    { Grd2, DBSearchSteamReal},
                    { Grd3, DBSearchElecReal},
                    { Grd4, DBSearchWaterNormal},
                    { Grd5, DBSearchWaterFR}
                };

                InitControls();

                await btn_Search();


                // 타이머 설정 & 시작
                if (_refreshTimer == null)
                {
                    _refreshTimer = new Timer(async _ =>
                {

                    if(IsActive)
                    {
                        // 확장된 탭들 데이터 갱신
                        await btn_Search();

                        // UI 수동 갱신
                        //InvokeAsync(StateHasChanged);
                    }


                }, null, TimeSpan.FromSeconds(_refreshInterval), TimeSpan.FromSeconds(_refreshInterval));
                }
            }
        }

        public void Dispose()
        {
            // 타이머 정리
            if (_refreshTimer != null)
            {
                _refreshTimer.Dispose();
                _refreshTimer = null;
            }
        }

        #region [ 컨트롤 초기 세팅 ]

        private void InitControls()
        {
            foreach (CommonGrid? grd in GridFuncDict.Keys)
            {
                if (grd == null || grd.Grid == null)
                    return;
            }
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
            try
            {
                ShowLoadingPanel();

                foreach((CommonGrid? grd, Func<Task<DataTable?>> dbSearch) in GridFuncDict)
                {
                    if (grd == null)
                        continue;
                    grd.DataSource = await dbSearch();
                    await grd.PostEditorAsync();
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

        private async Task<DataTable?> DBSearchNotice()
        {
            if (QueryService == null)
                return null;

            var datatable = await QueryService.ExecuteDatatableAsync("P_HMI_COMMUNITY_BOARDLIST_SELECT01", new Dictionary<string, object?>()
                {
                    {"START_INDEX", 1},
                    {"PAGE_SIZE", 5},
                    {"BOARD_ID", "Notice"},
                    {"BOARD_NUM", "%"},
                    {"S_TITLE", "%"},
                    {"S_USER_NAME", "%" },

                });

            return datatable;
        }

        private async Task<DataTable?> DBSearchSteamReal()
        {
            if (QueryService == null)
                return null;

            var datatable = await QueryService.ExecuteDatatableAsync("P_HMI_USE_NOWLIST02STM_SELECT01", new Dictionary<string, object?>()
                {
                    {"DATE", DateTime.Today.ToString("yyyy-MM-dd")},
                    {"COM_ID", USER_ID},

                });

            return TransformDataTableSteamReal(datatable);
        }

        private async Task<DataTable?> DBSearchElecReal()
        {
            if (QueryService == null)
                return null;

            var datatable = await QueryService.ExecuteDatatableAsync("P_HMI_USE_NOWLIST02ELC_SELECT01", new Dictionary<string, object?>()
                {
                    {"DATE", DateTime.Today.ToString("yyyy-MM-dd")},
                    {"COM_ID", USER_ID},

                });

            return TransformDataTableElecReal(datatable);
        }
        private async Task<DataTable?> DBSearchWaterNormal()
        {
            var dwTable = await DBSearchDW();
            var fwTable = await DBSearchFW();

            return MergeWaterTables(dwTable, fwTable);
        }
        private async Task<DataTable?> DBSearchDW()
        {
            if (QueryService == null)
                return null;

            var datatable = await QueryService.ExecuteDatatableAsync("P_HMI_USE_NOWLIST02WAR_SELECT01", new Dictionary<string, object?>()
                {
                    {"DATE", DateTime.Today.ToString("yyyy-MM-dd")},
                    {"COM_ID", USER_ID},

                });

            return datatable;
        }

        private async Task<DataTable?> DBSearchFW()
        {
            if (QueryService == null)
                return null;

            var datatable = await QueryService.ExecuteDatatableAsync("P_HMI_USE_NOWLIST02WAR_SELECT02", new Dictionary<string, object?>()
                {
                    {"DATE", DateTime.Today.ToString("yyyy-MM-dd")},
                    {"COM_ID", USER_ID},

                });

            return datatable;
        }
        private async Task<DataTable?> DBSearchWaterFR()
        {
            var dwTable = await DBSearchFRDW();
            var fwTable = await DBSearchFRFW();

            return MergeWaterTables(dwTable, fwTable);
        }
        private async Task<DataTable?> DBSearchFRDW()
        {
            if (QueryService == null)
                return null;

            var datatable = await QueryService.ExecuteDatatableAsync("P_HMI_USE_NOWLIST02WAR_FR_SELECT01", new Dictionary<string, object?>()
                {
                    {"DATE", DateTime.Today.ToString("yyyy-MM-dd")},
                    {"COM_ID", USER_ID},
                    {"GAUGE", "DW"},

                });

            return datatable;
        }
        private async Task<DataTable?> DBSearchFRFW()
        {
            if (QueryService == null)
                return null;

            var datatable = await QueryService.ExecuteDatatableAsync("P_HMI_USE_NOWLIST02WAR_FR_SELECT01", new Dictionary<string, object?>()
                {
                    {"DATE", DateTime.Today.ToString("yyyy-MM-dd")},
                    {"COM_ID", USER_ID},
                    {"GAUGE", "FW"},

                });

            return datatable;
        }
        #endregion [ 데이터 정의 메소드 ]

        #region [데이터 테이블 가공 메소드]
        private DataTable? MergeWaterTables(DataTable? dwTable, DataTable? fwTable)
        {
            if (dwTable == null && fwTable == null)
                return new DataTable();

            DataTable waterTable = new DataTable();

            if (dwTable != null && dwTable.Columns.Count > 0)
            {
                waterTable = dwTable.Clone();
            }
            else if (fwTable != null && fwTable.Columns.Count > 0)
            {
                waterTable = fwTable.Clone();
            }

            waterTable.Columns.Add("Type", typeof(string));

            if (dwTable != null)
            {
                foreach (DataRow row in dwTable.Rows)
                {
                    DataRow newRow = waterTable.NewRow();
                    foreach (DataColumn col in dwTable.Columns)
                    {
                        newRow[col.ColumnName] = row[col];
                    }
                    newRow["Type"] = "순수";
                    waterTable.Rows.Add(newRow);
                }
            }

            if (fwTable != null)
            {
                foreach (DataRow row in fwTable.Rows)
                {
                    DataRow newRow = waterTable.NewRow();
                    foreach (DataColumn col in fwTable.Columns)
                    {
                        newRow[col.ColumnName] = row[col];
                    }
                    newRow["Type"] = "여과수";
                    waterTable.Rows.Add(newRow);
                }
            }

            return waterTable;
        }

        protected DataTable TransformDataTableSteamReal(DataTable? datatable)
        {
            DataTable newTable = new DataTable();
            newTable.Columns.Add("MeasurementType", typeof(string));

            // 입력 dt가 null이거나 행이 없는 경우, 빈 테이블 반환
            if (datatable == null || datatable.Rows.Count == 0)
            {
                return newTable;
            }

            List<string> groups = new List<string>();


            foreach (DataColumn col in datatable.Columns)
            {
                if (col.ColumnName.Length >= 2)
                {
                    string group = col.ColumnName.Substring(0, col.ColumnName.Length - 2);
                    if (!groups.Contains(group))
                        groups.Add(group);
                }
            }
            groups.Sort();

            foreach (string group in groups)
            {
                newTable.Columns.Add(group, typeof(string));
            }

            string[] measurementTypes = new string[] { "유량(T/H)", "온도(°C)", "압력(kgf/cm²)" };
            string[] suffixes = new string[] { "01", "02", "03" };

            for (int i = 0; i < measurementTypes.Length; i++)
            {
                DataRow newRow = newTable.NewRow();
                newRow["MeasurementType"] = measurementTypes[i];
                foreach (string group in groups)
                {
                    string inputColumnName = group + suffixes[i];
                    if (datatable.Columns.Contains(inputColumnName))
                    {
                        newRow[group] = datatable.Rows[0][inputColumnName];
                    }
                    else
                    {
                        newRow[group] = "";
                    }
                }
                newTable.Rows.Add(newRow);
            }
            return newTable;
        }

        protected DataTable TransformDataTableElecReal(DataTable? dt)
        {
            DataTable newTable = new DataTable();
            newTable.Columns.Add("Types", typeof(string));
            newTable.Columns.Add("Usages", typeof(string));

            // 입력 dt가 null이거나 행이 없는 경우, 빈 테이블 반환
            if (dt == null || dt.Rows.Count == 0)
            {
                return newTable;
            }

            string[] Types = new string[] { "실시간 사용량", "일간 사용량", "당월 Peak" };
            string[] columnNames = new string[] { "S01", "S02", "S03" };

            for (int i = 0; i < Types.Length; i++)
            {
                DataRow newRow = newTable.NewRow();
                string inputColumnName = columnNames[i];

                newRow["Types"] = Types[i];
                newRow["Usages"] = dt.Rows[0][inputColumnName];

                if (dt.Columns.Contains(inputColumnName))
                {
                    newRow["Usages"] = dt.Rows[0][inputColumnName];
                }
                else
                {
                    newRow["Usages"] = "";
                }
                newTable.Rows.Add(newRow);
            }

            return newTable;
        }
        #endregion [데이터 테이블 가공 메소드]
    }

}
