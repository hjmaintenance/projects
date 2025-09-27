using MIT.DataAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using MIT.ServiceModel;
using MIT.WebService.Services;
using MIT.WebService.Services.Database;
using System.Diagnostics;
using DevExpress.DataProcessing;
//using DevExpress.Pdf.Native.BouncyCastle.Asn1.Ocsp;

namespace MIT.WebService.Controllers {
  /// <summary>
  /// 쿼리 서비스 컨트롤러
  /// </summary>
  [MIT.WebService.Attributes.Authorize]
  [Route("api/[controller]")]
  [ApiController]
  public class QueryServiceController : CommonControllerBase {
    public QueryServiceController(MSSQLDatabaseService mssqlDatabaseService, IHttpContextAccessor httpContextAccessor)
        : base(mssqlDatabaseService, httpContextAccessor) {
    }

    [HttpPost]
    [Route("ExecuteRequestAsync")]
    public async Task<QueryResponse> ExecuteRequestAsync(QueryRequest request) {


      string ip = UtilService.GetIPAddressX(_IHttpContextAccessor);
      string ip2 = UtilService.GetIPAddress(_IHttpContextAccessor);

      foreach (var p in request.QueryParameters) {
        if (p.Prefix == "IN_") { break; }
        if (p.ParameterName.IndexOf("SS_SOURCE") >= 0) {
          request.QueryParameters.Add(

            new QueryParameter() {
              ParameterName = "SS_SOURCE",
              ParameterValue = p.ParameterValue,
              Prefix = "IN_" }
            
            
            );
          break;
        }
      }

      request.QueryParameters.Add(

            new QueryParameter() {
              ParameterName = "SS_CLIENT_IP",
              ParameterValue = ip,
              Prefix = "IN_"
            }


            );


      request.QueryParameters.Add(

            new QueryParameter() {
              ParameterName = "SS_WEB_IP",
              ParameterValue = ip2,
              Prefix = "IN_"
            }


            );


      if (request == null) {
        return new ErrorQueryResponse("ExecuteRequestAsync QueryRequest null Failed.");
      }

      QueryResponse res;

      try {
        res = await _mssqlDatabaseService.ExecuteAsync(request);



      }
      catch (Exception ex) {
        return new ErrorQueryResponse(ex.Message, ex);
      }


      return res;
    }

    [HttpPost]
    [Route("ExecuteRequestsAsync")]
    public async Task<QueryResponses> ExecuteRequestsAsync(QueryRequests requests) {
      string ip = UtilService.GetIPAddressX(_IHttpContextAccessor);
      string ip2 = UtilService.GetIPAddress(_IHttpContextAccessor);

      if (requests == null) {
        return new ErrorQueryResponses("ExecuteRequestsAsync QueryRequests null Failed.");
      }
      else {

        List<QueryRequest> qrList = requests.QueryResponseList;

        int ccnt = 0;
        if (qrList.Count > 0) {
          //debug_str += qrList[0].QueryName + Environment.NewLine;

          foreach (QueryRequest qr in qrList) {


            QueryParameter qp = new QueryParameter();
            qp.ParameterName = "SS_CLIENT_IP";
            qp.ParameterValue = ip;
            qp.Prefix = "IN_";
            qr.QueryParameters.Add(qp);


            qr.QueryParameters.Add(new QueryParameter() { 
            ParameterName = "SS_WEB_IP",
            ParameterValue = ip2+"..",
            Prefix = "IN_"});
            ccnt++;
          }


        }


      }

      QueryResponses res;

      try {
        res = await _mssqlDatabaseService.ExecuteAsync(requests);
      }
      catch (Exception ex) {
        return new ErrorQueryResponses(ex.Message, ex);
      }
      return res;
    }

  }
}
