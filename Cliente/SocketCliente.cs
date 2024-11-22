using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Cliente
{
    public class SocketCliente:IDisposable
    {
        public const int BUFFER_SIZE = 65535;

        private TcpClient oCliente = null;

        public string IP { get; set; }

        public int PORT { get; set; }

        public SocketCliente(string sIP, int iPort)
        {
            IP = sIP;
            PORT = iPort;
        }

        public void Connect()
        {
            try
            {
                oCliente = new TcpClient(IP, PORT);
            }
            catch (Exception ex)
            {
                Log.Instancia.LogWrite("SocketCliente.Connect "+ ex);
                throw;
            }
            finally
            {
            }
        }

        public void Send(string sDato)
        {
            byte[] array = new byte[0];
            try
            {
                string arg = Encriptado.Encrypt_AES(sDato);
                array = Encoding.ASCII.GetBytes($"{arg}<EOF>");
                Log.Instancia.LogWrite("SocketCliente.Send: " + array.Length);
                oCliente.Client.Send(array);
            }
            catch (Exception ex)
            {
                Log.Instancia.LogWrite("SocketCliente.Send "+ ex);
                throw;
            }
            finally
            {
            }
        }

        public string Recived()
        {
            byte[] array = null;
            int num = 0;
            StringBuilder stringBuilder = null;
            try
            {
                array = new byte[65535];
                num = oCliente.Client.Receive(array);
                stringBuilder = new StringBuilder();
                stringBuilder.Append(Encoding.ASCII.GetString(array, 0, num));
                while (num > 0)
                {
                    num = oCliente.Client.Receive(array, array.Length, SocketFlags.None);
                    stringBuilder.Append(Encoding.ASCII.GetString(array, 0, num));
                }

                string sMsjEncrypt = stringBuilder.ToString();
                return Encriptado.Desencrypt_AES(sMsjEncrypt);
            }
            catch (Exception ex)
            {
                Log.Instancia.LogWrite($"SocketCliente.Recived: {ex.Message} | {ex.StackTrace}");
                return string.Empty;
            }
            finally
            {
                if (stringBuilder.Length > 1)
                {
                    stringBuilder.Clear();
                }

                Array.Clear(array, 0, array.Length);
                Close();
            }
        }

        private void Close()
        {
            if (oCliente != null && oCliente.Connected)
            {
                oCliente.Client.Shutdown(SocketShutdown.Both);
                oCliente.Close();
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Close();
            }
        }

        ~SocketCliente()
        {
            Dispose(disposing: false);
        }
    }
}
