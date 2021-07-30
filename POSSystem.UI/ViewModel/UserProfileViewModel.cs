using Notifications.Wpf;
using POS.BusinessRule;
using POS.Model;
using POS.Utilities;
using POS.Utilities.Encryption;
using POSSystem.UI.Service;
using Prism.Commands;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace POSSystem.UI.ViewModel
{
    public class UserProfileViewModel : ViewModelBase
    {
        private IBouncyCastleEncryption _encryption;
        private string _profileImageFullPath = "";
        private bool _isProfileImageChanged;
        private ImageSource _profileImage = null;
        private int _salesCount;
        private int _purchaseCount;

        public int SalesCount
        {
            get { return _salesCount; }
            set { _salesCount = value; OnPropertyChanged(); }
        }

        public int PurchaseCount
        {
            get { return _purchaseCount; }
            set { _purchaseCount = value; OnPropertyChanged(); }
        }


        public bool IsProfileImageChanged
        {
            get { return _isProfileImageChanged; }
            set { _isProfileImageChanged = value; OnPropertyChanged(); }
        }


        public User CurrentUser { get; private set; }
        public ImageSource ProfileImage
        {
            get { return _profileImage; }
            set
            {
                _profileImage = value;
                OnPropertyChanged();
            }
        }

        public ICommand ImportNewImageCommand { get; set; }
        public ICommand ChangeProfilePicCommand { get; set; }
        public ICommand DiscardProfilePicCommand { get; set; }


        public UserProfileViewModel(IBouncyCastleEncryption encryption)
        {
            _encryption = encryption;
            CurrentUser = _loggedInUser;
            ImportNewImageCommand = new DelegateCommand(OnNewImageSelectionExecute);
            ChangeProfilePicCommand = new DelegateCommand(OnProfileImageChangedExecute);
            DiscardProfilePicCommand = new DelegateCommand(OnProfileImageDiscar);

            LoadDefaultProfileImage();
            GetUserStat();
        }

        private void OnProfileImageDiscar()
        {
            LoadDefaultProfileImage();
        }

        private async void OnProfileImageChangedExecute()
        {
            try
            {
                string existingPath = FilePath.GetProfileImageFullPath(_loggedInUser.ProfileImage);
                File.Delete(existingPath);

                UserBO bO = new UserBO(_encryption);
                string imageName = Guid.NewGuid().ToString("N").Substring(0, 15)  + Path.GetExtension(_profileImageFullPath);
                _loggedInUser.ProfileImage = imageName;
                FileUtility.SaveProfileFile(_profileImageFullPath, imageName);
                await bO.UpdateUser(_loggedInUser);
                StaticContainer.ShowNotification("Image Changed", "You have changed your profile picture.", NotificationType.Success);

            }
            catch (Exception)
            {
                StaticContainer.ShowNotification("Error", StaticContainer.ErrorMessage, NotificationType.Error);
            }
            finally
            {
                IsProfileImageChanged = false;
            }
        }

        private void OnNewImageSelectionExecute()
        {
            _profileImageFullPath = FileUtility.OpenImageFilePicker();
            if(!string.IsNullOrEmpty(_profileImageFullPath))
            {
                ProfileImage = GetImageFromFileName(_profileImageFullPath);
                IsProfileImageChanged = true;
            }
            

        }

        private void LoadDefaultProfileImage()
        {

            IsProfileImageChanged = false;
            _profileImageFullPath = "";
            string getProfilePath = FilePath.GetProfileImageFullPath(_loggedInUser.ProfileImage==null? "": _loggedInUser.ProfileImage);
            if (string.IsNullOrEmpty(CurrentUser.ProfileImage) || !File.Exists(getProfilePath))
            {
                BitmapImage img = new BitmapImage();

                img.BeginInit();
                img.UriSource = new Uri("pack://application:,,,/POSSystem;component/Image/profileImage.png");
                img.EndInit();

                ProfileImage = img;
            }
            else
            {
                ProfileImage = GetImageFromFileName(getProfilePath);
            }
        }

        private async void GetUserStat()
        {
            UserBO userBO = new UserBO(_encryption);
            Tuple<int, int> userStat = await userBO.GetUserStat(_loggedInUser.Id);
            PurchaseCount = userStat.Item1;
            SalesCount = userStat.Item2;
        }

        private ImageSource GetImageFromFileName(string imageFullPath)
        {
            using (System.Drawing.Bitmap img = new System.Drawing.Bitmap(imageFullPath))
            {
                var handle = img.GetHbitmap();
                try
                {
                    
                    return Imaging.CreateBitmapSourceFromHBitmap(handle, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                }
                catch
                {
                    return null;
                }
                finally
                {
                    DeleteObject(handle);
                }
            }
        }


        //If you get 'dllimport unknown'-, then add 'using System.Runtime.InteropServices;'
        [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeleteObject([In] IntPtr hObject);
    }
}
