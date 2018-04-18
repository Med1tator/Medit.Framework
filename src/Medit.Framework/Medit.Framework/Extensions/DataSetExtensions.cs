
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medit.Framework.Extensions
{
    public static class DataSetExtensions
    {
        public static bool IsNullOrEmpty(this DataSet dataSet)
        {
            return (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0);
        }
        public static bool IsNullOrEmpty(this DataTable dataTable)
        {
            return (dataTable != null && dataTable.Rows.Count > 0);
        }
    }
}
