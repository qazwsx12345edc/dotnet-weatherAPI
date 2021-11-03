using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 存放一些与数据库无关的公共方法
/// </summary>
namespace Infrastructure.Utils
{
    public class BaseUtils
    {
        /// <summary>
        /// 钉钉机器人发送消息
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public string RobotSend(string msg)
        {
            var result = "";
            string url = "https://oapi.dingtalk.com/robot/send?access_token=778bb8e7ed6998792f36f820a6686a51e13eaf7638fe0afab4ee277bc083097b";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "POST";
            req.ContentType = "application/json;charset=utf-8";

            var sendObject = new
            {
                msgtype = "text",
                text = new
                {
                    content = msg
                }
            };
            string sendJson = JsonConvert.SerializeObject(sendObject);
            var sendBytes = Encoding.UTF8.GetBytes(sendJson);

            using (Stream reqStream = req.GetRequestStream())
            {
                reqStream.Write(sendBytes, 0, sendBytes.Length);
            }
            HttpWebResponse res = (HttpWebResponse)req.GetResponse();
            Stream stream = res.GetResponseStream();
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                result = reader.ReadToEnd();
            }
            return result;
        }

        /// <summary>
        /// HttpWebRequest SetHeaderValue
        /// </summary>
        /// <param name="header"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void SetHeaderValue(WebHeaderCollection header, string name, string value)
        {
            var property = typeof(WebHeaderCollection).GetProperty("InnerCollection", BindingFlags.Instance | BindingFlags.NonPublic);
            if (property != null)
            {
                var collection = property.GetValue(header, null) as NameValueCollection;
                collection[name] = value;
            }
        }
    }
}
