using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Link.Multicanal.API.Response
{
    public class EnvioSmsResponse
    {
        public string resultado { get; set; }
        public string descripcion { get; set; }
        public string refWeb { get; set; }

        public static EnvioSmsResponse BuildResponse(string webServiceResponse)
        {

            EnvioSmsResponse result = null;

            if (!String.IsNullOrEmpty(webServiceResponse))
            {
                var webServiceResponse_part1 = String.Empty;
                var webServiceResponse_part2 = String.Empty;
                if (webServiceResponse.Contains('-'))
                {
                    webServiceResponse_part1 = webServiceResponse.Split('-')[0];
                    webServiceResponse_part2 = webServiceResponse.Split('-')[1];
                }
                else
                {
                    webServiceResponse_part1 = webServiceResponse;
                }
                var _resultado = String.Empty;
                var _descripcion = String.Empty;
                var _refWeb = String.Empty;
                if (webServiceResponse_part1.Contains(':') && !webServiceResponse_part1.ToLower().Replace(" ", String.Empty).Contains("refweb"))
                {
                    _resultado = webServiceResponse_part1.Split(":")[0];
                    _descripcion = webServiceResponse_part1.Split(":")[1];
                }

                if (webServiceResponse_part1.Contains(':') && webServiceResponse_part1.ToLower().Replace(" ", String.Empty).Contains("refweb"))
                    _refWeb = webServiceResponse_part1.Split(":")[1];

                if (!String.IsNullOrEmpty(webServiceResponse_part2) && webServiceResponse_part2.Contains(':') && webServiceResponse_part2.ToLower().Replace(" ", String.Empty).Contains("refweb"))
                    _refWeb = webServiceResponse_part2.Split(":")[1];

                result = new EnvioSmsResponse { 
                    descripcion = _descripcion,
                    refWeb = _refWeb,
                    resultado = _resultado
                };

            }

            return result;
        }
    }
}
