﻿using Municipal_App.Services.DatabaseServices;
using Municipal_App.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Municipal_App
{
    //---------------------------------------------------------------------------------
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            this.MainWindow = new MainWindow();
            this.MainWindow.DataContext = new MainViewModel();
            this.MainWindow.Show();

            // Test Database
            //ReportIssuesDataService.TestDatabase();

            base.OnStartup(e);
        }
    }
}
