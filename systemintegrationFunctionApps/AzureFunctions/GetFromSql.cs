using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace systemintegrationFunctionApps.AzureFuntions
{
    public static class GetFromSql
    {
        [FunctionName("GetFromSql")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("***Fetching data from SQL***");

            string query = "EXEC GetSQLData";

            List<DeviceMessage> deviceMessages = new List<DeviceMessage>();

            using (SqlConnection sql_Connection = new SqlConnection(Environment.GetEnvironmentVariable("Sqldb_Conn")))
            {
                sql_Connection.Open();

                using (SqlCommand sql_command = new SqlCommand(query, sql_Connection))
                {
                    try
                    {
                        using (SqlDataReader sql_DataReader = sql_command.ExecuteReader())
                        {
                            while (sql_DataReader.Read())
                            {
                                DeviceMessage deviceMessage = new DeviceMessage
                                {
                                    

                                    temperature = float.Parse(sql_DataReader["Temperature"].ToString()),
                                    humidity = float.Parse(sql_DataReader["Humidity"].ToString()),
                                    epochtime = long.Parse(sql_DataReader["Epochtime"].ToString()),
                                    id = int.Parse(sql_DataReader["id"].ToString())
                                  
                                };
                                deviceMessages.Add(deviceMessage);
                            }
                        }
                    }
                    catch
                    {
                        log.LogInformation("**SQL FETCHING ERROR***");
                    }
                }
            };

            return deviceMessages != null
                ? (ActionResult)new OkObjectResult(deviceMessages)
                : new BadRequestResult();
        }
    }
}
