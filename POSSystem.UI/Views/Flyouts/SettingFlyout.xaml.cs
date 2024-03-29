﻿using Autofac;
using ControlzEx.Theming;
using MahApps.Metro.Controls;
using POS.Model;
using POSSystem.UI.Enum;
using POSSystem.UI.Service;
using System;
using System.Windows;
using System.Windows.Input;

namespace POSSystem.UI.Views.Flyouts
{
    /// <summary>
    /// Interaction logic for SettingView.xaml
    /// </summary>
    public partial class SettingFlyout : Flyout
    {
        private string _currentBaseColor = "";
        private string _currentColorScheme = "";
        private ICacheService _cacheService;

        public SettingFlyout()
        {
            InitializeComponent();
            _cacheService = StaticContainer.Container.Resolve<ICacheService>();
            ManageShopSetting();
            this.Loaded += SettingFlyout_Loaded;
        }

        private void SettingFlyout_Loaded(object sender, RoutedEventArgs e)
        {
            _currentBaseColor = Convert.ToString(Application.Current.Properties["BaseColour"]);
            _currentColorScheme = Convert.ToString(Application.Current.Properties["ColorScheme"]);
        }

        private void DarkTheme(object sender, MouseButtonEventArgs e)
        {
            if (!string.Equals(_currentBaseColor, "Dark", StringComparison.OrdinalIgnoreCase))
            {
                ThemeManager.Current.ChangeThemeBaseColor(Application.Current, "Dark");
                _currentBaseColor = "Dark";
                Application.Current.Properties["BaseColour"] = _currentBaseColor;
            }
        }

        private void LightTheme(object sender, MouseButtonEventArgs e)
        {
            if (!string.Equals(_currentBaseColor, "Light", StringComparison.OrdinalIgnoreCase))
            {
                ThemeManager.Current.ChangeThemeBaseColor(Application.Current, "Light");
                _currentBaseColor = "Light";
                Application.Current.Properties["BaseColour"] = _currentBaseColor;
            }
        }

        private void ChangeColorScheme(object sender, MouseButtonEventArgs e)
        {
            string colorScheme = ((System.Windows.FrameworkElement)sender).Tag as string;
            if(!string.Equals(colorScheme, _currentColorScheme, StringComparison.OrdinalIgnoreCase))
            {
                ThemeManager.Current.ChangeThemeColorScheme(Application.Current, colorScheme);
                _currentColorScheme = colorScheme;
                Application.Current.Properties["ColorScheme"] = _currentColorScheme;
            }
        }

        void ManageShopSetting()
        {
            User u = _cacheService.ReadCache<User>(CacheKey.LoginUser.ToString());
            if(u!= null)
            {
                shopSetting.Visibility = u.IsAdmin ? Visibility.Visible : Visibility.Collapsed;
            }
        }
    }
}
