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
    public class ResourceRepository
    {
        public static List<ResourceDTO> GetResourceRecords()
        {
            var lstResourceRecords = new List<ResourceDTO>();
            string dbConnectionSettings = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            using (var dbConnection = new SqlConnection(dbConnectionSettings))
            {
                dbConnection.Open();

                var sqlCommandText = @"select top 10 
                                            r.Id RID,InsertedDateTime,CPUInfo_Id,DiskInfo_Id,ethernetInfo_Id,MemoryInfo_Id,ServerInfo_Id,
                                            c.Id CID,UsagePresent,CountOfCore,Speed,CountOfProcessoer,
                                            m.Id MID ,Size,Usage,InUse,Avialible,Cached,Commited,
                                            d.Id DID,TotalSize,TotalFreesize,AvailableFreeSpace,DrivesInfo,
                                            e.Id EID,AdaptorName,isnull(e.DomainName,'') DomainName,ConnectionType,IPv4Address,isnull(e.IPv6Address,'') IPv6Address,Send,Receive,
                                            s.Id SID ,Domain,LocalIP,HostName,ServerName
                                            from ResourceInfoes r 
                                            inner join CPUInfo c on c.Id=r.CPUInfo_Id
                                            inner join MemoryInfo m on m.Id=r.MemoryInfo_Id
                                            inner join EthernetInfo e on e.Id=r.ethernetInfo_Id
                                            inner join DiskInfo d on d.Id=r.DiskInfo_Id
                                            inner join ServerInfo s on s.Id=r.ServerInfo_Id
                                            order by InsertedDateTime desc";

                using (var sqlCommand = new SqlCommand(sqlCommandText, dbConnection))
                {
                    AddSQLDependency(sqlCommand);

                    if (dbConnection.State == ConnectionState.Closed)
                        dbConnection.Open();

                    var reader = sqlCommand.ExecuteReader();
                    lstResourceRecords = GetResourceRecords(reader);
                }
            }
            return lstResourceRecords;
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

        /// <summary>
        /// Fills the Resource Records
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private static List<ResourceDTO> GetResourceRecords(SqlDataReader reader)
        {
            var lstResourceRecords = new List<ResourceDTO>();
            var dt = new DataTable();
            dt.Load(reader);
            dt
                .AsEnumerable()
                .ToList()
                .ForEach
                (
                    item => lstResourceRecords.Add(new ResourceDTO()
                    {
                        AdaptorName = (string)item["AdaptorName"],
                        Avialible = (float)item["Avialible"],
                        AvailableFreeSpace = (long)item["AvailableFreeSpace"],
                        Commited = (float)item["Commited"],
                        ConnectionType = (string)item["ConnectionType"],
                        CountOfCore = (int)item["CountOfCore"],
                        CountOfProcessoer = (int)item["CountOfProcessoer"],
                        Domain = (string)item["Domain"],
                        DomainName = (string)item["DomainName"],
                        DrivesInfo = (string)item["DrivesInfo"],
                        HostName = (string)item["HostName"],
                        InsertedDateTime = Convert.ToDateTime( item["InsertedDateTime"]),
                        InUse = (float)item["InUse"],
                        IPv4Address = (string)item["IPv4Address"],
                        IPv6Address = (string)item["IPv6Address"],
                        LocalIP = (string)item["LocalIP"],
                        Receive = (decimal)item["Receive"],
                        Send = (decimal)item["Send"],
                        ServerName = (string)item["ServerName"],
                        Size = (float)item["Size"],
                        Cached = (float)item["Cached"],
                        Speed = (float)item["Speed"],
                        TotalFreesize = (long)item["TotalFreesize"],
                        TotalSize = (long)item["TotalSize"],
                        Usage = (float)item["Usage"],
                        UsagePresent = (float)item["UsagePresent"],
                    })
                );
            return lstResourceRecords;
        }
    }
}