using MonitoringService.DTOs;
using MonitoringService.Hubs;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace MonitoringService.Repository
{
    public class ApplicationLogRepository
    {
        public static List<ApplicationLogDTO> GetApplicationLogRecords()
        {
            var lstStudentRecords = new List<ApplicationLogDTO>();
            string dbConnectionSettings = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            using (var dbConnection = new SqlConnection(dbConnectionSettings))
            {
                dbConnection.Open();

                var sqlCommandText = @"
        SELECT a.Id
      ,isnull(Message,'')Message
      ,isnull(ApplicationName,'')ApplicationName
      ,isnull(convert(varchar, GeneratedDate, 21) ,'')GeneratedDate
      , case LogType when 0 then 'Information'
	  when 1 then 'Error'
	  when 2 then 'Warning'
	  else ''
	  end LogType
      ,ServerInfo_ID
	  ,isnull(s.Domain,'') Domain
	  ,isnull(s.HostName,'')HostName
	  ,isnull(s.LocalIP,'')LocalIP
	  ,isnull(s.ServerName,'')ServerName
  FROM dbo.ApplicationLog a
  inner join ServerInfo s on s.Id =ServerInfo_ID
";

                using (var sqlCommand = new SqlCommand(sqlCommandText, dbConnection))
                {
                    AddSQLDependency(sqlCommand);

                    if (dbConnection.State == ConnectionState.Closed)
                        dbConnection.Open();

                    var reader = sqlCommand.ExecuteReader();
                    lstStudentRecords = GetStudentRecords(reader);
                }
            }
            return lstStudentRecords;
        }

        private static void AddSQLDependency(SqlCommand sqlCommand)
        {
            sqlCommand.Notification = null;

            var dependency = new SqlDependency(sqlCommand);

            dependency.OnChange += (sender, sqlNotificationEvents) =>
            {
                if (sqlNotificationEvents.Type == SqlNotificationType.Change|| sqlNotificationEvents.Type == SqlNotificationType.Subscribe)
                {
                    MyHub.SendUptodateInformation(sqlNotificationEvents.Info.ToString());
                }
            };
        }

        private static List<ApplicationLogDTO> GetStudentRecords(SqlDataReader reader)
        {
            var lstStudentRecords = new List<ApplicationLogDTO>();
            var dt = new DataTable();
            dt.Load(reader);
            dt
                .AsEnumerable()
                .ToList()
                .ForEach
                (
                    item => lstStudentRecords.Add(new ApplicationLogDTO()
                    {
                         ApplicationName = (string)item["ApplicationName"],
                        LogType= (string)item["LogType"],
                        Message= (string)item["Message"],
                        GeneratedDate= (string) item["GeneratedDate"],
                        Domain = (string)item["Domain"],
                        HostName = (string)item["HostName"],
                        LocalIP = (string)item["LocalIP"],
                        ServerName = (string)item["ServerName"]
                    })
                );
            return lstStudentRecords;
        }
    }
}