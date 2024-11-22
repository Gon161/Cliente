using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cliente
{
    public class Log
    {
        private static volatile Log oInstancia = null; // Variable estática que mantiene la instancia única de la clase Log, marcada como volátil para asegurar su visibilidad entre hilos.
        private static readonly object objLock = new object(); // Objeto para sincronizar el acceso a la instancia de Log en un entorno multihilo.

        // Propiedad estática que obtiene la única instancia de la clase Log (patrón Singleton)
        public static Log Instancia
        {
            get
            {
                if (oInstancia == null) // Si la instancia aún no ha sido creada
                {
                    lock (objLock) // Bloquea el acceso para asegurar que solo un hilo cree la instancia
                    {
                        if (oInstancia == null) // Verifica nuevamente después de haber adquirido el bloqueo
                            oInstancia = new Log(); // Crea la instancia de la clase Log
                    }
                }
                return oInstancia; // Devuelve la instancia única
            }
        }

        // Método que escribe un mensaje en el archivo de log
        public void LogWrite(string sMsj)
        {
            try
            {
                lock (objLock) // Bloquea el acceso al bloque de código para garantizar que solo un hilo escriba en el archivo a la vez
                {
                    // Define el nombre del archivo de log, basado en la fecha actual, para crear un archivo por día.
                    string sPathLogDefault = string.Format(@".\Log - {0}.txt", DateTime.Now.ToString("dd-MM-yyyy"));

                    // Abre el archivo de log en modo de adición (true), para agregar nuevas entradas al final del archivo
                    using (StreamWriter oLogWriter = new StreamWriter(sPathLogDefault, true))
                        // Escribe el mensaje en el archivo, formateado con la fecha y hora actuales
                        oLogWriter.WriteLine(string.Format("[{0}] - {1}", DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"), sMsj));
                }
            }
            catch (Exception) // Captura cualquier excepción que pueda ocurrir al escribir en el archivo de log
            {
                // No se realiza ninguna acción si ocurre una excepción (vacío)
            }
        }
    }
}