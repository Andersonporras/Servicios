using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml;
using Newtonsoft.Json;

namespace Servicios
{
    /// <summary>
    /// Descripción breve de Servicios
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    // [System.Web.Script.Services.ScriptService]
    public class Servicios : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hola a todos";
        }

        [WebMethod(Description = "Saluda a la persona")]
        public string Saludar(string nombre)
        {
            return "Hola " + nombre;
        }

        [WebMethod]
        public string Guardarlog(string mensaje)
        {
            Funciones.Logs("Anderson", mensaje);
            return "ok";
        }

        [WebMethod]
        public string Logs(string nombrearchivo, string contenidoarchivo)
        {
            Funciones.Logs(nombrearchivo, contenidoarchivo);
            return "ok";
        }

        [WebMethod]
        public int sumar(int numero1, int numero2)
        {
            return numero1 + numero2;
        }

        [WebMethod]
        public string[] Obrtenerfrutas()
        {
            string[] frutas = new string[3];

            frutas[0] = "manzana";
            frutas[1] = "Pera";
            frutas[2] = "Limon";

            GuardarFrutas(frutas);

            return frutas;
        }

        [WebMethod]
        public string GuardarFrutas(string[] frutas)
        {
            foreach (string fruta in frutas)
            {
                Funciones.Logs("frutas", fruta);
            }
            return "proceso realizado con exito";
        }

        [WebMethod]
        public List<Equipos> ObtenerEquipos()
        {
            List<Equipos> equipos = new List<Equipos>();
            equipos.Add(new Equipos { Nombre = "Milan", Pais = "Italia" });
            equipos.Add(new Equipos { Nombre = "AJAX", Pais = "Holanda" });

            GuardarEquipos(equipos.ToArray());

            return equipos;
        }

        [WebMethod]
        public string GuardarEquipos(Equipos[] listaequipos)
        {
            foreach (Equipos equipo in listaequipos)
            {
                Funciones.Logs("equipos", equipo.Nombre + " - " + equipo.Pais);
            }
            return "proceso realizado con exito";

        }

        [WebMethod]
        public string createxml()
        {

            string result2 = GuardarXML("<?xml version='1.0' encoding='UTF-8'?><documento><deporte><![CDATA[Futbol]]>" +
                "</deporte><equipos><equipo><nombre><![CDATA[Ajax]]></nombre><pais><![CDATA[Holanda]]></pais>" +
                "</equipo><equipo><nombre><![CDATA[Valencia]]></nombre><pais><![CDATA[España]]></pais></equipo></equipos>" +
                "<equipos><equipo><nombre><![CDATA[ander]]></nombre><pais><![CDATA[porras]]></pais></equipo><equipo>" +
                "<nombre><![CDATA[mary]]></nombre><pais><![CDATA[valencia]]></pais></equipo></equipos></documento>");

            return "xml almacenado con exito";
        }



        [WebMethod]
        public string GuardarXML(string xml)
        {
            XmlDocument data = new XmlDocument();
            data.LoadXml(xml);
            XmlNode documento = data.SelectSingleNode("documento");
            string deporte = documento["deporte"].InnerText;
            Funciones.Logs("xml", "Deporte: " + deporte + "; Equipos : ");
            XmlNodeList equipos_nodes = data.GetElementsByTagName("equipos");
            XmlNodeList equipos = ((XmlElement)equipos_nodes[0]).GetElementsByTagName("equipo");
            XmlNodeList equiposdos = ((XmlElement)equipos_nodes[1]).GetElementsByTagName("equipo");

            foreach (XmlElement equipo in equipos)
            {
                string nombre = equipo.GetElementsByTagName("nombre")[0].InnerText;
                string pais = equipo.GetElementsByTagName("pais")[0].InnerText;
                Funciones.Logs("XML", nombre + " " + pais);
            }

            foreach (XmlElement equipo in equiposdos)
            {
                string nombre = equipo.GetElementsByTagName("nombre")[0].InnerText;
                string pais = equipo.GetElementsByTagName("pais")[0].InnerText;
                Funciones.Logs("XML", nombre + " " + pais);
            }

            return "proceso realizado con exito";

        }

        [WebMethod]
        public string RetornarJson()
        {
            dynamic json = new Dictionary<string, dynamic>();
            json.Add("deporte", "Futbol");
            List<Dictionary<string, string>> equiposjson = new List<Dictionary<string, string>>();

            Dictionary<string, string> equipo1 = new Dictionary<string, string>();
            equipo1.Add("nombre", "manchester");
            equipo1.Add("Pais", "Inglaterra");
            equiposjson.Add(equipo1);

            Dictionary<string, string> equipo2 = new Dictionary<string, string>();
            equipo2.Add("nombre", "Valencia");
            equipo2.Add("Pais", "España");
            equiposjson.Add(equipo2);

            json.Add("equipos", equiposjson);
            
           
            GuardarJson(JsonConvert.SerializeObject(json));

            return JsonConvert.SerializeObject(json);
        }

        [WebMethod]
        public string GuardarJson(string json)
        {
            var data_json = JsonConvert.DeserializeObject<DataJson>(json);

            Funciones.Logs("Json", "Deporte :" + data_json.deporte + " ; Equipos : ");
            foreach (var equipo in data_json.equipos)
            {
                Funciones.Logs("json", equipo.Nombre + " -- " + equipo.Pais);
            }

            return "proceso realizado con exito";

        }

        [WebMethod]
        public string ObtenerProductos()
        {
            List<Dictionary<string, string>> json = new List<Dictionary<string, string>>();
            if (!EnlaceSqlServer.ConnectarSqlServer())
            {
                return "";
            }
            try
            {
                SqlCommand com = new SqlCommand("select * from productos",EnlaceSqlServer.conexion);
                com.CommandType = CommandType.Text;
                com.CommandTimeout = DatosEnlace.timeoutsqlserver;
                SqlDataReader record = com.ExecuteReader();
                if (record.HasRows)
                {
                    Dictionary<string, string> row;
                    while (record.Read())
                    {
                        row = new Dictionary<string, string>();
                        for (int f= 0; f < record.FieldCount; f++)
                        {
                            row.Add(record.GetName(f),record.GetValue(f).ToString());
                        }
                        json.Add(row);
                    }
                }
                record.Close();
                record.Dispose();//libera memoria
                record = null;
                com.Dispose();

            }
            catch (Exception e)
            {
                Funciones.Logs("Obtenerproductos", e.Message);
                Funciones.Logs("Obtenerproductos_DEBUG", e.StackTrace);
            }

            return JsonConvert.SerializeObject(json);
        }

        [WebMethod]
        public Producto ObtenerProducto(int idproducto)
        {
            Producto producto = new Producto();
            producto.idproducto = 0;
            producto.nombre = "";
            producto.precio = 0;
            producto.stock = 0;
            if (!EnlaceSqlServer.ConnectarSqlServer())
            {
                return producto;
            }
            try {
                SqlCommand com = new SqlCommand("select top 1 * from productos where idproducto = " + idproducto, EnlaceSqlServer.conexion);
                com.CommandType = CommandType.Text;
                com.CommandTimeout = DatosEnlace.timeoutsqlserver;
                SqlDataReader record = com.ExecuteReader();
                if (record.HasRows && record.Read ())
                {
                    producto.idproducto = int.Parse(record.GetValue(0).ToString());
                    producto.nombre = record.GetValue(1).ToString();
                    producto.precio = double.Parse(record.GetValue(2).ToString());
                    producto.stock = int.Parse(record.GetValue(3).ToString());
                }
                record.Close();
                record.Dispose();
                record = null;
                com.Dispose();
            }
            catch (Exception e)
            {
                Funciones.Logs("ObtenerProducto", e.Message);
                Funciones.Logs("ObtenerProducto_debug", e.StackTrace);
            }

            return producto;
        }


        [WebMethod]
        public string ActualizarProducto(Producto producto)
        {
            string result = "";

            if (!EnlaceSqlServer.ConnectarSqlServer())
            {
                return "";
            }

            try
            {
                SqlCommand com = new SqlCommand("Update productos set nombre = @Nombre, precio = @precio,"
                    + "stock = @stock where idproducto = @idProducto" ,EnlaceSqlServer.conexion);

                com.Parameters.AddWithValue("@Nombre", producto.nombre);
                com.Parameters.AddWithValue("@precio", producto.precio);
                com.Parameters.AddWithValue("@Stock",  producto.stock);
                com.Parameters.AddWithValue("@idProducto",  producto.idproducto);

                int cant = com.ExecuteNonQuery();

                if (cant == 1)
                {
                    result = "Producto actualizado con exito";
                }
                else
                {
                    result = "Error al actualizar el producto";
                }

                com.Dispose();


             }
            catch (Exception e)
            {

                Funciones.Logs("ActualizarProducto", e.Message);
                Funciones.Logs("ActualizarProducto_Debug", e.StackTrace);
            }

            return result;
        }
        [WebMethod]
        public int GuardarProducto(Producto producto)
        {
            int idproducto = -1;

            if (!EnlaceSqlServer.ConnectarSqlServer())
            {
                return 0;
            }

            try
            {
                SqlCommand com = new SqlCommand("insert into productos (nombre,precio,stock) " +
                                               "values(@nombre,@precio,@stock)" +
                                               "; select  cast(scope_identity() as int)", EnlaceSqlServer.conexion);
                com.Parameters.AddWithValue("@nombre", producto.nombre);
                com.Parameters.AddWithValue("@precio", producto.precio);
                com.Parameters.AddWithValue("@stock", producto.stock);

                idproducto = (int)com.ExecuteScalar();

                com.Dispose();
            }
            
            catch (Exception e)
            {
                Funciones.Logs("insertarproducto", e.Message);
                Funciones.Logs("insertarproducto_DEBUG", e.StackTrace);
            }
            
            return idproducto;
        }

        [WebMethod]
        public string EliminarRegistro(int idproducto)
        {

            string result = "";

            if (!EnlaceSqlServer.ConnectarSqlServer())
            {
                return "";
            }

            try
            {   //de ambas manera funciona
                //SqlCommand com = new SqlCommand("delete productos where idproducto = " + idproducto, EnlaceSqlServer.conexion);
                SqlCommand com = new SqlCommand("delete productos where idproducto = @idproducto ", EnlaceSqlServer.conexion);
                com.Parameters.AddWithValue("@idproducto", idproducto);

                int cant = com.ExecuteNonQuery();

                if (cant == 1)
                {
                    result = "Producto eliminado con exito";
                }
                else
                {
                    result = "Error al eliminar el producto";
                }

                com.Dispose();
            }
            catch (Exception e)
            {
                Funciones.Logs("eliminarproducto", e.Message);
                Funciones.Logs("eliminarproducto_debug", e.StackTrace);
                
            }

            return result;
        }
        public AuthUser User;

        [WebMethod]
        [SoapHeader("User")]
        public string ObtenerFecha()
        {
            if (this.User != null && this.User.IsValid())
            {
                return DateTime.Now.Year.ToString()+"-"+DateTime.Now.Month.ToString()+"-"+DateTime.Now.Date.ToString()+"Correcto";
            }
            else
            {
                return "credenciales incorrectas";
            }
           
        }
        [WebMethod]
        public string ObtenerHora()
        {
            return DateTime.Now.Hour.ToString()+":"+DateTime.Now.Minute.ToString()+":"+DateTime.Now.Second.ToString();
        }
    }
}
