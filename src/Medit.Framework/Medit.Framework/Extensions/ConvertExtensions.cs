using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medit.Framework.Extensions
{
    public static class ConvertExtensions
    {
        public static T ConvertTo<T>(object input)
        {
            object obj = default(T);
            T result;
            if (input == null || input == DBNull.Value)
            {
                result = (T)((object)obj);
            }
            else
            {
                if (typeof(T) == typeof(int))
                {
                    obj = Convert.ToInt32(input);
                }
                else if (typeof(T) == typeof(long))
                {
                    obj = Convert.ToInt64(input);
                }
                else if (typeof(T) == typeof(string))
                {
                    obj = Convert.ToString(input);
                }
                else if (typeof(T) == typeof(bool))
                {
                    obj = Convert.ToBoolean(input);
                }
                else if (typeof(T) == typeof(double))
                {
                    obj = Convert.ToDouble(input);
                }
                else if (typeof(T) == typeof(DateTime))
                {
                    obj = Convert.ToDateTime(input);
                }
                result = (T)obj;
            }
            return result;
        }
    }
}
