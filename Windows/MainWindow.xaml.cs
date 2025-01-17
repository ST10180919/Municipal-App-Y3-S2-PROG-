﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Municipal_App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Border_MouseEnter(object sender, MouseEventArgs e)
        {
            InputsGrid.Visibility = Visibility.Visible;

            var storyboard = (Storyboard)this.FindResource("ExpandInputsGrid");
            storyboard.Begin(); // Begin the expand animation
        }

        private void Border_MouseLeave(object sender, MouseEventArgs e)
        {
            var storyboard = (Storyboard)this.FindResource("CollapseInputsGrid");
            storyboard.Completed += (s, ev) => InputsGrid.Visibility = Visibility.Collapsed; // Hide after collapse
            storyboard.Begin(); // Begin the collapse animation
        }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Allows the user to drag the Window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Header_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Minimizes the application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonMinimize_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Changes the window state Maximise to minimise and vice versa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonChangeWindowState_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow.WindowState != WindowState.Maximized)
            {
                Application.Current.MainWindow.WindowState = WindowState.Maximized;
            }
            else
            {
                Application.Current.MainWindow.WindowState = WindowState.Normal;
            }
        }

        //-----------------------------------------------------------------------------
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
