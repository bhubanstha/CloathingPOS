﻿using Autofac;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSSystem.UI.Service
{
    public static class StaticContainer
    {
        public static App ThisApp { get; set; }
        public static IContainer Container { get; set; }

        public static Flyout SettingFlyout { get; set; }
        public static Flyout AddCategoryFlyout { get; set; }

        public static IDialogCoordinator DialogCoordinator { get; set; }

        public static MetroDialogSettings DialogSettings{get ; private set;}

        static StaticContainer()
        {
            DialogSettings = new MetroDialogSettings
            {
                AnimateShow = true,
                AnimateHide = true,
                DefaultButtonFocus = MessageDialogResult.Affirmative,
                AffirmativeButtonText = "OK",
                OwnerCanCloseWithDialog = true,
                ColorScheme = MetroDialogColorScheme.Theme
            };
        }

    }
}
