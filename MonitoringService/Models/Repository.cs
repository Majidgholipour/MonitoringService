using MonitoringService.DTOs;
using MonitoringService.Hubs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace MonitoringService.Models
{
    public class Repository
    {
        SqlConnection con = new SqlConnection(System.Web.Configuration.WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

        public List<ResourceDTO> getResource()
        {
            var messages = new List<ResourceDTO>();
            using (var cmd = new SqlCommand(@"select top 10 r.InsertedDateTime,c.CountOfCore,c.CountOfProcessoer,c.Speed,c.UsagePresent,
                                                m.Avialible,m.Cached,m.Commited,m.InUse,m.Size,m.Usage,
                                                d.AvailableFreeSpace,d.DrivesInfo,d.TotalFreesize,d.TotalSize,
                                                e.AdaptorName,e.ConnectionType,e.DomainName,e.IPv4Address,e.IPv6Address,e.Receive,e.Send,
                                                s.Domain,s.HostName,s.LocalIP,s.ServerName
                                                from ResourceInfoes r 
                                                 inner join CPUInfo c on c.Id=r.CPUInfo_Id
                                                inner join MemoryInfo m on m.Id=r.MemoryInfo_Id
                                                inner join EthernetInfo e on e.Id=r.ethernetInfo_Id
                                                inner join DiskInfo d on d.Id=r.DiskInfo_Id
                                                inner join ServerInfo s on s.Id=r.ServerInfo_Id
                                                order by r.InsertedDateTime desc", con))
            {
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                var dependency = new SqlDependency(cmd);
                dependency.OnChange += new OnChangeEventHandler(dependency_OnChange);
                DataTable ds = new DataTable();
                da.Fill(ds);
                for (int i = 0; i < ds.Rows.Count; i++)
                {
                    messages.Add(item: new ResourceDTO
                    {
                        InsertedDateTime = Convert.ToDateTime(ds.Rows[i]["InsertedDateTime"].ToString()),
                        CountOfCore = Convert.ToInt32(ds.Rows[i]["CountOfCore"].ToString()),
                        CountOfProcessoer = Convert.ToInt32(ds.Rows[i]["CountOfProcessoer"]),
                        Speed = Convert.ToInt32(ds.Rows[i]["Speed"].ToString()),
                        UsagePresent = Convert.ToInt32(ds.Rows[i]["UsagePresent"].ToString()),

                        Avialible = (float)Convert.ToDecimal(ds.Rows[i]["Avialible"].ToString()),
                        Cached = (float)Convert.ToDecimal(ds.Rows[i]["Cached"].ToString()),
                        Commited = (float)Convert.ToDecimal(ds.Rows[i]["Commited"].ToString()),
                        InUse = (float)Convert.ToDecimal(ds.Rows[i]["InUse"].ToString()),
                        Size = (float)Convert.ToDecimal(ds.Rows[i]["Size"].ToString()),
                        Usage = (float)Convert.ToDecimal(ds.Rows[i]["Usage"].ToString()),

                        AvailableFreeSpace = Convert.ToInt64(ds.Rows[i]["AvailableFreeSpace"].ToString()),
                        //DrivesInfo = ds.Rows[i]["DrivesInfo"].ToString(),
                        TotalFreesize = Convert.ToInt64(ds.Rows[i]["TotalFreesize"].ToString()),
                        TotalSize = Convert.ToInt64(ds.Rows[i]["TotalSize"].ToString()),

                        AdaptorName = ds.Rows[i]["AdaptorName"].ToString(),
                        ConnectionType = ds.Rows[i]["ConnectionType"].ToString(),
                        DomainName = ds.Rows[i]["DomainName"].ToString(),
                        IPv4Address = ds.Rows[i]["IPv4Address"].ToString(),
                        IPv6Address = ds.Rows[i]["IPv6Address"].ToString(),
                        Receive = Convert.ToDecimal(ds.Rows[i]["Receive"].ToString()),
                        Send = Convert.ToDecimal(ds.Rows[i]["Send"].ToString()),

                        Domain = ds.Rows[i]["Domain"].ToString(),
                        HostName = ds.Rows[i]["HostName"].ToString(),
                        LocalIP = ds.Rows[i]["LocalIP"].ToString(),
                        ServerName = ds.Rows[i]["ServerName"].ToString()
                    });
                }
            }
            return messages;
        }

        public List<empDTO> getEmp()
        {
            var messages = new List<empDTO>();
            using (var cmd = new SqlCommand(@"SELECT 
                                                  [empName]
                                                  ,[Salary]
                                                  ,[DeptName]
                                                  ,[Designation]
                                              FROM [dbo].[Employee]", con))
            {
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                var dependency = new SqlDependency(cmd);
                dependency.OnChange += new OnChangeEventHandler(dependency_OnChange);
                DataTable ds = new DataTable();
                da.Fill(ds);
                for (int i = 0; i < ds.Rows.Count; i++)
                {
                    messages.Add(item: new empDTO
                    {
                        empName = ds.Rows[i]["empName"].ToString(),
                        Salary = Convert.ToInt32(ds.Rows[i]["Salary"].ToString()),
                        DeptName =ds.Rows[i]["DeptName"].ToString(),
                        Designation =ds.Rows[i]["Designation"].ToString()
                        
                    });
                }
            }
            return messages;
        }



        private void dependency_OnChange(object sender, SqlNotificationEventArgs e) //this will be called when any changes occur in db table. 
        {
            if (e.Type == SqlNotificationType.Change || e.Type == SqlNotificationType.Subscribe)
            {
                MyHub.SendMessages();
            }
        }
    }
}