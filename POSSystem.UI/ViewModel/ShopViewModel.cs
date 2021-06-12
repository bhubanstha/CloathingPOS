using Microsoft.Win32;
using Notifications.Wpf;
using POS.BusinessRule;
using POS.Model;
using POSSystem.UI.Service;
using POSSystem.UI.Wrapper;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace POSSystem.UI.ViewModel
{
    public class ShopViewModel : NotifyPropertyChanged
    {

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

        public ShopViewModel()
        {

            FilePickCommand = new DelegateCommand(OnFilePickCommandExecute);
            SaveCommand = new DelegateCommand(OnSaveCommandExecute);
            LoadShopInfo();
        }

        private async void OnSaveCommandExecute()
        {
            try
            {
                ShopBO bo = new ShopBO();
                if(_isLogoChanged)
                {
                    SaveLogoFile();
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
                StaticContainer.ShowNotification("Error", StaticContainer.ErrorMessage, NotificationType.Error);
            }
        }

        private void OnFilePickCommandExecute()
        {
            OpenFileDialog opfd = new OpenFileDialog();
            opfd.Filter = "Image Files(*.BMP;*.JPG;*.PNG)|*.BMP;*.JPG;*.PNG";
            opfd.FilterIndex = 1;
            opfd.RestoreDirectory = true;
            if (opfd.ShowDialog() == true)
            {
                string fileName = opfd.FileName;
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
                    Address = StaticContainer.Shop.Address,
                    PANNumber = StaticContainer.Shop.PANNumber,
                    LogoPath = StaticContainer.Shop.LogoPath,
                    CalculateVATOnSales = StaticContainer.Shop.CalculateVATOnSales,
                    PrintInvoice = StaticContainer.Shop.PrintInvoice
                };
                LogoFullPathName = LogoPath(ShopWrapper.LogoPath);
                LogoName = $".....\\{ShopWrapper.LogoPath}";
                ButtonText = "Update";
            }
            else
            {
                ShopWrapper = new ShopWrapper(new Shop());
            }

        }

        void SaveLogoFile()
        {
            try
            {
                string logoPath = LogoPath("");// Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Image", "CompanyLogo");
                CleanupDirectory(logoPath);
                if (!Directory.Exists(logoPath))
                {
                    Directory.CreateDirectory(logoPath);
                }
                File.Copy(LogoFullPathName, Path.Combine(logoPath, ShopWrapper.LogoPath));
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        void CleanupDirectory(string directoryPath)
        {
            try
            {
                if (Directory.Exists(directoryPath))
                {
                    string[] files = Directory.GetFiles(directoryPath);
                    foreach (string file in files)
                    {
                        File.Delete(file);
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        string LogoPath(string logoName)
        {
            string logoPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Image", "CompanyLogo", logoName);
            return logoPath;
        }
    }
}
