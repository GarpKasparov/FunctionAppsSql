using IoTHubTrigger = Microsoft.Azure.WebJobs.EventHubTriggerAttribute;

using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.EventHubs;
using System.Text;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Data.SqlClient;
using Newtonsoft.Json;

namespace systemintegrationFunctionApps
{
    public static class SaveToSql
    {
        private static HttpClient client = new HttpClient();

        [FunctionName("SaveToSql")]
        public static void Run([IoTHubTrigger("messages/events", Connection = "IotHub", ConsumerGroup ="systemintegration" ),]EventData message, ILogger log)
        {
           log.LogInformation( "I början...");
            DeviceMessage deviceMessage = JsonConvert.DeserializeObject<DeviceMessage>(Encoding.UTF8.GetString(message.Body.Array));
            using (SqlConnection sql_Connection = new SqlConnection(Environment.GetEnvironmentVariable("Sqldb_Conn")))
                
            {
                sql_Connection.Open();
                log.LogInformation("I mitten...");





                using (SqlCommand sql_command = new SqlCommand("INSERT INTO SensorData (Temperature, Humidity, Epochtime) VALUES(@temperature, @humidity, @epochtime)", sql_Connection)) {
                    log.LogInformation("I slutet...");
                    sql_command.Parameters.AddWithValue("@temperature", deviceMessage.temperature);
                    sql_command.Parameters.AddWithValue("@humidity", deviceMessage.humidity);
                    sql_command.Parameters.AddWithValue("@epochtime", deviceMessage.epochtime);
                    sql_command.ExecuteNonQuery();
                    

                }
            }



            log.LogInformation($"C# IoT Hub trigger function processed a message: {Encoding.UTF8.GetString(message.Body.Array)}");
        }
    }
}