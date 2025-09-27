using Microsoft.JSInterop;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIT.Razor.Pages.Service
{
    /// <summary>
    /// 브라우저 Session Storage에 데이터 저장 및 가져오기 인터페이스 서비스
    /// </summary>
    public interface ISessionStorageService
    {
        /// <summary>
        /// 데이터 가져오기 비동기 함수
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<T?> GetItemAsync<T>(string name);
        /// <summary>
        /// 데이터 저장 비동기 함수
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task SetItemAsync(string name, object value);
        /// <summary>
        /// 데이터 삭제 비동기 함수
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task RemoveItemAsync(string name);
    }

    /// <summary>
    /// 브라우저 Session Storage에 데이터 저장 및 가져오기 클래스 서비스
    /// </summary>
    public class SessionStorageService : ISessionStorageService
    {
        private readonly IJSRuntime _jsRuntime;

        public SessionStorageService(IJSRuntime jsRuntime) 
        {
            this._jsRuntime = jsRuntime;
        }

        /// <summary>
        /// 데이터 가져오기 비동기 함수
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<T?> GetItemAsync<T>(string name)
        {
      try
      {
        var data = await _jsRuntime.InvokeAsync<string>("sessionStorage.getItem", name);

        if (data == null)
          return default;

        return JsonConvert.DeserializeObject<T>(data);

      }
      catch
      {
        return default;
      }
        }

        /// <summary>
        /// 데이터 저장 비동기 함수
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task SetItemAsync(string name, object value)
        {
            await _jsRuntime.InvokeVoidAsync("sessionStorage.setItem", name, JsonConvert.SerializeObject(value));
        }

        /// <summary>
        /// 데이터 삭제 비동기 함수
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task RemoveItemAsync(string name)
        {
            await _jsRuntime.InvokeVoidAsync("sessionStorage.removeItem", name);
        }
    }
}
