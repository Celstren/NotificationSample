using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NotificationSample.Dto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace NotificationSample.Controllers
{
    [ApiController]
    [Route("api")]
    public class WeatherForecastController : ControllerBase
    {

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private static readonly string ServerKey = "AAAA6y04l-A:APA91bEvoQwstGMUkjyiJCaHd5YaSzAxYqrVj5Ihs8M5-u2mQ6RC8hRd953oS9AWiS4Hber-cK2a3EW1enmnsR1Pak4PPuZ11_oY_cEu2uLZ9wyQzlg8zTEREZttbNL_tt1DtltoHMlF";
        private static readonly string SenderId = "1010075998176";

        public string PostCustomNotification(SendNotificationDto notification)
        {
            try
            {
                WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
                tRequest.Method = "post";
                //serverKey - Key from Firebase cloud messaging server  
                tRequest.Headers.Add(string.Format("Authorization: key={0}", ServerKey));
                //Sender Id - From firebase project setting  
                tRequest.Headers.Add(string.Format("Sender: id={0}", SenderId));
                tRequest.ContentType = "application/json";
                var payload = new
                {
                    to = notification.Token,
                    priority = "high",
                    content_available = true,
                    notification = new
                    {
                        body = notification.Message,
                        title = notification.Title,
                        badge = 1
                    },
                    data = new
                    {
                        key1 = "value1",
                        key2 = "value2"
                    }

                };

                string postbody = JsonConvert.SerializeObject(payload).ToString();
                var byteArray = Encoding.UTF8.GetBytes(postbody);
                tRequest.ContentLength = byteArray.Length;

                Stream dataStream = tRequest.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();

                WebResponse tResponse = tRequest.GetResponse();

                dataStream = tResponse.GetResponseStream();

                StreamReader tReader = new StreamReader(dataStream);

                var sResponseFromServer = tReader.ReadToEnd();


                tReader.Close();
                dataStream.Close();
                tResponse.Close();
                return sResponseFromServer;
            }
            catch (Exception ex)
            {
                throw new Exception($"Hubo un error en la base de datos - {JsonConvert.SerializeObject(ex)}");
            }
        }

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet("test")]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpPost("notification")]
        [ProducesResponseType(typeof(IEnumerable<string>), (int)HttpStatusCode.OK)]
        public IActionResult PostNotification([FromBody] SendNotificationDto request)
        {
            return Ok(PostCustomNotification(request));
        }
    }
}
