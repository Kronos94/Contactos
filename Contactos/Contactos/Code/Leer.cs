using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contactos.Code
{
    class Leer
    {
        public static List<Contacto> LeerArchivo(String ruta)
        {
            
            StreamReader objReader = new StreamReader(GenerateStreamFromString(ruta));
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
            return arrText;
        }

        public static Stream GenerateStreamFromString(string s)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}
