using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;
using WebSocketSharp.Server;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;



namespace BlackClover
{
    public class Echo : WebSocketBehavior
    {
        protected override void OnMessage(MessageEventArgs e)
        {
            Console.WriteLine("im the server");
            Send(e.Data);
        }
    }
    public class Server
    {
        public static void Main(string[] args)
        {

            var wssv = new WebSocketServer(port: 8080);
            wssv.AddWebSocketService<Echo>("/Echo");
            //string s = "";
            // string s = "{\"moveLog\":[{\"mov\": \"hi\"},{\"mov\": \"hi\"}]}";
            string s = "{\"moveLog\":[{\"move\":{\"type\":\"place\",\"point\":{\"row\":3,\"column\":16}},\"deltaTime\":42113},{\"move\":{\"type\":\"place\",\"point\":{\"row\":9,\"column\":9}},\"deltaTime\":11842}]}";

            //string s = "{\"type\":\"NAME\",\"name\":\"credit\"}";
            JObject o = JObject.Parse(s);
            JArray array = new JArray();
            array = (JArray)o["moveLog"];
           //Console.WriteLine(array.Count);
           

           // Console.WriteLine("hiiiiiiii");

            //wssv.Start();
            //Client c1 = new Client();

           //c1.Start();



            Console.ReadKey(true);
            wssv.Stop();
        }
    }
}