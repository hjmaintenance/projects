using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace MIT.WebService.Common
{
    public class SuccessJsonResult : JsonResult
    {
        public SuccessJsonResult(object? value = null) : base(value)
        {
            StatusCode = StatusCodes.Status200OK;
            var options = new JsonSerializerOptions();
            options.Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.HangulSyllables);
        }

        public SuccessJsonResult(object? value, object? serializerSettings) : base(value, serializerSettings)
        {
            StatusCode = StatusCodes.Status200OK;
        }
    }
}
