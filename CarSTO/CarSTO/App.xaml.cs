using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using CarSTO.Views;

namespace CarSTO
{
    
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            
            LoginWindow lw = new LoginWindow();

            //ProductsWindow lw = new ProductsWindow();

            //MainWindowViewModel vm = new MainWindowViewModel();
            //mv.DataContext = vm;

            lw.Show();
        }

    }
}
