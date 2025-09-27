using System.Data;
using MIT.DataUtil.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MIT.ServiceModel
{
    /// <summary>
    /// DataSet형식 커스텀 Json Convert
    /// </summary>
    public class HttpJsonConverter : Newtonsoft.Json.JsonConverter
    {
        public HttpJsonConverter()
        {
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(DataSet);
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            if (objectType == typeof(DataSet))
            {
                var root = JObject.Load(reader);

                DataSet ds = new DataSet("ds");

                foreach (var table in root)
                {
                    if (table.Value == null)
                        continue;

                    var cols = table.Value["cols"];
                    if (cols == null)
                        continue;

                    DataTable dt = new DataTable(table.Key);

                    foreach (var col in (JObject)cols)
                    {
                        Type? type = col.Value == null ? null : Type.GetType(col.Value.ToString());

                        dt.Columns.Add(col.Key, type == null ? typeof(string) : type);
                    }

                    var data = table.Value["data"];
                    if (data == null)
                        continue;

                    foreach (var values in (JArray)data)
                    {
                        DataRow newRow = dt.NewRow();
                        foreach (DataColumn col in dt.Columns)
                        {
                            var value = values[col.ColumnName];

                            if (value == null)
                                continue;

                            newRow[col] = GetReadData(value, col.DataType);
                        }
                        dt.Rows.Add(newRow);
                    }

                    ds.Tables.Add(dt);
                }



                return ds;
            }

            return null;
        }

        private object GetReadData(object data, Type type)
        {
            if (type == typeof(DateTime))
                return (data == null || data == DBNull.Value || string.IsNullOrEmpty(data.ToString())) ? DBNull.Value : Convert.ToDateTime(data);
            else if (type == typeof(int))
                return (data == null || data == DBNull.Value || string.IsNullOrEmpty(data.ToString())) ? DBNull.Value : data.ToInt();
      else if (type == typeof(decimal))
        return (data == null || data == DBNull.Value || string.IsNullOrEmpty(data.ToString())) ? DBNull.Value : data.ToDecimal();
      else if (type == typeof(Double))
        return (data == null || data == DBNull.Value || string.IsNullOrEmpty(data.ToString())) ? DBNull.Value : data.ToDecimal();

      return data;
        }

    private string? GetWrithData(object data, Type type) {
      if (type == typeof(DateTime)) {
        return data == null || data == DBNull.Value ? string.Empty : ((DateTime)data).ToString("yyyy-MM-dd HH:mm:dd.FFFFFF");
      }
      else if (type == typeof(Double)) {
        return data == null || data == DBNull.Value ? "0" : data.ToString();
      }

      return data?.ToString();
    }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            if (value is DataSet ds)
            {
                JObject obj = new JObject();

                foreach (DataTable dt in ds.Tables)
                {
                    JObject table = new JObject();

                    JObject cols = new JObject();
                    foreach (DataColumn col in dt.Columns)
                    {
                        cols.Add(col.ColumnName, col.DataType.FullName);
                    }

                    table.Add("cols", cols);

                    var dataarray = new JArray();
                    foreach (DataRow row in dt.Rows)
                    {
                        JObject datavalue = new JObject();

                        foreach (DataColumn col in dt.Columns)
                        {
                            datavalue.Add(col.ColumnName, GetWrithData(row[col], col.DataType));
                        }
                        dataarray.Add(datavalue);
                    }
                    table.Add("data", dataarray);

                    obj.Add(dt.TableName, table);
                }

                obj.WriteTo(writer);
            }
        }
    }
}
