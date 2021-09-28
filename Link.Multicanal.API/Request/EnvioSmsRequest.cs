using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Link.Multicanal.API.Request
{
    public class EnvioSmsRequest
    {
        public string login { get; set; }
        public string password { get; set; }
        public string tipoProgramacion { get; set; }
        public string dia { get; set; }
        public string mes { get; set; }
        public string ano { get; set; }
        public string hora { get; set; }
        public string minuto { get; set; }
        public string departamento { get; set; }
        public string referencia { get; set; }
        public string asunto { get; set; }
        public string RemitenteSms { get; set; }
        public string texto { get; set; }
        public string destino { get; set; }
    }
}
