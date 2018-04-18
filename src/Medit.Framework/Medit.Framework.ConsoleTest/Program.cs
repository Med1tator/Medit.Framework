using Medit.Framework.Abstractions;
using Medit.Framework.Attributes;
using Medit.Framework.Builders;
using Medit.Framework.DatabaseAccess;
using Medit.Framework.Extensions;
using Medit.Framework.Mappers;
using Medit.Framework.Serializer;
using Medit.Framework.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Medit.Framework.ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            #region 20180414
            //bool isuse = FileUtility.IsFileInUse(@"D:\工作\产品客户\02-无限极\消息集成\实施\实施文件\2017-02实施说明.txt");
            //DataTable dt333 = new DAL().GetData();

            //bool iss = "丁鹏".IsStartWithLower();
            //bool iss2 = "丁鹏".IsStartWithUpper();
            //string tbName = EntityUtility.GetTableName(typeof(Person));
            //string colName = EntityUtility.GetColumnName(typeof(Person).GetProperty("Name"));

            //string colL = EntityUtility.GetColumnName(typeof(Person).GetProperty("Name"));
            //List<string> pkeyList = EntityUtility.GetPrimaryKeyNameList(typeof(Person));

            //bool isPKey1 = EntityUtility.IsPrimaryKey(typeof(Person).GetProperty("Name"));
            //bool isPKey2 = EntityUtility.IsPrimaryKey(typeof(Person).GetProperty("Address"));
            //List<Person> pList = new List<Person>
            //{
            //   //new Person{ Name="dp",Age=25,Address="江西高安"},
            //   new Person{ Name="hm",Age=22},
            //   new Person{Name="ldg",Age=25}
            //};

            //DataTable dt = EntityUtility.FillDataTable(pList, true);

            //DataTable dt2 = new DataTable();
            //DataColumn dc1 = new DataColumn("姓名", typeof(string));
            //DataColumn dc2 = new DataColumn("年龄", typeof(string));
            //DataColumn dc3 = new DataColumn("Address", typeof(string));
            //DataColumn dc4 = new DataColumn("性别", typeof(string));
            //dt2.Columns.Add(dc1); dt2.Columns.Add(dc2); dt2.Columns.Add(dc3); dt2.Columns.Add(dc4);
            //DataRow dr1 = dt2.NewRow(); dr1["姓名"] = "dp"; dr1["年龄"] = "25"; dr1["Address"] = "2"; dr1["性别"] = "1";
            //DataRow dr2 = dt2.NewRow(); dr2["姓名"] = "dp"; dr2["年龄"] = "25"; dr2["性别"] = 0;
            //DataRow dr3 = dt2.NewRow(); dr3["姓名"] = "dp"; dr3["年龄"] = "25"; dr3["Address"] = null;
            //dt2.Rows.Add(dr1); dt2.Rows.Add(dr2); dt2.Rows.Add(dr3); 

            //List<Person> pList2 = EntityUtility.LoadDataTable<Person>(dt2, true);
            #endregion

            #region 20180414
            //Student s = new Student() { Age = 20, Id = 1, Name = "Emrys", Address = new Address { Princ = "江西", City = "高安" } };

            //Student s2 = EntityMapper<Student, Student>.Map(s);

            //Stopwatch sw = new Stopwatch();
            //sw.Start();
            //for (int i = 0; i < 1000000; i++)
            //{
            //    StudentSecond ss = EntityMapper<Student, StudentSecond>.Map(s);
            //}
            ////
            ////for (int i = 0; i < 1000000; i++)
            ////{
            ////    StudentSecond ss = MapUtility<Student, StudentSecond>.ExpressionMap<Student, StudentSecond>(s);
            ////}
            //sw.Stop();
            //Console.WriteLine(sw.ElapsedMilliseconds);

            ////Expression<Func<Student, StudentSecond>> ss = (x) => new StudentSecond { Age = x.Age, Id = x.Id, Name = x.Name };
            ////var f = ss.Compile(); 
            //StudentSecond studentSecond = f(s);
            #endregion
            #region MyRegion
            //string tbName1 = EntityUtility.GetTableName<Person>();
            //string create = EntityUtility.GetCreateTableSql<Person>();

            //string xml = XMLSerializer.Serialize(new Student() { Age = 20, Id = 1, Name = "Emrys", BirthDate=DateTime.Now, Address = new Address { Princ = "江西", City = "高安" } });
            //Student s = XMLSerializer.Deserialize<Student>(xml);

            //string json = JSONSerializer.Serialize(new Student() { Age = 20, Id = 1, Name = "Emrys", BirthDate = DateTime.Now, Address = new Address { Princ = "江西", City = "高安" } });
            //Student s1 = XMLSerializer.Deserialize<Student>(json); 
            #endregion


        }
    }

    //[Table( name="人员")]
    [Table(Name = "人员")]
    class Person : IEntity
    {
        [Column(Name = "姓名", Length = 10)]
        [PrimaryKey]
        public string Name { get; set; }

        [Column("年龄", Length = 200)]
        [PrimaryKey]
        public int Age { get; set; }

        [Column("住址")]
        public int Address { get; set; }

        [Column("性别")]
        [Ignore]
        public Gender Gender { get; set; }
    }

    enum Gender
    {
        Male = 1,
        Female = 2
    }

    class DAL : DAO
    {
        public DataTable GetData()
        {
            //string sql = "SELECT * FROM Customer where customerId=@customerId";
            string sql = SqlBuilder.GetSelect("*", "Customer", "customerId=@customerId");
            ParameterList.Clear();
            ParameterList.Add("@customerId", "1");
            ParameterList.Add("@customerName", "34t");
            DataTable dt1 = GetDataSet(sql).Tables[0];
            SetDbConnStr("uid=sa;password=dp;data source=.;initial catalog=Temp");

            ParameterList.Clear();
            ParameterList.Add("@id", "1");
            DataTable dt2 = GetDataSet("Select * from person where id=@id").Tables[0];

            return dt2;
        }
    }

    public class Student : ICloneable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }

        public DateTime BirthDate { get; set; }
        public Address Address { get; set; }

        public object Clone()
        {
            return new Student { Id = this.Id, Name = Name, Age = Age, Address = Address };
        }
    }

    public class StudentSecond
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }

        public string Addr { get; set; }
    }

}
