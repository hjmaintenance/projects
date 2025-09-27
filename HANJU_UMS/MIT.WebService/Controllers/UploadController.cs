using DevExpress.Charts.Native;
using DevExpress.CodeParser;
using DevExpress.DataProcessing;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MIT.Razor.Pages.Service;
using MIT.ServiceModel;
using MIT.WebService.Services.Database;
using System.Data;
using System.Net;
using System.Threading.Tasks;

namespace MIT.WebService.Controllers;
[Route("api/[controller]")]
[ApiController]
public class UploadController : CommonControllerBase {


  public UploadController(MSSQLDatabaseService mssqlDatabaseService, IHttpContextAccessor httpContextAccessor)
        : base(mssqlDatabaseService, httpContextAccessor) {
  }




  [HttpPost("[action]")]
  public async Task<ActionResult> Upload(IFormFile file) {
    var uploadResult = new UploadResult();
    bool isError = false;
    string file_gid = Request.Query["fid"].ToString();
    try {
      // Write code that saves the 'myFile' file.
      // Don't rely on or trust the FileName property without validation.

      //long maxFileSize = 1024 * 15;
      var resourcePath = new Uri($"{Request.Scheme}://{Request.Host}/");

      string trustedFileNameForFileStorage;
      var untrustedFileName = file.FileName;
      uploadResult.FileName = untrustedFileName;
      var trustedFileNameForDisplay =          WebUtility.HtmlEncode(untrustedFileName);

        if (file.Length == 0) {
          uploadResult.ErrorCode = 1;
        uploadResult.ErrorMessage = "FileSize is Zero!";
      }
        //else if (file.Length > maxFileSize) {
        //  uploadResult.ErrorCode = 2;
        //}
        else {
          try {
          var real_name = file_gid + "_" + untrustedFileName;// Path.GetRandomFileName();
          trustedFileNameForFileStorage = Path.GetRandomFileName();
          var path = Path.Combine("c:\\uploads", trustedFileNameForFileStorage);
          var path2 = Path.Combine("c:\\uploads", real_name);
          uploadResult.FilePath = path;

          await using FileStream fs = new(path, FileMode.Create);
          await file.CopyToAsync(fs);

          uploadResult.Uploaded = true;
            uploadResult.StoredFileName = trustedFileNameForFileStorage;

          await using FileStream fs2 = new(path2, FileMode.Create);
          await file.CopyToAsync(fs2);

        }
          catch (IOException ex) {
            uploadResult.ErrorCode = 3;
          uploadResult.ErrorMessage = ex.Message;
          isError = true;
          }
        }

    }
    catch {
      return BadRequest();
    }

    if (isError) {
      return BadRequest();
    }
    else {

      QueryRequest req = new QueryRequest();
      req.QueryName = "SP_File_upload";
      req.QueryParameters = new QueryParameters();
      req.QueryParameters.Add(new QueryParameter() { ParameterName = "FILE_GID", ParameterValue = file_gid, Prefix = "IN_" });
      req.QueryParameters.Add(new QueryParameter() { ParameterName = "FILE_ID", ParameterValue = DateTime.Now.Ticks.ToString() + "_I", Prefix = "IN_" });
      req.QueryParameters.Add(new QueryParameter() { ParameterName = "FILE_NAME", ParameterValue = uploadResult.FileName, Prefix = "IN_" });
      req.QueryParameters.Add(new QueryParameter() { ParameterName = "FILE_SIZE", ParameterValue = file.Length.ToString(), Prefix = "IN_" });
      req.QueryParameters.Add(new QueryParameter() { ParameterName = "FILE_REAL_NAME", ParameterValue = uploadResult.StoredFileName, Prefix = "IN_" });
      req.QueryParameters.Add(new QueryParameter() { ParameterName = "PROG", ParameterValue = "", Prefix = "IN_" });

      QueryResponse res = await _mssqlDatabaseService.ExecuteAsync(req);

      return Ok(uploadResult);
    }
  }


  [HttpGet("[action]")]
  public async Task<ActionResult> Download([FromQuery] string fgid, string fid) {


    // 파일 경로를 찾기 위해 데이터베이스에서 파일 정보를 조회합니다.
    QueryRequest req = new QueryRequest {
      QueryName = "SP_GetFileByFid",
      QueryParameters = new QueryParameters {
            new QueryParameter { ParameterName = "FILE_GID", ParameterValue = fgid, Prefix = "IN_" },
            new QueryParameter { ParameterName = "FILE_ID", ParameterValue = fid, Prefix = "IN_" }
        }
    };

    QueryResponse res = await _mssqlDatabaseService.ExecuteAsync(req);

    if (!res.IsSuccess || res.DataSet.Tables.Count == 0 || res.DataSet.Tables[0].Rows.Count == 0) {
      return NotFound("File not found.");
    }

    DataRow fileRow = res.DataSet.Tables[0].Rows[0];
    string filePath = "c:/uploads/"+fileRow["FILE_REAL_NAME"].ToString();
    string fileName = fileRow["FILE_NAME"].ToString();

    if (string.IsNullOrEmpty(filePath) || !System.IO.File.Exists(filePath)) {
      return NotFound("File not found.");
    }

    var memory = new MemoryStream();
    using (var stream = new FileStream(filePath, FileMode.Open)) {
      await stream.CopyToAsync(memory);
    }
    memory.Position = 0;

    return File(memory, GetContentType(fileName), fileName);



  }


  private string GetContentType(string path) {
    var types = GetMimeTypes();
    var ext = Path.GetExtension(path).ToLowerInvariant();
    return types[ext];
  }



  private Dictionary<string, string> GetMimeTypes() {
    return new Dictionary<string, string> {
        { ".txt", "text/plain" },
        { ".pdf", "application/pdf" },
        { ".doc", "application/vnd.ms-word" },
        { ".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document" },
        { ".xls", "application/vnd.ms-excel" },
        { ".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" },
        { ".png", "image/png" },
        { ".jpg", "image/jpeg" },
        { ".jpeg", "image/jpeg" },
        { ".gif", "image/gif" },
        { ".csv", "text/csv" }
    };
  }




}