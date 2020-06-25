using System;
using System.Data;

namespace Map.DataAccess.Dapper
{
    public class DapperFieldInfoAttribute : Attribute
    {
        public string FieldName { get; set; }

        public int Size { get; set; }

        public bool IsOutput { get; set; }

        public bool IsPrimaryKey { get; set; }

        public DbType DataType { get; set; }

        public DapperFieldInfoAttribute()
        {

        }

        public DapperFieldInfoAttribute(string fieldName, bool isOutput = false, int size = 0)
        {
            FieldName = fieldName;
            IsOutput = isOutput;
            Size = size;
        }
    }
}