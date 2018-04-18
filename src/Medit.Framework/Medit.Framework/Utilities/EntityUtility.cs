using Medit.Framework.Abstractions;
using Medit.Framework.Attributes;
using Medit.Framework.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Medit.Framework.Utilities
{
    /// <summary>
    /// 实体工具类
    /// </summary>
    public static class EntityUtility
    {
        #region 公有方法
        /// <summary>
        /// 实体集转DataTable
        /// </summary>
        /// <param name="entityList">实体集</param>
        /// <param name="mapEntityAttr">是否映射实体特性，默认值：false</param>
        /// <returns></returns>
        public static DataTable FillDataTable<T>(List<T> entityList, bool mapEntityAttr = false) where T : new()
        {
            return mapEntityAttr ? FillDataTableMapEntityAttr(entityList) : FillDataTableMapProp(entityList);
        }

        /// <summary>
        /// DataTable转实体集
        /// </summary>
        /// <param name="dataTable">数据表</param>
        /// <returns></returns>
        public static List<T> LoadDataTable<T>(DataTable dt, bool mapEntityAttr = false) where T : new()
        {
            return LoadDataRowArray<T>(dt.AsEnumerable().ToArray(), mapEntityAttr);
        }

        /// <summary>
        /// DataRow[]转实体集
        /// </summary>
        /// <param name="drArr">数据行集</param>
        /// <returns></returns>
        public static List<T> LoadDataRowArray<T>(DataRow[] drArr, bool mapEntityAttr = false) where T : new()
        {
            List<T> result = new List<T>();
            if (drArr != null && drArr.Length > 0)
            {
                foreach (DataRow dr in drArr)
                {
                    result.Add(LoadDataRow<T>(dr, mapEntityAttr));
                }
            }
            else
            {
                result = null;
            }
            return result;
        }

        /// <summary>
        /// DataRow转实体
        /// </summary>
        /// <param name="dr">数据行</param>
        /// <returns></returns>
        public static T LoadDataRow<T>(DataRow dr, bool mapEntityAttr = false) where T : new()
        {
            return mapEntityAttr ? LoadDataRowMapEntityAttr<T>(dr) : LoadDataRowMapProp<T>(dr);
        }

        /// <summary>
        /// 获取实体映射数据库表名
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static string GetTableName<T>() where T : new()
        {
            Type type = typeof(T);
            string result = type.Name;
            TableAttribute tableAttribute = (TableAttribute)type.GetCustomAttribute(typeof(TableAttribute));
            if (tableAttribute != null && !string.IsNullOrEmpty(tableAttribute.Name))
            {
                result = tableAttribute.Name;
            }
            return result;
        }

        /// <summary>
        /// 获取实体映射数据库主键字段集
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<string> GetPrimaryKeyNameList<T>() where T : new()
        {
            Type type = typeof(T);
            List<string> result = new List<string>();
            foreach (PropertyInfo propertyInfo in type.GetProperties())
            {
                PrimaryKeyAttribute primaryKeyAttribute = (PrimaryKeyAttribute)propertyInfo.GetCustomAttribute(typeof(PrimaryKeyAttribute));
                if (primaryKeyAttribute != null)
                {
                    result.Add(GetColumnName(propertyInfo));
                }
            }
            return result;
        }

        /// <summary>
        /// 获取实体映射数据库建表语句
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static string GetCreateTableSql<T>() where T : new()
        {
            Type type = typeof(T);
            string result = string.Empty;
            var tbName = GetTableName<T>();
            var primaryKeyList = GetPrimaryKeyNameList<T>();

            StringBuilder sb = new StringBuilder();
            sb.Append("CREATE TABLE [dbo].[{0}](");
            foreach (var p in type.GetProperties())
            {
                if (p.IsIgnore())
                    continue;

                string colAddition = string.Empty;
                //1.字段名称
                var colName = p.GetColumnName();
                //2.字段类型
                if (p.PropertyType == typeof(string))
                {
                    var length = p.GetColumnLength();

                    var columnAttr = (ColumnAttribute)Attribute.GetCustomAttribute(p, typeof(ColumnAttribute), false);
                    if (columnAttr != null)
                    {

                        if (length >= 8000)
                            colAddition += string.Format("NVARCHAR(MAX)");
                        else
                            colAddition += string.Format("NVARCHAR({0})", length);
                    }
                    else
                        colAddition += "NVARCHAR(MAX)";
                }
                else if (p.PropertyType == typeof(DateTime))
                {
                    colAddition += "DATETIME";
                }
                else if (p.PropertyType == typeof(int))
                {
                    colAddition += "INT";
                }
                else
                {
                    colAddition += "NVARCHAR(MAX)";
                }
                sb.Append(string.Format("[{0}] {1},", colName, colAddition));
            }

            //3.约束
            string primaryKeysql = string.Join(",", GetPrimaryKeyNameList<T>().Select(key => string.Format("[{0}] ASC", key)));
            sb.AppendFormat("PRIMARY KEY CLUSTERED ({0})", primaryKeysql);
            sb.Append("WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]) ON[PRIMARY]");

            result = string.Format(sb.ToString(), tbName);
            return result;
        }

        /// <summary>
        /// 获取实体映射数据库列名
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <returns></returns>
        public static string GetColumnName(this PropertyInfo propertyInfo)
        {
            string result = propertyInfo.Name;
            ColumnAttribute columnAttribute = (ColumnAttribute)propertyInfo.GetCustomAttribute(typeof(ColumnAttribute));
            if (columnAttribute != null && !string.IsNullOrEmpty(columnAttribute.Name))
            {
                result = columnAttribute.Name;
            }
            return result;
        }

        /// <summary>
        /// 获取实体映射数据库字段长度
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <returns></returns>
        public static int GetColumnLength(this PropertyInfo propertyInfo)
        {
            int result = Int32.MaxValue;
            ColumnAttribute columnAttribute = (ColumnAttribute)propertyInfo.GetCustomAttribute(typeof(ColumnAttribute));
            if (columnAttribute != null && !string.IsNullOrEmpty(columnAttribute.Name))
            {
                result = columnAttribute.Length;
            }
            return result;
        }

        /// <summary>
        /// 实体映射数据库字段是否为主键
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <returns></returns>
        public static bool IsPrimaryKey(this PropertyInfo propertyInfo)
        {
            return propertyInfo.IsDefined(typeof(PrimaryKeyAttribute));
        }

        /// <summary>
        /// 属性是否忽略映射数据库字段
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <returns></returns>
        public static bool IsIgnore(this PropertyInfo propertyInfo)
        {
            return propertyInfo.IsDefined(typeof(IgnoreAttribute));
        }
        #endregion

        #region 私有方法
        /// <summary>
        /// 实体集转DataTable，实体的属性名作为列名，类名作为表名
        /// </summary>
        /// <param name="entityList">实体集</param>
        /// <returns></returns>
        static DataTable FillDataTableMapProp<T>(List<T> entityList) where T : new()
        {
            Type entityType = typeof(T);
            DataTable result = new DataTable(entityType.Name);

            if (entityList == null && entityList.Count == 0)
            {
                return result;
            }

            foreach (PropertyInfo prop in entityType.GetProperties())
            {
                result.Columns.Add(new DataColumn(prop.Name, prop.PropertyType));
            }

            foreach (T entity in entityList)
            {
                DataRow dataRow = result.NewRow();
                foreach (PropertyInfo prop in entityType.GetProperties())
                {
                    dataRow[prop.Name] = prop.GetValue(entity, null);
                }
                result.Rows.Add(dataRow);
            }
            return result;
        }

        /// <summary>
        /// 实体集转DataTable，实体的属性的Filed特性Name属性的值作为列名，类的Table特性Name属性的值作为表名
        /// </summary>
        /// <param name="entityList">实体集</param>
        /// <returns></returns>
        static DataTable FillDataTableMapEntityAttr<T>(List<T> entityList)
        {
            Type entityType = typeof(T);
            string tbName = entityType.Name;
            var tableAttr = (TableAttribute)Attribute.GetCustomAttribute(entityType, typeof(TableAttribute), false);
            if (tableAttr != null)
                tbName = tableAttr.Name;
            DataTable result = new DataTable(tbName);

            if (entityList == null && entityList.Count == 0)
            {
                return result;
            }
            Dictionary<PropertyInfo, string> propDic = new Dictionary<PropertyInfo, string>();

            var flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            foreach (PropertyInfo prop in entityType.GetProperties(flags))
            {
                string dcName = prop.Name;
                if (prop.IsDefined(typeof(IgnoreAttribute)))
                    continue;

                var fieldAttr = (ColumnAttribute)Attribute.GetCustomAttribute(prop, typeof(ColumnAttribute), false);
                if (fieldAttr != null)
                    dcName = fieldAttr.Name;
                propDic.Add(prop, dcName);
                result.Columns.Add(new DataColumn(dcName, prop.PropertyType));
            }

            foreach (T entity in entityList)
            {
                DataRow dataRow = result.NewRow();
                foreach (PropertyInfo prop in entityType.GetProperties(flags))
                {
                    dataRow[propDic[prop]] = prop.GetValue(entity, null);
                }
                result.Rows.Add(dataRow);
            }
            return result;
        }

        /// <summary>
        /// DataRow转实体，列名作为实体属性名
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dr"></param>
        /// <returns></returns>
        static T LoadDataRowMapProp<T>(DataRow dr) where T : new()
        {
            T result = new T();
            if (dr == null)
            {
                result = default(T);
            }

            foreach (DataColumn dc in dr.Table.Columns)
            {
                string dcName = dc.ColumnName;
                PropertyInfo prop = result.GetType().GetProperty(dcName);
                if (prop != null && dr[dcName] != DBNull.Value)
                {
                    prop.SetValue(result, dr[dcName], null);
                }
            }
            return result;
        }

        /// <summary>
        /// DataRow转实体，列名映射实体属性的Field特性Name属性的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dr"></param>
        /// <returns></returns>
        static T LoadDataRowMapEntityAttr<T>(DataRow dr) where T : new()
        {
            T result = new T();
            Type entityType = typeof(T);
            if (dr == null)
            {
                result = default(T);
            }
            Dictionary<PropertyInfo, string> propDic = new Dictionary<PropertyInfo, string>();

            foreach (PropertyInfo prop in entityType.GetProperties())
            {
                string dcName = prop.Name;
                if (prop.IsDefined(typeof(IgnoreAttribute)))
                    continue;

                var fieldAttr = (ColumnAttribute)Attribute.GetCustomAttribute(prop, typeof(ColumnAttribute), false);
                if (fieldAttr != null)
                    dcName = fieldAttr.Name;
                propDic.Add(prop, dcName);
            }

            foreach (DataColumn dc in dr.Table.Columns)
            {
                string dcName = dc.ColumnName;
                PropertyInfo prop;
                PropertyInfo map = propDic.FirstOrDefault(p => p.Value == dcName).Key;
                if (map != null && map.Name != null)
                    prop = result.GetType().GetProperty(map.Name);
                else
                    prop = result.GetType().GetProperty(dcName);
                if (prop != null && dr[dcName] != DBNull.Value)
                {
                    if (prop.PropertyType.IsEnum)
                        prop.SetValue(result, Enum.ToObject(prop.PropertyType, Enum.Parse(prop.PropertyType, dr[dcName].ToString(), true)), null);
                    else
                        prop.SetValue(result, Convert.ChangeType(dr[dcName], prop.PropertyType), null);
                }
            }
            return result;
        }
        #endregion
    }
}
