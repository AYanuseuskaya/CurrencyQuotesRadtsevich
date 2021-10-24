using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace ModelTestTask
{
    public class CurrencyCode
    {
        public DateTime Date { get; set; }
        public Dictionary<string, Code> Valute { get; set; }
       
    }
    public class Code
    {
        public string ID { get; set; }
        public string NumCode { get; set; }
        public string CharCode{ get; set; }
        public int Nominal { get; set; }
        public string Name { get; set; }
        public decimal Value { get; set; }
        public decimal Previous { get; set; }
    }
}
