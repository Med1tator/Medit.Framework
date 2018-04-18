using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medit.Framework.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class TableAttribute : Attribute
    {
        public string Name { get; set; }

        public TableAttribute(string name = null)
        {
            this.Name = name;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public sealed class ColumnAttribute : Attribute
    {
        public string Name { get; set; }

        public int Length { get; set; }

        public bool IsAllowNull { get; set; }

        public ColumnAttribute(string name = null, int length = Int32.MaxValue, bool isAllowNull = true)
        {
            this.Name = name;
            this.Length = length;
            this.IsAllowNull = isAllowNull;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public sealed class PrimaryKeyAttribute : Attribute
    {
        public bool AutoIncrement { get; set; } = false;

        public PrimaryKeyAttribute(bool autoIncrement = false)
        {
            this.AutoIncrement = autoIncrement;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public sealed class IndexAttribute : Attribute
    {
        public IndexType Type { get; set; }

        public IndexAttribute(IndexType type = IndexType.UNIQUE_INDEX)
        {
            this.Type = type;
        }
    }
    public enum IndexType
    {
        UNIQUE_INDEX,
        CLUSTERED_INDEX,
        NONCLUSTERED_INDEX
    }

    [AttributeUsage(AttributeTargets.Property)]
    public sealed class IgnoreAttribute : Attribute
    {

    }
}
