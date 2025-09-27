using MIT.DataUtil.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MIT.DataUtil.Common
{
    public static class DataExtensions
    {

        /// <summary>
        /// DataTable에 체크된 데이터 리턴
        /// </summary>
        /// <param name="datatable"></param>
        /// <param name="checkedColumnName"></param>
        /// <returns></returns>
        public static DataRow[]? GetCheckedRows(this DataTable datatable, string checkedColumnName = "CHK", string value = "Y")
        {
            if (datatable == null || !datatable.Columns.Contains(checkedColumnName))
                return null;

            return datatable.AsEnumerable().Where(w => w[checkedColumnName].ToStringTrim().Equals(value)).ToArray();
        }

        /// <summary>
        /// DataTable에 체크된 데이터 있는 체크
        /// </summary>
        /// <param name="datatable"></param>
        /// <param name="checkedColumnName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsCheckedRows(this DataTable datatable, string checkedColumnName = "CHK", string value = "Y")
        {
            if (datatable == null || !datatable.Columns.Contains(checkedColumnName))
                return false;

            return datatable.AsEnumerable().Any(w => w[checkedColumnName].ToStringTrim().Equals(value));
        }

        
    }
}
