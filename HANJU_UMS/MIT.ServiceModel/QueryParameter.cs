using System.Data;

namespace MIT.ServiceModel
{
    /// <summary>
    /// 기본 WebService Parameter Message 클래스
    /// </summary>
    public class QueryParameter
    {
        public string? ParameterName { get; set; }
        public object? ParameterValue { get; set; }
        public ParameterDirection? ParameterDirection { get; set; }

        public string Prefix { get; set; } = "IN_";

        public QueryParameter() { }

        public QueryParameter(string name, object? value, string prefix = "IN_", ParameterDirection dir = System.Data.ParameterDirection.Input) 
        {
            this.ParameterName = name;
            this.ParameterValue = value;
            this.Prefix = prefix;
            this.ParameterDirection = dir;
        }
    }
}