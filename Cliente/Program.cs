using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cliente
{
    internal class Program
    {
        static void Main(string[] args)
        {
            SocketCliente peticion = new SocketCliente("192.168.1.101", 8381);
            peticion.Connect();
            List<int> lista = new List<int> { 1,2, 3 };

            Request request = new Request();
            request.Metodo = "Suma";
            request.Parametros = Newtonsoft.Json.JsonConvert.SerializeObject(lista);    

            peticion.Send(Newtonsoft.Json.JsonConvert.SerializeObject(request));
            string a = peticion.Recived();
            Console.ReadKey();
        }
    }
}
