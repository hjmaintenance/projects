using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using MIT.ServiceModel;
using System;
using MIT.WebService.Common;
using MIT.WebService.Services;
using MIT.WebService.Services.Database;

namespace MIT.WebService.Controllers
{
    /// <summary>
    /// 유저 관련 컨트롤러
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        { 
            this._accountService = accountService;
        }

        [HttpPost]
        [Route("CheckID")]
        public async Task<QueryResponse> CheckID(QueryRequest request)
        {
#if DEBUG
      Console.WriteLine("api CheckID..");
#endif
      if (request == null)
            {
                return new ErrorQueryResponse("QueryRequest null Failed.");
            }

            var response = await _accountService.CheckID(request);

            return response;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<QueryResponse> Register(QueryRequest request)
        {
#if DEBUG
            Console.WriteLine("api register..");
#endif      
            if (request == null)
            {
                return new ErrorQueryResponse("QueryRequest null Failed.");
            }

            var response = await _accountService.Register(request);

            return response;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<QueryResponse> Login(QueryRequest request)
        {
            if (request == null)
            {
                return new ErrorQueryResponse("QueryRequest null Failed.");
            }
#if DEBUG
            Console.WriteLine("api login..");
#endif   
            var response = await _accountService.Login(request, HttpContext);

            return response;
        }

        [HttpPost]
        [Route("RefreshToken")]
        public async Task<QueryResponse> RefreshToken(QueryRequest request)
        {
            if (request == null)
            {
                return new ErrorQueryResponse("QueryRequest null Failed.");
            }

            var response = await _accountService.RefreshToken(request, HttpContext);

            return response;
        }
    }
}
