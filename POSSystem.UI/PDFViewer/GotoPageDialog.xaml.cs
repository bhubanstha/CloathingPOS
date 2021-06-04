/*! MoonPdf - A WPF-based PDF Viewer application that uses the MoonPdfLib library
Copyright (C) 2013  (see AUTHORS file)

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
!*/
using MahApps.Metro.Controls;
using System.Windows;

namespace MoonPdf
{
    public partial class GotoPageDialog : MetroWindow
	{
		private int MaxPageNumber { get; set; }
		public int? SelectedPageNumber { get; private set; }

		public GotoPageDialog(int currentPageNumber, int maxPageNumber)
		{
			InitializeComponent();

			//this.Icon = MoonPdf.Resources.moon.ToBitmapSource();
			this.MaxPageNumber = maxPageNumber;
			this.txtPage.Value = currentPageNumber;
			this.lblMaxPageNumber.Text = maxPageNumber.ToString();
			this.Loaded += GotoPageDialog_Loaded;
		}

		void GotoPageDialog_Loaded(object sender, RoutedEventArgs e)
		{
			this.txtPage.Focus();
			this.txtPage.SelectAll();
		}

		private void BtnGotoPage_Click(object sender, RoutedEventArgs e)
		{
			int page;

			if (!int.TryParse(this.txtPage.Value.ToString(), out page) || page > MaxPageNumber || page < 1)
			{
				MessageBox.Show("Please enter a valid page number.");
				return;
			}

			this.SelectedPageNumber = page;
			this.DialogResult = true;
			this.Close();
		}

		private void BtnCancel_Click(object sender, RoutedEventArgs e)
		{
			this.Close();
		}
	}
}
