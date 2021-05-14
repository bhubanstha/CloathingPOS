﻿using Autofac;
using POSSystem.UI.Service;
using POSSystem.UI.ViewModel;
using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace POSSystem.UI.Views
{
    /// <summary>
    /// Interaction logic for GraphView.xaml
    /// </summary>
    public partial class GraphView : UserControl
    {
        LineSeriesViewModel model;
        public GraphView()
        {
            InitializeComponent();
            model  = new LineSeriesViewModel(new ColorService());
            this.DataContext = model;

            this.Loaded += GraphView_Loaded;
        }

        private void GraphView_Loaded(object sender, RoutedEventArgs e)
        {
            model.AnimateAsync();
        }
    }
}
