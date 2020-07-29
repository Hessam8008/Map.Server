// ***********************************************************************
// Assembly         : Map.DataAccess
// Author           : U12178
// Created          : 07-28-2020
//
// Last Modified By : U12178
// Last Modified On : 07-28-2020
// ***********************************************************************
// <copyright file="DapperExtensions.cs" company="Golriz">
//     Copyright (c) 2020 Golriz,Inc. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Map.DataAccess.Dapper
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Linq;
    using System.Reflection;

    using global::Dapper;

    /// <summary>
    /// Class DapperExtensions.
    /// </summary>
    public static class DapperExtensions
    {
        /// <summary>
        /// Converts to data table.
        /// </summary>
        /// <param name="list">The i list.</param>
        /// <returns>returns <exception cref="DataTable">Data table</exception>.</returns>
        public static DataTable ToDataTable(this List<int> list)
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add("ID", typeof(int));
            
            foreach (var i in list)
            {
                dataTable.Rows.Add(i);
            }

            return dataTable;
        }

        /// <summary>
        /// Converts to data table.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="list">The list.</param>
        /// <returns>returns <exception cref="DataTable">Data table</exception>.</returns>
        public static DataTable ToDataTable<T>(this List<T> list)
        {
            var dataTable = new DataTable();
            var propertyDescriptorCollection =
                TypeDescriptor.GetProperties(typeof(T));
            for (var i = 0; i < propertyDescriptorCollection.Count; i++)
            {
                var propertyDescriptor = propertyDescriptorCollection[i];
                var type = propertyDescriptor.PropertyType;

                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    type = Nullable.GetUnderlyingType(type);
                }

                if (type != null)
                {
                    dataTable.Columns.Add(propertyDescriptor.Name, type);
                }
            }

            var values = new object[propertyDescriptorCollection.Count];
            foreach (var obj in list)
            {
                for (var i = 0; i < values.Length; i++)
                {
                    values[i] = propertyDescriptorCollection[i].GetValue(obj);
                }

                dataTable.Rows.Add(values);
            }

            return dataTable;
        }

        /// <summary>
        /// This extension converts an enumerable set to a Dapper TVP
        /// </summary>
        /// <typeparam name="T">type of enumerable</typeparam>
        /// <param name="enumerable">list of values</param>
        /// <param name="typeName">database type name</param>
        /// <param name="orderedColumnNames">if more than one column in a TVP,
        /// columns order must match order of columns in TVP</param>
        /// <returns>a custom query parameter</returns>
        /// <exception cref="ArgumentException">Ordered list of column names must be provided when TVP contains more than one column</exception>
        public static SqlMapper.ICustomQueryParameter AsTableValuedParameter<T>(
            this IEnumerable<T> enumerable,
            string typeName,
            IEnumerable<string> orderedColumnNames = null)
        {
            var dataTable = new DataTable();
            var fullName = typeof(T).FullName;
            if (fullName != null && (typeof(T).IsValueType || fullName.Equals("System.String")))
            {
                dataTable.Columns.Add(orderedColumnNames == null ? "noname" : orderedColumnNames.First(), typeof(T));
                foreach (var obj in enumerable)
                {
                    dataTable.Rows.Add(obj);
                }
            }
            else
            {
                var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                var readableProperties = properties.Where(w => w.CanRead).ToArray();

                if (readableProperties.Length > 1 && orderedColumnNames == null)
                {
                    throw new ArgumentException(
                        "Ordered list of column names must be provided when TVP contains more than one column");
                }

                var columnNames = (orderedColumnNames ??
                                   readableProperties.Select(s => s.Name)).ToArray();
                foreach (var name in columnNames)
                {
                    dataTable.Columns.Add(name, readableProperties.Single(s => s.Name.Equals(name)).PropertyType);
                }

                foreach (var obj in enumerable)
                {
                    dataTable.Rows.Add(
                        columnNames.Select(s => readableProperties.Single(s2 => s2.Name.Equals(s)).GetValue(obj))
                            .ToArray());
                }
            }

            return dataTable.AsTableValuedParameter(typeName);
        }

        /// <summary>
        /// Dynamics the parameters.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>returns DynamicParameters.</returns>
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
                {
                    continue;
                }

                var paramName = extraInfoAtt?.FieldName ?? item.Name;
                var isOut = extraInfoAtt?.IsOutput ?? false;
                var size = extraInfoAtt?.Size > 0 ? extraInfoAtt.Size : (int?)null;
                param.Add(
                    paramName,
                    item.GetValue(obj),
                    extraInfoAtt?.DataType,
                    isOut ? ParameterDirection.InputOutput : ParameterDirection.Input,
                    size);
            }

            return param;
        }
    }
}