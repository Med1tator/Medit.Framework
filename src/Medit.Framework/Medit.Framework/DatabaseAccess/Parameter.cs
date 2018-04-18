using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medit.Framework.DatabaseAccess
{
    public class Parameter
    {
        #region 属性
        /// <summary>
        /// 参数名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 参数值
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// 查询内使用的参数类型
        /// </summary>
        public ParameterDirection Direction { get; set; }

        /// <summary>
        /// 数据提供者对象类型
        /// </summary>
        public DbType DbType { get; set; }

        /// <summary>
        /// 参数长度
        /// </summary>
        public int Size { get; set; }
        #endregion

        #region 构造函数
        public Parameter()
        {
        }

        public Parameter(string paramName, object paramValue)
        {
            this.Name = paramName;
            this.Value = paramValue;
        }

        public Parameter(string paramName, object paramValue, ParameterDirection paramDirection, DbType dbType)
        {
            this.Name = paramName;
            this.Value = paramValue;
            this.Direction = paramDirection;
            this.DbType = dbType;
        }

        public Parameter(string paramName, object paramValue, ParameterDirection paramDirection, DbType dbType, int size)
        {
            this.Name = paramName;
            this.Value = paramValue;
            this.Direction = paramDirection;
            this.DbType = dbType;
            this.Size = size;
        }
        #endregion
    }

    public class ParameterList : List<Parameter>
    {
        public void Add(string paramName, object paramValue)
        {
            Parameter param = new Parameter(paramName, paramValue);
            base.Add(param);
        }
        public void Add(string paramName, object paramValue, ParameterDirection paramDirection, DbType dbType)
        {
            Parameter item = new Parameter(paramName, paramValue, paramDirection, dbType);
            base.Add(item);
        }

        public void AddRange(List<Parameter> paramList)
        {
            base.AddRange(paramList);
        }
    }
}
