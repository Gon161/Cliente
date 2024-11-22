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
            peticion.Send("Hola");
            string a = peticion.Recived();
            Console.ReadKey();
        }
    }
}
