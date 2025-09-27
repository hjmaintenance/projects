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
    public interface IConfigurationService
    {
        object? Get(string key);
        void Set(string key, object value);
    }

    /// <summary>
    /// 브라우저 Session Storage에 데이터 저장 및 가져오기 클래스 서비스
    /// </summary>
    public class ConfigurationService : IConfigurationService
    {
        private Dictionary<string, object> _configDic = new Dictionary<string, object>();

        public ConfigurationService() 
        {
        }

        public object? Get(string key)
        {
            return _configDic.ContainsKey(key) ? _configDic[key] : null;
        }

        public void Set(string key, object value)
        {
            if(!_configDic.ContainsKey(key))
                _configDic.Add(key, value);
            else
                _configDic[key] = value;
        }
    }
}
