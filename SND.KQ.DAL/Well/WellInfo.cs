
namespace SND.KQ.DAL.Well
{
    using SND.DA.CommandBuilder;
    using SND.DA.DataAccessHelper;
    using System.Data;
    using System.Collections.Generic;
using System;
    
    
    public partial class WellInfo
    {
        
        private DataConnection mConnection;

        public WellInfo(DataConnection conn)
        {
            this.mConnection = ((DataConnection)(conn));
        }
        public virtual System.Data.DataTable GetWellInfo(string userId,DateTime firstTime,DateTime lastTime)
        {
            try
            {
                DataSet ds = new DataSet();
                Command cmdBuilder = mConnection.Commands.GetCommand(CommandEnumeration.GetWellInfo);
                System.Data.Common.DataTableMappingCollection tableMappings = null;
                tableMappings = new System.Data.Common.DataTableMappingCollection();
                tableMappings.Add("Table", "Table1");
                cmdBuilder.Parameters["UserId"].Value = userId;
                cmdBuilder.Parameters["FirtTime"].Value = firstTime;
                cmdBuilder.Parameters["LastTime"].Value = lastTime;
                cmdBuilder.ExecuteAndFillDataSet(mConnection.GetConnection(), mConnection.Transaction, ds, tableMappings);
                if (ds.Tables.Count > 0)
                {
                    return ds.Tables["Table1"];
                }
                return null;
            }
            finally
            {
 
            }
        }
    }
}
