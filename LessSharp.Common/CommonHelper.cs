using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;

namespace LessSharp.Common
{
    /// <summary>
    /// 公共助手
    /// </summary>
    public class CommonHelper
    {
        public static string ObjectToJsonString(object obj)
        {
            var setting = new JsonSerializerSettings
            {
                ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
            };
            return JsonConvert.SerializeObject(obj, setting);
        }
        public static T JsonStringToObject<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static object JsonStringToObject(string json, Type type)
        {
            return JsonConvert.DeserializeObject(json, type);
        }
        public static string CamelCaseToLowerUnderScore(string name)
        {
            return Regex.Replace(name, "([a-z])([A-Z])", "$1_$2").ToLower();
        }

        /// <summary>
        /// 计算两个点之间的距离，单位米
        /// </summary>
        /// <param name="lat"></param>
        /// <param name="lng"></param>
        /// <param name="lat2"></param>
        /// <param name="lng2"></param>
        /// <returns></returns>
        public static double GetDistance(double lat, double lng, double lat2, double lng2)
        {
            double EARTH_RADIUS = 6378.137;
            double radLat1 = lat * Math.PI / 180.0;
            double radLat2 = lat2 * Math.PI / 180.0;
            var a = radLat1 - radLat2;
            var b = (lng * Math.PI / 180.0) - (lng2 * Math.PI / 180.0);
            var s = 2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(a / 2), 2) + Math.Cos(radLat1) * Math.Cos(radLat2) * Math.Pow(Math.Sin(b / 2), 2)));
            s = s * EARTH_RADIUS;
            s = Math.Round(s * 1000);
            return s;
        }
    }
}
