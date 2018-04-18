using System;
using System.Text;

namespace Medit.Framework.Builders
{
    public class SqlBuilder
    {
        #region 获取【SELECT】Sql语句
        /// <summary>
        /// 获取【SELECT】Sql语句
        /// </summary>
        /// <param name="queryField">查询字段</param>
        /// <param name="tableName">表名</param>
        /// <param name="whereCondition">查询语句条件</param>
        /// <param name="orderCondition">查询结果排序条件</param>
        /// <returns></returns>
        public static string GetSelect(string queryField, string tableName, string whereCondition = null, string orderCondition = null)
        {
            StringBuilder sqlSb = new StringBuilder();
            queryField = string.IsNullOrWhiteSpace(queryField) ? "*" : queryField;
            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new ArgumentNullException(tableName, string.Format("传入参数表名{0}为空无法构造SELECT语句", tableName));
            }
            whereCondition = string.IsNullOrWhiteSpace(whereCondition) ? string.Empty : string.Format("WHERE {0}", whereCondition);
            orderCondition = string.IsNullOrWhiteSpace(orderCondition) ? string.Empty : string.Format("ORDER BY {0}", orderCondition);
            sqlSb.AppendFormat("SELECT {0} FROM {1} {2} {3}", queryField, tableName, whereCondition, orderCondition);
            return sqlSb.ToString();
        }
        #endregion

        #region 获取【DELETE】Sql语句
        /// <summary>
        /// 获取【DELETE】Sql语句
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="whereCondition">删除语句条件，可选参数</param>
        /// <returns></returns>
        public static string GetDelete(string tableName, string whereCondition = null)
        {
            StringBuilder sqlSb = new StringBuilder();
            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new ArgumentNullException(tableName, string.Format("传入参数表名{0}为空无法构造DELETE语句", tableName));
            }
            whereCondition = string.IsNullOrWhiteSpace(whereCondition) ? string.Empty : string.Format("WHERE {0}", whereCondition);
            sqlSb.AppendFormat("DELETE FROM {0} {1}", tableName, whereCondition);
            return sqlSb.ToString();
        }
        #endregion

        #region 获取【UPDATE】Sql语句
        /// <summary>
        /// 获取【UPDATE】Sql语句
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="fieldValueList">字段名-值列表</param>
        /// <param name="whereCondition">更新语句条件，可选参数</param>
        /// <returns></returns>
        public static string GetUpdate(string tableName, string fieldValueList, string whereCondition = null)
        {
            StringBuilder sqlSb = new StringBuilder();
            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new ArgumentNullException(tableName, string.Format("传入参数表名{0}为空无法构造UPDATE语句", tableName));
            }
            if (string.IsNullOrWhiteSpace(fieldValueList))
            {
                throw new ArgumentNullException(fieldValueList, string.Format("传入参数字段名-值列表{0}为空无法构造UPDATE语句", fieldValueList));
            }
            whereCondition = string.IsNullOrWhiteSpace(whereCondition) ? string.Empty : string.Format("WHERE {0}", whereCondition);
            sqlSb.AppendFormat("UPDATE {0} SET {1} {2}", tableName, fieldValueList, whereCondition);
            return sqlSb.ToString();
        }
        #endregion

        #region 获取【INSERT】Sql语句
        /// <summary>
        /// 获取【INSERT】Sql语句
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="fieldNameList">新增字段名列表</param>
        /// <param name="fieldValueList">新增字段值列表</param>
        /// <returns></returns>
        public static string GetInsert(string tableName, string fieldNameList, string fieldValueList)
        {
            StringBuilder sqlSb = new StringBuilder();
            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new ArgumentNullException(tableName, string.Format("传入参数表名{0}为空无法构造INSERT语句", tableName));
            }
            if (string.IsNullOrWhiteSpace(fieldValueList))
            {
                throw new ArgumentNullException(fieldValueList, string.Format("传入参数字段值列表{0}为空无法构造INSERT语句", fieldValueList));
            }
            fieldNameList = string.IsNullOrWhiteSpace(fieldNameList) ? string.Empty : string.Format("({0})", fieldNameList);
            sqlSb.AppendFormat("INSERT INTO {0} {1} VALUES ({2})", tableName, fieldNameList, fieldValueList);
            return sqlSb.ToString();
        }
        #endregion
    }
}
