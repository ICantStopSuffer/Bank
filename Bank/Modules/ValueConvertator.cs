using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Bank.Modules {
    public class ValueConvertator {
        System.Net.WebClient client = new System.Net.WebClient();



        public void Convert(double value, valueType type) {
        }

        public Dictionary<string, double> getKot() {
            string page = client.DownloadString("https://www.cbr-xml-daily.ru/latest.js");

            var jobject = JObject.Parse(page);
            var values = JsonConvert.DeserializeObject<Dictionary<string, double>>(jobject["rates"].ToString());

            return values;

            //var responseString = await client.GetAsync("https://www.cbr-xml-daily.ru/money.js");
            //Console.WriteLine(responseString);

            //using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "https://www.google.com");
            // выполняем запрос
            //await httpClient.SendAsync(request);
            //Dictionary<valueType,double>
        }
    }
}
