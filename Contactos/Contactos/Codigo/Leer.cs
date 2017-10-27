using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contactos.Codigo
{
    class Leer
    {
        public static List<Contacto> LeerArchivo(String ruta)
        {
            var assembly = typeof(LoadResourceText).GetTypeInfo().Assembly;
            Stream stream = assembly.GetManifestResourceStream(ruta);
            StreamReader objReader = new StreamReader(stream);
            List<Contacto> arrText = new List<Contacto>();
            String edad;
            String nombre;
            String dni;
            do
            {

                nombre = objReader.ReadLine();
                edad = objReader.ReadLine();
                dni = objReader.ReadLine();
                if (nombre != null && edad != null && dni != null)
                    arrText.Add(new Contacto(nombre, edad, dni));

            } while (nombre != null && edad != null && dni != null);
            objReader.Close();
            return arrText;
        }
    }
}
