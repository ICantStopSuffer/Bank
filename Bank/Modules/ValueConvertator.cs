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

        public double Convert(double value, valueType type) {

            if (value < 0) {
                throw Logger.logException(new InvalidAmountError("Количество должно быть положительным"));
            }

            var kotirovki = getKot();

            return value * kotirovki[Enum.GetName(typeof(valueType), type)];

        }

        public Dictionary<string, double> getKot() {
            string page = client.DownloadString("https://www.cbr-xml-daily.ru/latest.js");

            var jobject = JObject.Parse(page);
            var values = JsonConvert.DeserializeObject<Dictionary<string, double>>(jobject["rates"].ToString());

            return values;
        }
    }
}
