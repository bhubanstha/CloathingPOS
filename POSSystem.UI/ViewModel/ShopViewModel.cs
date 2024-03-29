﻿using log4net;
using Notifications.Wpf;
using POS.BusinessRule;
using POS.Model;
using POS.Utilities;
using POSSystem.UI.Service;
using POSSystem.UI.Wrapper;
using Prism.Commands;
using System;
using System.IO;
using System.Windows.Input;

namespace POSSystem.UI.ViewModel
{
    public class ShopViewModel : NotifyPropertyChanged
    {
        private ILog _log;
        private string _buttonText= "Save";
        private string _logoFullPathName;
        private string _logoName;
        private bool _isLogoChanged = false;

        public string ButtonText
        {
            get { return _buttonText; }
            set {
                _buttonText = value;
                OnPropertyChanged();
            }
        }

        public string LogoFullPathName
        {
            get { return _logoFullPathName; }
            set {
                _logoFullPathName = value;
                OnPropertyChanged();
            }
        }

        public string LogoName
        {
            get { return _logoName; }
            set { _logoName = value; OnPropertyChanged(); }
        }

        public ShopWrapper ShopWrapper { get; set; }
        public ICommand FilePickCommand { get; }
        public ICommand SaveCommand { get; }

        public ShopViewModel(ILogger logger)
        {
            _log = logger.GetLogger(typeof(ShopViewModel));
            FilePickCommand = new DelegateCommand(OnFilePickCommandExecute);
            SaveCommand = new DelegateCommand(OnSaveCommandExecute);
            ShopWrapper = new ShopWrapper(new Shop());

            LoadShopInfo();
        }

        private async void OnSaveCommandExecute()
        {
            try
            {
                ShopBO bo = new ShopBO();
                if(_isLogoChanged)
                {
                    FileUtility.SaveLogoFile(LogoFullPathName, ShopWrapper.LogoPath);
                }
                if (ShopWrapper.Id > 0)
                {
                     await bo.UpdateShop(ShopWrapper.Model);
                }
                else
                {
                     await bo.SaveShop(ShopWrapper.Model);
                }
                StaticContainer.ShowNotification("Shop Created", "Shop information successfully added", NotificationType.Success);
            }
            catch (Exception ex)
            {
                _log.Error("OnSaveCommandExecute", ex);
                StaticContainer.ShowNotification("Error", StaticContainer.ErrorMessage, NotificationType.Error);
            }
            finally
            {
                StaticContainer.UpdateShop(ShopWrapper.Model);
            }
        }

        private void OnFilePickCommandExecute()
        {
            string fileName = FileUtility.OpenImageFilePicker();
            if(!string.IsNullOrEmpty(fileName))
            {
                ShopWrapper.LogoPath = Path.GetFileName(fileName);
                LogoName = $".....\\{ShopWrapper.LogoPath}";
                LogoFullPathName = fileName;
                _isLogoChanged = true;
            }
        }


        private void LoadShopInfo()
        {
            if (StaticContainer.Shop != null)
            {
                ShopWrapper = new ShopWrapper(new Shop())
                {
                    Id = StaticContainer.Shop.Id,
                    Name = StaticContainer.Shop.Name,
                    PANNumber = StaticContainer.Shop.PANNumber,
                    LogoPath = StaticContainer.Shop.LogoPath,
                    CalculateVATOnSales = StaticContainer.Shop.CalculateVATOnSales,
                    PrintInvoice = StaticContainer.Shop.PrintInvoice
                };
                LogoFullPathName = FilePath.GetLogoFullPath(ShopWrapper.LogoPath);
                LogoName = $".....\\{ShopWrapper.LogoPath}";
                ButtonText = "Update";
            }
            else
            {
                ShopWrapper = new ShopWrapper(new Shop());
            }

        }

        

        
    }
}
