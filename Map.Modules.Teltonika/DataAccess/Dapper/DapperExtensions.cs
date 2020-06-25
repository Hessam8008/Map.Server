using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using Dapper;

namespace Map.Modules.Teltonika.DataAccess.Dapper
{
    public static class DapperExtensions
    {
        /// <summary>
        /// This extension converts an enumerable set to a Dapper TVP
        /// </summary>
        /// <typeparam name="T">type of enumerable</typeparam>
        /// <param name="enumerable">list of values</param>
        /// <param name="typeName">database type name</param>
        /// <param name="orderedColumnNames">if more than one column in a TVP, 
        /// columns order must match order of columns in TVP</param>
        /// <returns>a custom query parameter</returns>
        public static SqlMapper.ICustomQueryParameter AsTableValuedParameter<T>
            (this IEnumerable<T> enumerable, string typeName,
            IEnumerable<string> orderedColumnNames = null)
        {
            var dataTable = new DataTable();
            var fullName = typeof(T).FullName;
            if (fullName != null && (typeof(T).IsValueType || fullName.Equals("System.String")))
            {
                dataTable.Columns.Add(orderedColumnNames == null ?
                    "noname" : orderedColumnNames.First(), typeof(T));
                foreach (var obj in enumerable)
                    dataTable.Rows.Add(obj);
            }
            else
            {
                var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                var readableProperties = properties.Where(w => w.CanRead).ToArray();

                if (readableProperties.Length > 1 && orderedColumnNames == null)
                    throw new ArgumentException("Ordered list of column names must be provided when TVP contains more than one column");

                var columnNames = (orderedColumnNames ??
                    readableProperties.Select(s => s.Name)).ToArray();
                foreach (var name in columnNames)
                {
                    dataTable.Columns.Add(name, readableProperties.Single
                        (s => s.Name.Equals(name)).PropertyType);
                }

                foreach (var obj in enumerable)
                {
                    dataTable.Rows.Add(
                        columnNames.Select(s => readableProperties.Single
                            (s2 => s2.Name.Equals(s)).GetValue(obj))
                            .ToArray());
                }
            }
            return dataTable.AsTableValuedParameter(typeName);
        }


        public static DynamicParameters DynamicParameters(this object obj)
        {
            var type = obj.GetType();
            var props = type.GetProperties();
            var param = new DynamicParameters();
            foreach (var item in props)
            {
                var extraInfoAtt = item.GetCustomAttribute<DapperFieldInfoAttribute>();
                var ignoreAtt = item.GetCustomAttribute<DapperIgnoreParameterAttribute>();

                if (ignoreAtt != null)
                    continue;

                var paramName = extraInfoAtt?.FieldName ?? item.Name;
                var isOut = extraInfoAtt?.IsOutput ?? false;
                var size = extraInfoAtt?.Size > 0 ? extraInfoAtt.Size : (int?)null;
                param.Add(paramName,
                    item.GetValue(obj),
                    extraInfoAtt?.DataType,
                    isOut ? ParameterDirection.InputOutput : ParameterDirection.Input,
                    size);
            }
            return param;
        }
    }
}