using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using MIT.ServiceModel;
using System;
using MIT.WebService.Attributes;
using MIT.WebService.Common;
using MIT.WebService.Services;
using MIT.WebService.Services.Database;

namespace MIT.WebService.Controllers
{
    /// <summary>
    /// 공통 컨트롤러 베이스 클래스
    /// </summary>
    public class CommonControllerBase : ControllerBase {
    protected readonly MSSQLDatabaseService _mssqlDatabaseService;
    //protected readonly string _pathBase;
    protected readonly IHttpContextAccessor _IHttpContextAccessor;

    public CommonControllerBase(MSSQLDatabaseService mssqlDatabaseService, IHttpContextAccessor httpContextAccessor)
        { 
            this._mssqlDatabaseService = mssqlDatabaseService;
            this._mssqlDatabaseService.CreateDatabase();

      //_pathBase = HttpContext.Request.PathBase;
      _IHttpContextAccessor = httpContextAccessor;


    }

    }
}
