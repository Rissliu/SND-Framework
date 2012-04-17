using System;
using System.Xml;

using SND.DA.DataAccessHelper;

namespace SND.KQ.DAL.Well
{
	/// <summary>
	/// Summary description for Common.
	/// </summary>
	public abstract class Common
	{
		private static SqlClientDataCommands mCommands;

		public static SqlClientDataCommands Commands
		{
			get
			{
				return mCommands;
			}
		}

		public static void Initialize(string strConnectionString, 
			DataSourceType dataSourceType)
		{
			if(dataSourceType == DataSourceType.SqlClient)
			{
				if(System.Diagnostics.Debugger.IsAttached &&
                    System.IO.File.Exists(@"..\..\..\SND.KQ.DAL\Well\WellDataCommands.xml"))
					// In debug, we use the file
					mCommands = new SqlClientDataCommands(
                        new XmlTextReader(@"..\..\..\SND.KQ.DAL\Well\WellDataCommands.xml"),
						strConnectionString);
				else
					// Only use the resource in release
					mCommands = new SqlClientDataCommands(
                        "SND.KQ.DAL.Well.WellDataCommands.xml", 
						strConnectionString);
			}
			else
				throw new NotSupportedException();
		}

		public static void VerifyEnum(DataConnection conn, 
			Type enumType, string strTableName, string strValueColumnName,
			string strNameColumnName, string strDescriptionColumnName)
		{
			EnumVerifier.Verify(conn, enumType,
				strTableName, strValueColumnName, strNameColumnName,
				strDescriptionColumnName);
		}

        /// <summary>
        /// Get a DataConnection instance for the given connection string and 
        /// data source type.
        /// </summary>
        /// <returns></returns>
        public static DataConnection GetDataConnection(
            string strConnectionString, DataSourceType dataSourceType)
        {
            switch (dataSourceType)
            {
                case DataSourceType.SqlClient:
                    return new SqlDataConnection(
                        strConnectionString, mCommands);

                case DataSourceType.OleDb:
                    return new OleDbDataConnection(
                        strConnectionString, mCommands);

                case DataSourceType.OracleClient:
                    throw new NotImplementedException();
            }

            throw new NotSupportedException();
        }
	}
}
