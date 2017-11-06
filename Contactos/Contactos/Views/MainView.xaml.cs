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



        private void seleccionFichero()
        {

            //Cargar datos del fichero en un array
            contactos = Leer.LeerArchivo(Properties.Resources.Info_txt);

            lstContactos.ItemsSource = contactos;
        }

        
    }  
}