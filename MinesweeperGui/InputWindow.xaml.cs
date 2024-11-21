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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MinesweeperGui
{
    /// <summary>
    /// Interaction logic for InputWindow.xaml
    /// </summary>
    public partial class InputWindow : Window
    {
        public string PlayerName { get; set; }

        public InputWindow()
        {
            InitializeComponent();
            PlayerName = "";
        }

        private void BtnOkClick(object sender, RoutedEventArgs e)
        {
            PlayerName = TxtName.Text;
            Close();
        }
    }
}
