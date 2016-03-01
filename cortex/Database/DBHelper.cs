using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using NLog;

namespace Cortex.database
{
    public class DbHelper :IDisposable
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private string ConnectionStringKey { get; set; }

        public DbHelper(string strConnectionStringKey = "default")
        {
            ConnectionStringKey = strConnectionStringKey;
            Logger.Trace("Initialised DbHelper using Connection String Key : {0}", strConnectionStringKey);
        }



        private DataSet ExecuteStoredProcedure(string strStoredProcedureName, List<SqlParameter> lstSqlParameters)
        {
            var strConnectionString = ConfigurationManager.ConnectionStrings[ConnectionStringKey].ConnectionString;
            Logger.Trace("Creating new SQL connection with connection string: {0}", strConnectionString);
            using (var conn = new SqlConnection(strConnectionString))
            {
                try
                {
                    using (var dataAdapter = new SqlDataAdapter())
                    {
                        Logger.Trace("Executing SQL Stored Procedure: {0}", strStoredProcedureName);
                        dataAdapter.SelectCommand = new SqlCommand(strStoredProcedureName, conn) { CommandType = CommandType.StoredProcedure };
                        if (lstSqlParameters != null)
                        {
                            foreach (var param in lstSqlParameters)
                            {
                                dataAdapter.SelectCommand.Parameters.Add(param);
                            }
                        }
                        var dataSet = new DataSet();
                        dataAdapter.Fill(dataSet);
                        return dataSet;
                    }
                }
                catch (SqlException ex)
                {
                    Logger.Error("SQL Exception: {0}", ex.Message);
                    return null;
                }
                catch (Exception ex)
                {
                    Logger.Error("Exception: {0}", ex.Message);
                    return null;
                }
            }
        }
        
        public DataSet GetDataSet(string strStoredProcedureName)
        {
            return GetDataSet(strStoredProcedureName, null);
        }
        public DataSet GetDataSet(string strStoredProcedureName, List<SqlParameter> lstSqlParameters)
        {
            return ExecuteStoredProcedure(strStoredProcedureName, lstSqlParameters);
        }
        
        public DataTable GetDataTable(string strStoredProcedureName, int intIndex = 0)
        {
            return GetDataTable(strStoredProcedureName, null, intIndex);
        }
        public DataTable GetDataTable(string strStoredProcedureName, List<SqlParameter> lstSqlParameters ,int intIndex = 0)
        {
            var dataSet = GetDataSet(strStoredProcedureName, lstSqlParameters);
            if (dataSet != null && intIndex < dataSet.Tables.Count)
            {
                return dataSet.Tables[intIndex];
            }
            return null;
        }


        /// <summary>
        /// Executes specified stored procedure and returns the first value returned.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strStoredProcedureName">Name of the string stored procedure.</param>
        /// <returns></returns>
        public T GetDataValue<T>(string strStoredProcedureName)
        {
            return GetDataValue<T>(strStoredProcedureName, null);
        }
        public T GetDataValue<T>(string strStoredProcedureName, List<SqlParameter> lstSqlParameters)
        {
            try
            {
                Logger.Trace("Retrieving value of type: <{0}> with Stored Procedure: {1}", typeof(T), strStoredProcedureName);
                var dataSet = GetDataSet(strStoredProcedureName, lstSqlParameters);
                return (T) dataSet.Tables[0].Rows[0][0];
            }
            catch (InvalidCastException ex)
            {
                Logger.Error("Unable to cast value retrieved by: {0} to type: <{1}>. {2}", strStoredProcedureName, typeof(T), ex.Message);
                return default(T);
            }
            catch (Exception ex)
            {
                Logger.Error("Exception: {0}", ex.Message);
                return default(T);
            }

        }


        public void Dispose()
        {
            //TODO Refactor if neccessary for disposal... currently wrapped in method dispose...
            Debug.WriteLine("Disposed SQL Helper.");
        }
    }
}
