using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Contactos.Code;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Contactos.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainView : ContentPage
    {
        public MainView()
        {
            InitializeComponent();
            seleccionFichero();

        }

        List<Contacto> contactos; //Lista de contactos que obtendremos del fichero que el usuario debe indicar
        List<Contacto> contactosMostrar = new List<Contacto>(); //Lista con los contactos filtrados.
        List<Contacto> listaVacia; //Lista vacia para limpiar la listview.

        private void seleccionFichero()
        {

            //Cargar datos del fichero en un array
            contactos = Leer.LeerArchivo(Properties.Resources.Info_txt);

            lstContactos.ItemsSource = contactos;
        }

      

        /// <summary>
        /// Realiza la busqueda de los contactos.
        /// </summary>
        private void realizarBusqueda()
        {
            //Limpiamos los contactos cargados para volver a cargar los correctos
            contactosMostrar.Clear();
            //Se obtiene resultado de la busqueda con los valores introducidos y se carga en listView
            buscarContacto(txtBusqueda.Text, txtMaxEdad.Text, txtMinEdad.Text);
            //Se rellena listView
            cargarListView();
        }


        /// <summary>
        /// Busca el contacto segun la edad y el nombre introducidos.
        /// </summary>
        /// <param name="txtBusqueda">Parametro para el filtro de busqueda por nombre.</param>
        /// <param name="txtMaxEdad">Parametro para la edad máxima del contacto.</param>
        /// <param name="txtMinEdad">Parametro para la edad mínima del contacto.</param>

        private void buscarContacto(string txtBusqueda, string txtMaxEdad, string txtMinEdad)
        {
         
            if((txtMaxEdad == null && txtMinEdad == null) || (txtMaxEdad.Equals("") && txtMinEdad.Equals("")))
            {
                comprobarNombre(txtBusqueda);

            } else if (txtMinEdad == null || txtMinEdad.Equals(""))
            {
                comprobarNombre(txtBusqueda);
                comprobarEdad(txtMaxEdad, false);

            } else if (txtMaxEdad == null || txtMaxEdad.Equals(""))
            {
                comprobarNombre(txtBusqueda);
                comprobarEdad(txtMinEdad, true);

            } else if (txtBusqueda == null || txtBusqueda.Equals(""))
            {
                comprobarNombre(txtBusqueda);
                comprobarEdad(txtMinEdad, txtMaxEdad);

            } else
            {
                comprobarNombre(txtBusqueda);
                comprobarEdad(txtMinEdad, txtMaxEdad);
            }

            
        }

        /// <summary>
        /// Limpia la lista y la vuelve a cqrgar con los datos de la busqueda.
        /// </summary>
        private void cargarListView()
        {
            lstContactos.ItemsSource = listaVacia;

            lstContactos.ItemsSource = contactosMostrar;
        }


        /// <summary>
        /// Filtra por nombre de contacto en la lista principal del archivo introducido.
        /// </summary>
        /// <param name="filtro">Es una cadena con la que se filtrará el nombre del contacto.</param>
        public void comprobarNombre(string filtro)
        {
            Boolean ok = false;

            Contacto[] array = new Contacto[contactos.Count()];

            contactos.CopyTo(array);

            string filtrado, compCadena, nombreLower;

            int numApar=0;

            bool masPorc = false;

            /// Si el filtro es nullo lo ponemos a cadena vacia.
            if(filtro == null)
            {
                filtro = "";
            }

            compCadena = filtro;

            //Comprueba dos veces si hay mas de un % en el filtro.
            for (int i = 0; i<2; i++)
            {
                
                if (compCadena.LastIndexOf("%") >= 0)
                {

                    compCadena = compCadena.Remove(compCadena.LastIndexOf("%"));
 
                    numApar++;
                    if(numApar >= 2)
                    {
                        masPorc = true;
                    }
                    
                }

            }



            if (!masPorc)
            {
                ///Filtrado normal si acaba en %, solo hay un % o esta vacia la cadena.
                if (filtro.EndsWith("%")|| filtro.Equals(""))
                {
      
                    filtrado = filtro.Replace("%", "").ToLower();

                    for (int i = 0; i < contactos.Count(); i++)
                    {
                        nombreLower = array[i].Nombre.ToLower();

                        if (nombreLower.StartsWith(filtrado))
                        {
                            contactosMostrar.Add(array[i]);

                        };


                    }
                }

                //Filtra si no se ha introducido ningun porcentaje.
                else if (filtro.IndexOf("%") == -1)
                {
                    filtrado = filtro.ToLower();

                    for (int i = 0; i < contactos.Count(); i++)
                    {
                        nombreLower = array[i].Nombre.ToLower();

                        if (nombreLower.Equals(filtrado))
                        {
                            contactosMostrar.Add(array[i]);

                        };


                    }
                } 
            }
            else
            {
                lblError.Text = "Hay mas de un porcentaje.";
            }

                



         
        }


        /// <summary>
        /// Metodo que comprueba las edades del contacto teniendo una edad mínima y una máxima.
        /// </summary>
        /// <param name="edadMin">Parametro para la edad minima que se quiera filtrar.</param>
        /// <param name="edadMax">Parametro para la edad máxima que se quiera filtrar.</param>
        public void comprobarEdad(string edadMin, string edadMax)
        {

            Contacto[] array = new Contacto[contactosMostrar.Count()];

            string numEdad;

            int entEdadMax, entEdadMin;

            contactosMostrar.CopyTo(array);

            contactosMostrar.Clear();

            //Comprueba que se ha introducido un número y no un caracter erroneo.
            if (int.TryParse(edadMax, out entEdadMax) && int.TryParse(edadMin, out entEdadMin) && entEdadMax>entEdadMin)
            {
                for (int i = 0; i < array.Length; i++)
                {
                    numEdad = array[i].Edad;
                    if (int.Parse(numEdad) >= entEdadMin && int.Parse(numEdad) <= entEdadMax)
                    {
                        contactosMostrar.Add(array[i]);

                    };


                } 
            } else
            {
                lblError.Text = "Alguno de los datos no son validos.";

            }
        }

        /// <summary>
        /// Parametro que comprueba la edad del contacto siendo máxima o mínima para posteriormente filtrar la lista.
        /// </summary>
        /// <param name="edad">Edad maxima o minima por la que se quiera filtrar el contacto.</param>
        /// <param name="minEdad">Parametro para saber si es la edad minima o la maxima la que se ha introducido.</param>
        public void comprobarEdad(string edad, Boolean minEdad)
        {
            Contacto[] array = new Contacto[contactosMostrar.Count()];

            string numEdad;

            int entEdad;

            contactosMostrar.CopyTo(array);

            contactosMostrar.Clear();

            //Comprueba que se ha introducido un número y no un caracter erroneo.
            if (int.TryParse(edad, out entEdad))
            {
                for (int i = 0; i < array.Length; i++)
                {
                    numEdad = array[i].Edad;
                    if (minEdad && int.Parse(numEdad) >= entEdad)
                    {
                        contactosMostrar.Add(array[i]);

                    }
                    else if (!minEdad && int.Parse(numEdad) <= int.Parse(edad))
                    {

                        contactosMostrar.Add(array[i]);

                    }


                } 
            }
            else
            {
                lblError.Text = "Alguno de los datos no son validos.";

            }
        }
        /// <summary>
        /// Metodo para controlar el evento click del botón.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBuscar_Clicked(object sender, EventArgs e)
        {
            realizarBusqueda();
        }
    }  
}