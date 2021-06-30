using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace Google_Direction_API
{
    class Program
    {
        static readonly HttpClient client = new HttpClient();

        static async Task Main(string[ ] args)
        {
            // Call asynchronous network methods in a try/catch block to handle exceptions.
            try
            {
                //Diection API 參數配置
                string origin = "origin=23.706478,120.549414";
                string destination = "&destination=23.691157118064577,120.53531912566041";
                string mode = "&mode=walking";
                string language = "&language=zh-TW";
                string key = "&key=AIzaSyArO6pg7jjavY0zoiuREcqrKjQdPgLDnXs";
                string url = "https://maps.googleapis.com/maps/api/directions/json?" + origin + destination + mode + language + key;
                //發送request
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                //收到json字串
                string responseBody = await response.Content.ReadAsStringAsync();
                //處理json字串
                
                JObject json_all = JObject.Parse(responseBody);
                IList<JToken> route = json_all["routes"].Children().ToList();//用list取得routes底下的所有children並轉換成list
                string str_route = string.Join<JToken>(",", route.ToArray());//將list轉換成字串以供下一個分析
                JObject json_legs = JObject.Parse(str_route);
                IList<JToken> legs = json_legs["legs"].Children().ToList();
                string str_legs = string.Join<JToken>(",", legs.ToArray());
                JObject json_steps = JObject.Parse(str_legs);
                IList<JToken> steps = json_steps["steps"].Children().ToList();
                string str_steps = string.Join<JToken>(",", steps.ToArray());
                int num = steps.Count;//確認steps這個list有多少個項目在裡面
                //用新的list來存數值
                List<double> start_lat = new List<double>();
                List<double> start_lng = new List<double>();
                List<double> end_lat = new List<double>();
                List<double> end_lng = new List<double>();
                //逐個放進list中
                for (int i=0;i<num;i++)
                {
                    start_lat.Add((double)steps[i]["start_location"]["lat"]);
                    start_lng.Add((double)steps[i]["start_location"]["lng"]);
                    end_lat.Add((double)steps[i]["end_location"]["lat"]);
                    end_lng.Add((double)steps[i]["end_location"]["lng"]);
                }
                //顯示數值
                Console.WriteLine(steps[2]);
                Console.WriteLine("Start lat：" + start_lat[2].ToString());
                Console.WriteLine("Start lng：" + start_lng[2].ToString());
                Console.WriteLine("End lat：" + end_lat[2].ToString());
                Console.WriteLine("End lng：" + end_lng[2].ToString());
            }

            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }
        }
    }
}