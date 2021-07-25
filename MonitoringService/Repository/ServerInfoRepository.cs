using MonitoringService.DTOs;
using MonitoringService.Hubs;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace MonitoringService.Models
{
    public class ServerInfoRepository
    {
        public static List<ServerInfoDTO> GetServerInfoRecords()
        {
            var lstServerInfoRecords = new List<ServerInfoDTO>();
            string dbConnectionSettings = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            using (var dbConnection = new SqlConnection(dbConnectionSettings))
            {
                dbConnection.Open();

                var sqlCommandText = @"SELECT distinct
      Domain
      ,LocalIP
      ,HostName
      ,ServerName
  FROM ServerDB.dbo.ServerInfo";

                using (var sqlCommand = new SqlCommand(sqlCommandText, dbConnection))
                {
                    AddSQLDependency(sqlCommand);

                    if (dbConnection.State == ConnectionState.Closed)
                        dbConnection.Open();

                    var reader = sqlCommand.ExecuteReader();
                    lstServerInfoRecords = GetServerInfoRecords(reader);
                }
            }
            return lstServerInfoRecords;
        }

        /// <summary>
        /// Adds SQLDependency for change notification and passes the information to ServerInfo Hub for broadcasting
        /// </summary>
        /// <param name="sqlCommand"></param>
        private static void AddSQLDependency(SqlCommand sqlCommand)
        {
            sqlCommand.Notification = null;

            var dependency = new SqlDependency(sqlCommand);

            dependency.OnChange += (sender, sqlNotificationEvents) =>
            {
                if (sqlNotificationEvents.Type == SqlNotificationType.Change)
                {
                    MyHub.SendUptodateInformation(sqlNotificationEvents.Info.ToString());
                }
            };
        }

        /// <summary>
        /// Fills the ServerInfo Records
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private static List<ServerInfoDTO> GetServerInfoRecords(SqlDataReader reader)
        {
            var lstServerInfoRecords = new List<ServerInfoDTO>();
            var dt = new DataTable();
            dt.Load(reader);
            dt
                .AsEnumerable()
                .ToList()
                 .ForEach
                (
                    item => lstServerInfoRecords.Add(new ServerInfoDTO()
                    {
                        HostName = (string)item["HostName"],
                        Domain = (string)item["Domain"],
                        LocalIP = (string)item["LocalIP"],
                        ServerName = (string)item["ServerName"]
                    })
                );
            return lstServerInfoRecords;
        }
    }
}