using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIT.ServiceModel {
  public class UploadResult {
    public bool Uploaded { get; set; }
    public string? FileName { get; set; }
    public string? StoredFileName { get; set; }
    public int ErrorCode { get; set; }
    public string? ErrorMessage { get; set; }
    public string? FilePath { get; set; }
    public string? File_Gid { get; set; }
    public string? File_id { get; set; }
    public string? FileSize { get; set; }
    


  }
}
