namespace MIT.DataUtil.Common
{
    public static class CommonUtilityExtensions
    {
        /// <summary>
        ///  object 값 및 null string 데이터로 변환 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="nullValue"></param>
        /// <returns></returns>
        public static string ToStringTrim(this object? value, string nullValue = "") 
        {
            if (value == null || value == DBNull.Value)
                return nullValue;
            var s = value.ToString();

            return s == null ? nullValue : s.Trim();
        }

        /// <summary>
        /// object 값 및 null Decimal 데이터로 변환 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="nullValue"></param>
        /// <returns></returns>
        public static decimal ToDecimal(this object? value, decimal nullValue = 0)
        {
            decimal d;

            if (decimal.TryParse(value.ToStringTrim(), out d))
            {
                return d;
            }

            return nullValue;
        }

        /// <summary>
        /// object 값 및 null DateTime 데이터로 변환 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this object? value)
        {
            DateTime dte;

            if (DateTime.TryParse(value.ToStringTrim(), out dte))
            { 
                return dte;
            }
            return DateTime.MinValue;
        }



    public static TimeSpan ToTime(this object? value) {
      TimeSpan dte;


      string result = "";
      result = ((value + "").Length == 8) ? value+"" : value + ":00";


      if (TimeSpan.TryParse(result.ToStringTrim(), out dte)) {
        return dte;
      }
      return TimeSpan.MinValue;
    }




    /// <summary>
    /// object 값 및 null Int 데이터로 변환 
    /// </summary>
    /// <param name="value"></param>
    /// <param name="nullValue"></param>
    /// <returns></returns>
    public static int ToInt(this object? value, int nullValue = 0)
        {
            int iResult;
            if (!int.TryParse(value.ToStringTrim(), out iResult))
                return nullValue;

            return iResult;
        }

        /// <summary>
        /// object 값 및 null 체크
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsStringEmpty(this object? value)
        {
            if (value == null || value == DBNull.Value)
                return true;

            return string.IsNullOrEmpty(value.ToString());
        }
    }
}
