using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnyRec.Server
{
    internal class Bytes
    {
        public static byte[] JsonObjectToByte(object p)
        {
            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(p));
        }
        public static byte[] StringToByte(string p)
        {
            return Encoding.UTF8.GetBytes(p);
        }
    }
}
