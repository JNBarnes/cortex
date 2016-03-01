using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Cortex.Database.StoredProcedures
{
    public abstract class StoredProcedure
    {
        private List<SqlParameter> _lstParameters;

        protected string Name { get; set; }

        protected List<SqlParameter> Parameters
        {
            get { return _lstParameters ?? (_lstParameters = new List<SqlParameter>()); }

            set
            {
                _lstParameters = value;
            }
        }

        protected DataSet GetDataSet()
        {
            var strConnectionString = ConfigurationManager.ConnectionStrings["aws"].ConnectionString;

            using (var conn = new SqlConnection(strConnectionString))
            {
                try
                {
                    using (var dataAdapter = new SqlDataAdapter())
                    {
                        dataAdapter.SelectCommand = new SqlCommand(Name, conn) { CommandType = CommandType.StoredProcedure };
                        foreach (var param in _lstParameters)
                        {
                            dataAdapter.SelectCommand.Parameters.Add(param);
                        }

                        var dataSet = new DataSet();
                        dataAdapter.Fill(dataSet);
                        return dataSet;
                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("SQL Error: " + ex.Message);
                    return null;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error: " + e.Message);
                    return null;
                }
            }
        }

        protected DataTable GetDataTable(int intIndex = 0)
        {
            var dataSet = GetDataSet();
            if (dataSet != null && intIndex < dataSet.Tables.Count)
            {
                return dataSet.Tables[intIndex];
            }
            return null;
        }

    }
}
