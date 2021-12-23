using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Servicios
{
    public class Funciones
    {
        public static void Logs(string nombre_archivo,string descripcion)
        {
            string directorio = AppDomain.CurrentDomain.BaseDirectory + "logs/" +
                DateTime.Now.Year.ToString() + "/" +
                DateTime.Now.Month.ToString() + "/" +
                DateTime.Now.Day.ToString();

            if (!Directory.Exists(directorio)){
                Directory.CreateDirectory(directorio);
            }
            StreamWriter mi_archivo = new StreamWriter(directorio + "/" + nombre_archivo + ".txt", true);

            string cadena = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+">>>"+descripcion;
            mi_archivo.WriteLine(cadena);
            mi_archivo.Close();

        }
      
    }
}