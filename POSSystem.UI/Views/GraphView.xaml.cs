﻿using Autofac;
using POS.Model;
using POSSystem.UI.Service;
using POSSystem.UI.ViewModel;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace POSSystem.UI.Views
{
    /// <summary>
    /// Interaction logic for GraphView.xaml
    /// </summary>
    public partial class GraphView : UserControl
    {
        GraphViewModel model;
        public GraphView()
        {
            InitializeComponent();
            model = new GraphViewModel(new ColorService());
            this.DataContext = model;
            this.Loaded += GraphView_Loaded;
        }

        private void GraphView_Loaded(object sender, RoutedEventArgs e)
        {
            //model.AnimateAsync();
        }

        private void Txt_OnProductNameChaange(object sender, TextChangedEventArgs e)
        {
            TextBox inp = sender as TextBox;
            model.FilterProducts = model.Products.Where(p => p.Name.ToLower().StartsWith(inp.Text.ToLower())).ToList();
            txtProduct.AutoCompleteItemSource = model.FilterProducts;
        }

        private void Txt_ProductSelected(object sender, EventArgs e)
        {
            if (txtProduct.SelectedItem != null)
            {
                model.SelectedItem = txtProduct.SelectedItem as Inventory;
                model.CreateGraphModel();
                graph.InvalidatePlot();
                //var pr = txtProduct.SelectedItem as Inventory;
                //model.CurrentProduct.Inventory = pr;
                //model.CurrentProduct.Rate = pr.RetailRate;
                //txtRetailRate.Value = (double)model.CurrentProduct.Rate;
                //txtSalesQty.Maximum = (double)model.CurrentProduct.Inventory.Quantity;
                //txtProduct.Text = pr.Name + " - " + pr.Size;
            }
        }
    }
}
