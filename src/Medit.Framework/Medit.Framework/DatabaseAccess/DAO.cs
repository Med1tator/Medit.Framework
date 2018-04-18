using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medit.Framework.DatabaseAccess
{
    /// <summary>
    /// 数据库访问对象
    /// </summary>
    public class DAO : IDAO
    {
        #region 属性
        private DbConnection _conn { get; set; }
        public ParameterList ParameterList { get; set; } = new ParameterList();
        #endregion

        #region 构造函数
        public DAO()
        {
            //默认获取配置文件中DBConnStr配置项的值作为数据库连接字符串
            string connStr = ConfigurationManager.AppSettings["DBConnStr"] ?? string.Empty;

            if (!string.IsNullOrEmpty(connStr))
            {
                SetDbConn(connStr);
            }
            else
            {
                throw new Exception("未能获取到数据库连接字符串，请检查相关配置项！");
            }
        }
        public DAO(string connStr)
        {
            if (!string.IsNullOrEmpty(connStr))
            {
                SetDbConn(connStr);
            }
            else
            {
                new DAO();
            }
        }
        #endregion

        #region 公有方法
        public void SetDbConnStr(string connStr)
        {
            SetDbConn(connStr);
        }
        public int ExecuteNonQuery(string sql, CommandType cmdType = CommandType.Text)
        {
            int result;
            try
            {
                using (_conn)
                {
                    using (DbCommand cmd = GetDbCmd(_conn, sql, cmdType))
                    {
                        OpenDbConn();
                        result = cmd.ExecuteNonQuery();
                        CloseConn();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        public object ExecuteScalar(string sql, CommandType cmdType = CommandType.Text)
        {
            object result;
            try
            {
                using (_conn)
                {
                    using (DbCommand cmd = GetDbCmd(_conn, sql, cmdType))
                    {
                        OpenDbConn();
                        result = cmd.ExecuteScalar();
                        CloseConn();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        public DataSet GetDataSet(string sql, CommandType cmdType = CommandType.Text, SchemaType schemaType = SchemaType.Source)
        {
            DataSet result = new DataSet();
            try
            {
                using (_conn)
                {
                    using (DataAdapter dataAdapter = GetDbAdapter(_conn, sql, cmdType))
                    {
                        OpenDbConn();
                        dataAdapter.FillSchema(result, schemaType);
                        dataAdapter.Fill(result);
                        CloseConn();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        public DbDataReader GetDataReader(string sql, CommandType cmdType = CommandType.Text)
        {
            DbDataReader result;
            try
            {
                using (_conn)
                {
                    using (DbCommand cmd = GetDbCmd(_conn, sql, cmdType))
                    {
                        OpenDbConn();
                        result = cmd.ExecuteReader();
                        CloseConn();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        #endregion

        #region 私有方法
        private void SetDbConn(string connStr)
        {
            try
            {
                _conn = new SqlConnection(connStr);
            }
            catch (Exception ex)
            {
                throw new Exception("创建数据库连接发生异常，异常说明：" + ex.Message);
            }
        }

        void OpenDbConn()
        {
            if (_conn.State != ConnectionState.Open)
            {
                _conn.Open();
            }
        }

        void CloseConn()
        {
            if (_conn.State != ConnectionState.Closed)
            {
                _conn.Close();
            }
        }

        private DbCommand GetDbCmd(DbConnection conn, string sql, CommandType cmdType = CommandType.Text)
        {
            SqlCommand sqlCmd = new SqlCommand(sql, (SqlConnection)conn);
            sqlCmd.Parameters.Clear();
            if (ParameterList != null && ParameterList.Count > 0)
            {
                ParameterList.ForEach(param =>
                {
                    sqlCmd.Parameters.Add(new SqlParameter(param.Name, param.Value));
                });
            }
            sqlCmd.CommandType = cmdType;
            return sqlCmd;
        }

        private DataAdapter GetDbAdapter(DbConnection conn, string sql, CommandType cmdType = CommandType.Text)
        {
            return new SqlDataAdapter((SqlCommand)GetDbCmd(conn, sql, cmdType));
        }
        #endregion
    }
}
