using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medit.Framework.DatabaseAccess
{
    /// <summary>
    /// 数据库访问对象接口
    /// </summary>
    internal interface IDAO
    {
        ParameterList ParameterList { get; set; }
        void SetDbConnStr(string connStr);
        int ExecuteNonQuery(string sql, CommandType cmdType = CommandType.Text);
        object ExecuteScalar(string sql, CommandType cmdType = CommandType.Text);
        DataSet GetDataSet(string sql, CommandType cmdType = CommandType.Text, SchemaType schemaType = SchemaType.Source);
        DbDataReader GetDataReader(string sql, CommandType cmdType = CommandType.Text);
    }
}
