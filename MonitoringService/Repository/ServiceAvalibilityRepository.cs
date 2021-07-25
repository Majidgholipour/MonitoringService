using MonitoringService.DTOs;
using MonitoringService.Hubs;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MonitoringService.Repository
{
    public class ServiceAvalibilityRepository
    {
        public static List<ServiceAvalibilityDTO> GetServiceAvalibilityRecords()
        {
            var lstServiceAvalibilityRecords = new List<ServiceAvalibilityDTO>();
            string dbConnectionSettings = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            using (var dbConnection = new SqlConnection(dbConnectionSettings))
            {
                dbConnection.Open();

                var sqlCommandText = @"SELECT top 10
                                         isnull(Domain,'')Domain,
	                                     isnull(LocalIP,'')LocalIP,
	                                     isnull(HostName,'')HostName,
	                                     isnull(ServerName,'')ServerName,
	                                     isnull(convert(varchar, GenerateDateTime, 21) ,'')GenerateDateTime,
                                          case serviceAvalibilityType when 0 then 'FirstConnect'
	                                      when 1 then 'RuntimeConnect' end serviceAvalibilityType
                                      FROM dbo.ServiceAvailabilityInfo
                                      order by GenerateDateTime desc";

                using (var sqlCommand = new SqlCommand(sqlCommandText, dbConnection))
                {
                    AddSQLDependency(sqlCommand);

                    if (dbConnection.State == ConnectionState.Closed)
                        dbConnection.Open();

                    var reader = sqlCommand.ExecuteReader();
                    lstServiceAvalibilityRecords = GetServiceAvalibilityRecords(reader);
                }
            }
            return lstServiceAvalibilityRecords;
        }

        /// <summary>
        /// Adds SQLDependency for change notification and passes the information to ServiceAvalibility Hub for broadcasting
        /// </summary>
        /// <param name="sqlCommand"></param>
        private static void AddSQLDependency(SqlCommand sqlCommand)
        {
            sqlCommand.Notification = null;

            var dependency = new SqlDependency(sqlCommand);

            dependency.OnChange += (sender, sqlNotificationEvents) =>
            {
                if (sqlNotificationEvents.Type == SqlNotificationType.Change || sqlNotificationEvents.Type == SqlNotificationType.Subscribe)
                {
                    MyHub.SendUptodateInformation(sqlNotificationEvents.Info.ToString());
                }
            };
        }

        /// <summary>
        /// Fills the ServiceAvalibility Records
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private static List<ServiceAvalibilityDTO> GetServiceAvalibilityRecords(SqlDataReader reader)
        {
            var lstServiceAvalibilityRecords = new List<ServiceAvalibilityDTO>();
            var dt = new DataTable();
            dt.Load(reader);
            dt
                .AsEnumerable()
                .ToList()
                 .ForEach
                (
                    item => lstServiceAvalibilityRecords.Add(new ServiceAvalibilityDTO()
                    {
                        HostName = (string)item["HostName"],
                        Domain = (string)item["Domain"],
                        LocalIP = (string)item["LocalIP"],
                        ServerName = (string)item["ServerName"],
                        GenerateDateTime = (string)item["GenerateDateTime"],
                        serviceAvalibilityType= (string)item["serviceAvalibilityType"],
                    })
                );
            return lstServiceAvalibilityRecords;
        }
    }
}