using System;
using System.Data;
using System.Net.Http.Json;
using System.Reflection;
using System.Text;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using MIT.ServiceModel;
using System.Diagnostics;
using Microsoft.AspNetCore.Components;
using MIT.Razor.Pages.Data;
//using MIT.UI.Main.MainFrame;

namespace MIT.Razor.Pages.Service
{
    public interface IQueryService
    {
        Task<QueryResponse<T>?> ExecuteAsync<T>(string pageUri, QueryRequest request, string uriName = "BaseAddress");
        Task<QueryResponse<T>?> ExecuteAsync<T>(QueryRequest request, string uriName = "BaseAddress");
        Task<QueryResponse?> ExecuteAsync(string pageUri, QueryRequest request, string uriName = "BaseAddress");
        Task<QueryResponse?> ExecuteAsync(QueryRequest request, string uriName = "BaseAddress");
        Task<QueryResponses?> ExecuteAsync(string pageUri, QueryRequests request, string uriName = "BaseAddress");
        Task<QueryResponses?> ExecuteAsync(QueryRequests request, string uriName = "BaseAddress");
        Task<QueryResponse<T>?> ExecuteAsync<T>(string pageUri, string queryName, Dictionary<string, object?> parameters, string uriName = "BaseAddress");
        Task<QueryResponse<T>?> ExecuteAsync<T>(string queryName, Dictionary<string, object?> parameters, string uriName = "BaseAddress");
        Task<QueryResponse?> ExecuteAsync(string pageUri, string queryName, Dictionary<string, object?> parameters, string uriName = "BaseAddress");
        Task<QueryResponse?> ExecuteAsync(string queryName, Dictionary<string, object?> parameters, string uriName = "BaseAddress");
        Task<QueryResponse<T>?> ExecuteAsync<T>(string pageUri, string queryName, QueryParameters parameters, string uriName = "BaseAddress");
        Task<QueryResponse<T>?> ExecuteAsync<T>(string queryName, QueryParameters parameters, string uriName = "BaseAddress");
        Task<QueryResponse?> ExecuteAsync(string pageUri, string queryName, QueryParameters parameters, string uriName = "BaseAddress");
        Task<QueryResponse?> ExecuteAsync(string queryName, QueryParameters parameters, string uriName = "BaseAddress");
        Task<List<List<T>>?> ExecuteDatatableListAsync<T>(string pageUri, string queryName, Dictionary<string, object?> parameters, string uriName = "BaseAddress");
        Task<List<List<T>>?> ExecuteDatatableListAsync<T>(string queryName, Dictionary<string, object?> parameters, string uriName = "BaseAddress");
        Task<DataSet?> ExecuteDatatableListAsync(string pageUri, string queryName, Dictionary<string, object?> parameters, string uriName = "BaseAddress");
        Task<DataSet?> ExecuteDatatableListAsync(string queryName, Dictionary<string, object?> parameters, string uriName = "BaseAddress");
        Task<List<List<T>>?> ExecuteDatatableListAsync<T>(string pageUri, string queryName, QueryParameters parameters, string uriName = "BaseAddress");
        Task<List<List<T>>?> ExecuteDatatableListAsync<T>(string queryName, QueryParameters parameters, string uriName = "BaseAddress");
        Task<DataSet?> ExecuteDatatableListAsync(string pageUri, string queryName, QueryParameters parameters, string uriName = "BaseAddress");
        Task<DataSet?> ExecuteDatatableListAsync(string queryName, QueryParameters parameters, string uriName = "BaseAddress");
        Task<List<T>?> ExecuteDatatableAsync<T>(string pageUri, string queryName, Dictionary<string, object?> parameters, string uriName = "BaseAddress");
        Task<List<T>?> ExecuteDatatableAsync<T>(string queryName, Dictionary<string, object?> parameters, string uriName = "BaseAddress");
        Task<DataTable?> ExecuteDatatableAsync(string pageUri, string queryName, Dictionary<string, object?> parameters, string uriName = "BaseAddress");
    Task<DataTable?> ExecuteDatatableAsync(string queryName, Dictionary<string, object?> parameters, string uriName = "BaseAddress");


    Task<DataTable?> ExecuteDatatableAsync_fix(string queryName, Dictionary<string, object?> parameters, string prefix = "IN_", string uriName = "BaseAddress");




    Task<List<T>?> ExecuteDatatableAsync<T>(string pageUri, string queryName, QueryParameters parameters, string uriName = "BaseAddress");
        Task<List<T>?> ExecuteDatatableAsync<T>(string queryName, QueryParameters parameters, string uriName = "BaseAddress");
        Task<DataTable?> ExecuteDatatableAsync(string pageUri, string queryName, QueryParameters parameters, string uriName = "BaseAddress");
        Task<DataTable?> ExecuteDatatableAsync(string queryName, QueryParameters parameters, string uriName = "BaseAddress");
        Task ExecuteNonQuery(string pageUri, string queryName, Dictionary<string, object?> parameters, string uriName = "BaseAddress");
    Task ExecuteNonQuery(string queryName, Dictionary<string, object?> parameters, string uriName = "BaseAddress");

    Task ExecuteNonQuery(string pageUri, string queryName, QueryParameters parameters, string uriName = "BaseAddress");
        Task ExecuteNonQuery(string queryName, QueryParameters parameters, string uriName = "BaseAddress");
        Task ExecuteNonQuery(string pageUri, QueryRequests requests, string uriName = "BaseAddress");
        Task ExecuteNonQuery(QueryRequests requests, string uriName = "BaseAddress");
        Task ExecuteNonQuery(string pageUri, string queryName, DataTable datatable, Dictionary<string, object?>? values = null, string uriName = "BaseAddress");
        Task ExecuteNonQuery(string pageUri, string queryName, string[] columnNames, DataTable datatable, Dictionary<string, object?>? values = null, string uriName = "BaseAddress");
        Task ExecuteNonQuery(string queryName, DataTable datatable, Dictionary<string, object?>? values = null, string uriName = "BaseAddress");


    Task ExecuteNonQuery_fix(string queryName, DataTable datatable, Dictionary<string, object?>? values = null, string prefix = "IN_", string uriName = "BaseAddress");






    Task ExecuteNonQuery(string queryName, string[] columnNames, DataTable datatable, Dictionary<string, object?>? values = null, string uriName = "BaseAddress");
        QueryRequests CreateQueryRequests(string queryName, string[] columnNames, DataTable datatable, Dictionary<string, object?>? values = null);
        Task ExecuteNonQuery<T>(string pageUri, string queryName, T Item, string uriName = "BaseAddress");
        Task ExecuteNonQuery<T>(string queryName, T item, string uriName = "BaseAddress");
        Task ExecuteNonQuery<T>(string pageUri, string queryName, List<T> Items, Dictionary<string, object?>? values = null, string uriName = "BaseAddress");
        Task ExecuteNonQuery<T>(string queryName, List<T> Items, Dictionary<string, object?>? values = null, string uriName = "BaseAddress");

    IHttpService GetHttpService();

    }

    public class QueryService : IQueryService
    {
        private readonly IHttpService _httpService;
    private readonly ISessionStorageService _sessionStorageService;
    private readonly NavigationManager _navigationManager;
    private readonly IConfigurationService _configurationService;
    //private readonly IMainFrameService _mainFrameService;

    public QueryService(IHttpService httpService,
            ISessionStorageService sessionStorageService,
            NavigationManager navigationManager,
            IConfigurationService configurationService
            //IMainFrameService _mainFrameService
      )
        {
            _httpService = httpService;
      this._sessionStorageService = sessionStorageService;
      this._navigationManager = navigationManager;
      this._configurationService = configurationService;
      //this._mainFrameService = _mainFrameService;
    }

    public IHttpService GetHttpService() {
      return _httpService;
    }


        public async Task<QueryResponse<T>?> ExecuteAsync<T>(string pageUri, QueryRequest request, string uriName = "BaseAddress")
        {

      await SetSessionUser(request);

      var response = await _httpService.PostAsync<QueryResponse<T>>(pageUri, request, uriName);

            if (response == null)
                throw new Exception("response null");

            if (!response.IsSuccess)
            {
                throw response.Exception == null ? new Exception("IsSuccess 실패") : response.Exception;
            }

            return response;
        }

        public async Task<QueryResponse<T>?> ExecuteAsync<T>(QueryRequest request, string uriName = "BaseAddress")
        {
            return await ExecuteAsync<T>("QueryService/ExecuteRequestAsync", request, uriName);
        }

    private async Task SetSessionUser( QueryRequest request) {

      try {
        var user = await _sessionStorageService.GetItemAsync<User>("user");

        if (user != null) {
          request.QueryParameters.Add(new QueryParameter() { Prefix= "IN_", ParameterName = "SS_USER_ID", ParameterValue = user.USER_ID });
          request.QueryParameters.Add(new QueryParameter() { Prefix = "IN_", ParameterName = "SS_TOKEN", ParameterValue = user.Token });
          request.QueryParameters.Add(new QueryParameter() { Prefix = "IN_", ParameterName = "SS_ACCESS_TOKEN_ID", ParameterValue = user.ACCESS_TOKEN_ID });
          request.QueryParameters.Add(new QueryParameter() { Prefix = "IN_", ParameterName = "SS_REFRESH_TOKEN", ParameterValue = user.REFRESH_TOKEN });
        }
        else {
          request.QueryParameters.Add(new QueryParameter() { Prefix = "IN_", ParameterName = "SS_USER_ID", ParameterValue = "" });
          request.QueryParameters.Add(new QueryParameter() { Prefix = "IN_", ParameterName = "SS_TOKEN", ParameterValue = "" });
          request.QueryParameters.Add(new QueryParameter() { Prefix = "IN_", ParameterName = "SS_ACCESS_TOKEN_ID", ParameterValue = "" });
          request.QueryParameters.Add(new QueryParameter() { Prefix = "IN_", ParameterName = "SS_REFRESH_TOKEN", ParameterValue = "" });

        }
      }
      catch (Exception ex) {
        throw new Exception("zzzzzzzz : "+ex.Message);
      }
    }

        public async Task<QueryResponse?> ExecuteAsync(string pageUri, QueryRequest request, string uriName = "BaseAddress")
        {

      await SetSessionUser(request);

      var content = new StringContent("your raw text payload", Encoding.UTF8, "text/plain");

      var response = await _httpService.PostAsync<QueryResponse>(pageUri, request, uriName);

            if (response == null)
                throw new Exception("response null");

            if (!response.IsSuccess)
            {
                throw response.Exception == null ? new Exception("IsSuccess 실패") : response.Exception;
            }

            return response;
        }

        public async Task<QueryResponse?> ExecuteAsync(QueryRequest request, string uriName = "BaseAddress")
        {
      return await ExecuteAsync("QueryService/ExecuteRequestAsync", request, uriName);
        }

        public async Task<QueryResponses?> ExecuteAsync(string pageUri, QueryRequests request, string uriName = "BaseAddress") {



      try {
        

        foreach (var qr in request) {

          await SetSessionUser(qr);
        }



      }
      catch (Exception ex) {
        throw new Exception("cccccccccc");
      }



      var responses = await _httpService.PostAsync<QueryResponses>(pageUri, request, uriName);

            if (responses == null)
                throw new Exception("responses null");

            if (!responses.IsSuccessMain)
            {
                throw responses.ExceptionMain == null ? new Exception("IsSuccess 실패") : responses.ExceptionMain;
            }

            return responses;
        }

        public async Task<QueryResponses?> ExecuteAsync(QueryRequests request, string uriName = "BaseAddress")
        {
            return await ExecuteAsync("QueryService/ExecuteRequestsAsync", request, uriName);
        }

        public async Task<QueryResponse<T>?> ExecuteAsync<T>(string pageUri, string queryName, Dictionary<string, object?> parameters, string uriName = "BaseAddress")
        {
            return await ExecuteAsync<T>(pageUri, new QueryRequest(queryName, parameters), uriName);
        }

        public async Task<QueryResponse<T>?> ExecuteAsync<T>(string queryName, Dictionary<string, object?> parameters, string uriName = "BaseAddress")
        {
            return await ExecuteAsync<T>(new QueryRequest(queryName, parameters), uriName);
        }

        public async Task<QueryResponse?> ExecuteAsync(string pageUri, string queryName, Dictionary<string, object?> parameters, string uriName = "BaseAddress")
        {
            return await ExecuteAsync(pageUri, new QueryRequest(queryName, parameters), uriName);
        }

        public async Task<QueryResponse?> ExecuteAsync(string queryName, Dictionary<string, object?> parameters, string uriName = "BaseAddress")
        {
            return await ExecuteAsync(new QueryRequest(queryName, parameters), uriName);
        }

    public async Task<QueryResponse?> ExecuteAsync(string queryName, Dictionary<string, object?> parameters, string prefix ="IN_", string uriName = "BaseAddress") {
      return await ExecuteAsync(new QueryRequest(queryName, parameters, prefix), uriName);
    }

    public async Task<QueryResponse<T>?> ExecuteAsync<T>(string pageUri, string queryName, QueryParameters parameters, string uriName = "BaseAddress")
        {
            return await ExecuteAsync<T>(pageUri, new QueryRequest(queryName, parameters), uriName);
        }

        public async Task<QueryResponse<T>?> ExecuteAsync<T>(string queryName, QueryParameters parameters, string uriName = "BaseAddress")
        {
            return await ExecuteAsync<T>(new QueryRequest(queryName, parameters), uriName);
        }

        public async Task<QueryResponse?> ExecuteAsync(string pageUri, string queryName, QueryParameters parameters, string uriName = "BaseAddress")
        {
            return await ExecuteAsync(pageUri, new QueryRequest(queryName, parameters), uriName);
        }

        public async Task<QueryResponse?> ExecuteAsync(string queryName, QueryParameters parameters, string uriName = "BaseAddress")
        {
            return await ExecuteAsync(new QueryRequest(queryName, parameters), uriName);
        }

        public async Task<List<List<T>>?> ExecuteDatatableListAsync<T>(string pageUri, string queryName, Dictionary<string, object?> parameters, string uriName = "BaseAddress")
        {
            var response = await ExecuteAsync<T>(pageUri, new QueryRequest(queryName, parameters), uriName);

            return response?.datatableList;
        }

        public async Task<List<List<T>>?> ExecuteDatatableListAsync<T>(string queryName, Dictionary<string, object?> parameters, string uriName = "BaseAddress")
        {
            var response = await ExecuteAsync<T>(new QueryRequest(queryName, parameters), uriName);

            return response?.datatableList;
        }

        public async Task<DataSet?> ExecuteDatatableListAsync(string pageUri, string queryName, Dictionary<string, object?> parameters, string uriName = "BaseAddress")
        {
            var response = await ExecuteAsync(pageUri, new QueryRequest(queryName, parameters), uriName);

            return response?.DataSet;
        }

        public async Task<DataSet?> ExecuteDatatableListAsync(string queryName, Dictionary<string, object?> parameters, string uriName = "BaseAddress")
        {
            var response = await ExecuteAsync(new QueryRequest(queryName, parameters), uriName);

            return response?.DataSet;
        }

        public async Task<List<List<T>>?> ExecuteDatatableListAsync<T>(string pageUri, string queryName, QueryParameters parameters, string uriName = "BaseAddress")
        {
            var response = await ExecuteAsync<T>(pageUri, new QueryRequest(queryName, parameters), uriName);

            return response?.datatableList;
        }

        public async Task<List<List<T>>?> ExecuteDatatableListAsync<T>(string queryName, QueryParameters parameters, string uriName = "BaseAddress")
        {
            var response = await ExecuteAsync<T>(new QueryRequest(queryName, parameters), uriName);

            return response?.datatableList;
        }

        public async Task<DataSet?> ExecuteDatatableListAsync(string pageUri, string queryName, QueryParameters parameters, string uriName = "BaseAddress")
        {
            var response = await ExecuteAsync(pageUri, new QueryRequest(queryName, parameters), uriName);

            return response?.DataSet;
        }

        public async Task<DataSet?> ExecuteDatatableListAsync(string queryName, QueryParameters parameters, string uriName = "BaseAddress")
        {
            var response = await ExecuteAsync(new QueryRequest(queryName, parameters), uriName);

            return response?.DataSet;
        }

        public async Task<List<T>?> ExecuteDatatableAsync<T>(string pageUri, string queryName, Dictionary<string, object?> parameters, string uriName = "BaseAddress")
        {
            var response = await ExecuteAsync<T>(pageUri, new QueryRequest(queryName, parameters), uriName);

            return response?.datatableList?[0];
        }

        public async Task<List<T>?> ExecuteDatatableAsync<T>(string queryName, Dictionary<string, object?> parameters, string uriName = "BaseAddress")
        {
            var response = await ExecuteAsync<T>(new QueryRequest(queryName, parameters), uriName);

            return response?.datatableList?[0];
        }

        public async Task<DataTable?> ExecuteDatatableAsync(string pageUri, string queryName, Dictionary<string, object?> parameters, string uriName = "BaseAddress")
        {
            var response = await ExecuteAsync(pageUri, new QueryRequest(queryName, parameters), uriName);

            return response?.DataSet.Tables?[0];
        }

        public async Task<DataTable?> ExecuteDatatableAsync(string queryName, Dictionary<string, object?> parameters, string uriName = "BaseAddress")
        {


            var response = await ExecuteAsync(new QueryRequest(queryName, parameters), uriName);

            return response?.DataSet.Tables?[0];
        }


    public async Task<DataTable?> ExecuteDatatableAsync_fix(string queryName, Dictionary<string, object?> parameters, string prefix = "IN_", string uriName = "BaseAddress")
    {


      var response = await ExecuteAsync(new QueryRequest(queryName, parameters, prefix), uriName);

      return response?.DataSet.Tables?[0];
    }


    public async Task<List<T>?> ExecuteDatatableAsync<T>(string pageUri, string queryName, QueryParameters parameters, string uriName = "BaseAddress")
        {
            var response = await ExecuteAsync<T>(pageUri, new QueryRequest(queryName, parameters), uriName);

            return response?.datatableList?[0];
        }

        public async Task<List<T>?> ExecuteDatatableAsync<T>(string queryName, QueryParameters parameters, string uriName = "BaseAddress")
        {
            var response = await ExecuteAsync<T>(new QueryRequest(queryName, parameters), uriName);

            return response?.datatableList?[0];
        }

        public async Task<DataTable?> ExecuteDatatableAsync(string pageUri, string queryName, QueryParameters parameters, string uriName = "BaseAddress")
        {
            var response = await ExecuteAsync(pageUri, new QueryRequest(queryName, parameters), uriName);

            return response?.DataSet.Tables?[0];
        }

        public async Task<DataTable?> ExecuteDatatableAsync(string queryName, QueryParameters parameters, string uriName = "BaseAddress")
        {
            var response = await ExecuteAsync(new QueryRequest(queryName, parameters), uriName);

            return response?.DataSet.Tables?[0];
        }

        public async Task ExecuteNonQuery<T>(string pageUri, string queryName, T Item, string uriName = "BaseAddress")
        {
            var queryParameters = GetQueryParameters(Item);

            await ExecuteAsync(pageUri, queryName, queryParameters, uriName);
        }

        public async Task ExecuteNonQuery(string pageUri, string queryName, Dictionary<string, object?> parameters, string uriName = "BaseAddress")
        {
            await ExecuteAsync(pageUri, queryName, parameters, uriName);
        }

        public async Task ExecuteNonQuery<T>(string queryName, T item, string uriName = "BaseAddress")
        {
            var queryParameters = GetQueryParameters(item);

            await ExecuteAsync(queryName, queryParameters, uriName);
        }

        public async Task ExecuteNonQuery(string queryName, Dictionary<string, object?> parameters, string uriName = "BaseAddress")
        {
            await ExecuteAsync(queryName, parameters, uriName);
        }




    public async Task ExecuteNonQuery(string pageUri, string queryName, QueryParameters parameters, string uriName = "BaseAddress")
        {
            await ExecuteAsync(pageUri, queryName, parameters, uriName);
        }

        public async Task ExecuteNonQuery(string queryName, QueryParameters parameters, string uriName = "BaseAddress")
        {
            await ExecuteAsync(queryName, parameters, uriName);
        }

        public async Task ExecuteNonQuery(string pageUri, QueryRequests requests, string uriName = "BaseAddress")
        {
            await ExecuteAsync(pageUri, requests, uriName);
        }

        public async Task ExecuteNonQuery(QueryRequests requests, string uriName = "BaseAddress")
        {
            await ExecuteAsync(requests, uriName);
        }

        public async Task ExecuteNonQuery<T>(string pageUri, string queryName, List<T> Items, Dictionary<string, object?>? values = null, string uriName = "BaseAddress")
        {
            var requests = CreateQueryRequests<T>(queryName, Items, values);
            await ExecuteAsync(pageUri, requests, uriName);
        }

        public async Task ExecuteNonQuery<T>(string queryName, List<T> Items, Dictionary<string, object?>? values = null, string uriName = "BaseAddress")
        {
            var requests = CreateQueryRequests<T>(queryName, Items, values);
            await ExecuteAsync(requests, uriName);
        }

        public async Task ExecuteNonQuery(string pageUri, string queryName, DataTable datatable, Dictionary<string, object?>? values = null, string uriName = "BaseAddress")
        {
            string[] columnNames = datatable.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToArray();
            var requests = CreateQueryRequests(queryName, columnNames, datatable, values);
            await ExecuteAsync(pageUri, requests, uriName);
        }

        public async Task ExecuteNonQuery(string pageUri, string queryName, string[] columnNames, DataTable datatable, Dictionary<string, object?>? values = null, string uriName = "BaseAddress")
        {
            var requests = CreateQueryRequests(queryName, columnNames, datatable, values);
            await ExecuteAsync(pageUri, requests, uriName);
        }

        public async Task ExecuteNonQuery(string queryName, DataTable datatable, Dictionary<string, object?>? values = null, string uriName = "BaseAddress")
        {
            string[] columnNames = datatable.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToArray();
            var requests = CreateQueryRequests(queryName, columnNames, datatable, values);
            await ExecuteAsync(requests, uriName);
        }


    public async Task ExecuteNonQuery_fix(string queryName, DataTable datatable, Dictionary<string, object?>? values = null, string prefix = "IN_", string uriName = "BaseAddress") {
      string[] columnNames = datatable.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToArray();
      var requests = CreateQueryRequests_fix(queryName, columnNames, datatable, values, prefix);
      await ExecuteAsync(requests, uriName);
    }


    public async Task ExecuteNonQuery(string queryName, string[] columnNames, DataTable datatable, Dictionary<string, object?>? values = null, string uriName = "BaseAddress")
        {
            var requests = CreateQueryRequests(queryName, columnNames, datatable, values);
            await ExecuteAsync(requests, uriName);
        }

        public QueryRequests CreateQueryRequests(string queryName, string[] columnNames, DataTable datatable, Dictionary<string, object?>? values = null)
        {
            var queryRequests = new QueryRequests();

            if (datatable == null || datatable.Rows.Count == 0)
                return queryRequests;

            foreach (DataRow row in datatable.Rows)
            {
                var queryParameters = new QueryParameters();
                foreach (string columnName in columnNames)
                {
                    queryParameters.Add(columnName, row[columnName]);
                }

                if (values != null)
                {
                    foreach (var value in values)
                    {
                        queryParameters.Add(value.Key, value.Value);
                    }
                }

                queryRequests.Add(new QueryRequest(queryName, queryParameters));
            }

            return queryRequests;
        }

    public QueryRequests CreateQueryRequests_fix(string queryName, string[] columnNames, DataTable datatable, Dictionary<string, object?>? values = null, string prefix = "IN_") {
      var queryRequests = new QueryRequests();



      if (datatable == null || datatable.Rows.Count == 0)
        return queryRequests;

      foreach (DataRow row in datatable.Rows) {
        var queryParameters = new QueryParameters();
        foreach (string columnName in columnNames) {
          queryParameters.Add(columnName, row[columnName], prefix);
        }

        if (values != null) {
          foreach (var value in values) {
            queryParameters.Add(value.Key, value.Value);
            //Debug.WriteLine(prefix + value.Key+" : "+ value.Value);
          }
        }

        queryRequests.Add(new QueryRequest(queryName, queryParameters));
      }

      return queryRequests;
    }

    public QueryRequests CreateQueryRequests<T>(string queryName, List<T> items, Dictionary<string, object?>? values = null)
        {
            var queryRequests = new QueryRequests();

            if (items == null || items.Count == 0)
                return queryRequests;

            foreach (var item in items)
            {
                var queryParameters = GetQueryParameters(item);

                if (values != null)
                {
                    foreach (var value in values)
                    {
                        queryParameters.Add(value.Key, value.Value);
                    }
                }

                queryRequests.Add(new QueryRequest(queryName, queryParameters));
            }

            return queryRequests;
        }

        private QueryParameters GetQueryParameters<T>(T item)
        {
            var queryParameters = new QueryParameters();

            Dictionary<string, object> columns = GetFieldNameValues(item);
            foreach (var col in columns)
            {
                queryParameters.Add(col.Key, col.Value);
            }

            return queryParameters;
        }

        private Dictionary<string, object> GetFieldNameValues<T>(T item)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();

            GetChildFieldNameValues(typeof(T), item, dic);

            return dic;
        }



        private void GetChildFieldNameValues<T>(Type? type, T item, Dictionary<string, object> dic)
        {
            if (type == null)
                return;

            FieldInfo[] fieldInfos = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (FieldInfo fieldInfo in fieldInfos)
            {
                var value = fieldInfo.GetValue(item);

                if (fieldInfo.Name.IndexOf("<") >= 0)
                {
                    var name = fieldInfo.Name.Substring(1, fieldInfo.Name.IndexOf(">") - 1);

                    dic.Add(name, value == null ? DBNull.Value : value);
                }
                else
                {
                    dic.Add(fieldInfo.Name, value == null ? DBNull.Value : value);
                }
            }

            GetChildFieldNameValues(type.BaseType, item, dic);
        }
    }
}