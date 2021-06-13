﻿using POS.Model;
using POS.Utilities.Extension;
using System;
using System.ComponentModel.DataAnnotations;
namespace POSSystem.UI.Wrapper
{
    public class InventoryWrapper : WrapperBase<Inventory>
    {


        private bool _ColorNameEntryEnabled = false;

        public InventoryWrapper(Inventory obj) : base(obj)
        {
        }

        public Int64 Id
        {
            get { return GetValue<Int64>(); }
            set { SetValue(value); }
        }



        
        public DateTime FirstPurchaseDate 
        {
            get { return GetValue<DateTime>(); }
            set { SetValue(value); }
        }



        
        public decimal PurchaseRate
        {
            get { return GetValue<decimal>(); }
            set { SetValue(value); }
        }


       
        public decimal RetailRate
        {
            get { return GetValue<decimal>(); }
            set { SetValue(value); }
        }


        
        public string Name
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }


        
        public string Size
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }


       
        public string Color
        {
            get { return GetValue<string>(); }
            set { SetValue(value); GetKnownColorName(); }
        }


        
        public string ColorName
        {
            get { return GetValue<string>(); }
            set { SetValue(value);  }
        }


       
        public int Quantity
        {
            get { return GetValue<int>(); }
            set { SetValue(value); }
        }


        
        public Int64 CategoryId
        {
            get { return GetValue<Int64>(); }
            set { SetValue(value); }
        }

        public bool ColorNameEntryEnabled
        {
            get { return _ColorNameEntryEnabled; }
            set { _ColorNameEntryEnabled = value; OnPropertyChanged(); }
        }


        private void GetKnownColorName()
        {
            string colorName = Color.ToKnownColourName();
            if (string.IsNullOrEmpty(colorName))
            {
                ColorNameEntryEnabled = true;
                ColorName = "";
            }
            else
            {
                ColorNameEntryEnabled = false;
                ColorName = colorName;
            }
        }
    }
}
