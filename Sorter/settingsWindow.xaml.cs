using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Linq;
using Microsoft.WindowsAPICodePack.Dialogs;
using Microsoft.WindowsAPICodePack.Shell.Interop;


namespace Sorter
{
	/// <summary>
	/// Interaction logic for settingsWindow.xaml
	/// </summary>
	public partial class settingsWindow : Window
	{
		public settingsWindow()
		{
			InitializeComponent();
			this.PreviewKeyDown += new KeyEventHandler(HandleKey);

			lbl_defaultFolder1.Content = Properties.Settings.Default._folder1;
			lbl_defaultFolder2.Content = Properties.Settings.Default._folder2;
			lbl_defaultFolder3.Content = Properties.Settings.Default._folder3;
			lbl_defaultFolder4.Content = Properties.Settings.Default._folder4;
			lbl_defaultFolder5.Content = Properties.Settings.Default._folder5;
		}

		#region Buttons
		private void btn_okButton_Click(object sender, RoutedEventArgs e)
		{
			SaveAndExit();
		}

		private void btn_applyButton_Click(object sender, RoutedEventArgs e)
		{
			Save();
		}

		private void btn_cancelButton_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}

		private void btn_addDefaultFolder1_Click(object sender, RoutedEventArgs e)
		{
			Properties.Settings.Default._folder1 = FolderBrowserDialog(Properties.Settings.Default._folder1);
			lbl_defaultFolder1.Content = Properties.Settings.Default._folder1;
			this.Focus();
		}

		private void btn_addDefaultFolder2_Click(object sender, RoutedEventArgs e)
		{
			Properties.Settings.Default._folder2 = FolderBrowserDialog(Properties.Settings.Default._folder2);
			lbl_defaultFolder2.Content = Properties.Settings.Default._folder2;
			this.Focus();
		}

		private void btn_addDefaultFolder3_Click(object sender, RoutedEventArgs e)
		{
			Properties.Settings.Default._folder3 = FolderBrowserDialog(Properties.Settings.Default._folder3);
			lbl_defaultFolder3.Content = Properties.Settings.Default._folder3;
			this.Focus();
		}

		private void btn_addDefaultFolder4_Click(object sender, RoutedEventArgs e)
		{
			Properties.Settings.Default._folder4 = FolderBrowserDialog(Properties.Settings.Default._folder4);
			lbl_defaultFolder4.Content = Properties.Settings.Default._folder4;
			this.Focus();
		}

		private void btn_addDefaultFolder5_Click(object sender, RoutedEventArgs e)
		{
			Properties.Settings.Default._folder5 = FolderBrowserDialog(Properties.Settings.Default._folder5);
			lbl_defaultFolder5.Content = Properties.Settings.Default._folder5;
			this.Focus();
		}

		private void btn_removeDefaultFolder1_Click(object sender, RoutedEventArgs e)
		{
			Properties.Settings.Default._folder1 = "";
			lbl_defaultFolder1.Content = "";
		}

		private void btn_removeDefaultFolder2_Click(object sender, RoutedEventArgs e)
		{
			Properties.Settings.Default._folder2 = "";
			lbl_defaultFolder2.Content = "";
		}

		private void btn_removeDefaultFolder3_Click(object sender, RoutedEventArgs e)
		{
			Properties.Settings.Default._folder3 = "";
			lbl_defaultFolder3.Content = "";
		}

		private void btn_removeDefaultFolder4_Click(object sender, RoutedEventArgs e)
		{
			Properties.Settings.Default._folder4 = "";
			lbl_defaultFolder4.Content = "";
		}

		private void btn_removeDefaultFolder5_Click(object sender, RoutedEventArgs e)
		{
			Properties.Settings.Default._folder5 = "";
			lbl_defaultFolder5.Content = "";
		}

		private void btn_resetAllSettings_Click(object sender, RoutedEventArgs e)
		{
			MessageBoxResult result = MessageBox.Show("Do you want to reset all settings?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
			if(result == MessageBoxResult.Yes)
			{
				Properties.Settings.Default._folder1 = "";
				Properties.Settings.Default._folder2 = "";
				Properties.Settings.Default._folder3 = "";
				Properties.Settings.Default._folder4 = "";
				Properties.Settings.Default._folder5 = "";
				lbl_defaultFolder1.Content = "";
				lbl_defaultFolder2.Content = "";
				lbl_defaultFolder3.Content = "";
				lbl_defaultFolder4.Content = "";
				lbl_defaultFolder5.Content = "";
				this.Focus();
				Save();
            }
		}
		#endregion

		#region Misc
		private string FolderBrowserDialog(string originalFolder)
		{
			string folder = "";
            var fbd = new CommonOpenFileDialog();
			fbd.Title = "Select a folder to add";
			fbd.IsFolderPicker = true;
			fbd.AddToMostRecentlyUsedList = false;
			fbd.AllowNonFileSystemItems = false;
			fbd.EnsureFileExists = true;
			fbd.EnsurePathExists = true;
			fbd.EnsureValidNames = true;
			fbd.EnsureReadOnly = false;
			fbd.Multiselect = false;
			fbd.ShowPlacesList = true;
			if(fbd.ShowDialog() == CommonFileDialogResult.Ok)
				folder = fbd.FileName;
			else
				folder = originalFolder;
			return folder;
		}

		private void Save()
		{
			Properties.Settings.Default.Save();
			var mainWin = Application.Current.Windows.Cast<Window>().FirstOrDefault(window => window is MainWindow) as MainWindow;
			mainWin.LoadDefault();
		}

		private void SaveAndExit()
		{
			Save();
			Close();
		}
		#endregion

		#region Keys
		private void HandleKey(object sender, KeyEventArgs keyData)
		{
			if(keyData.Key == Key.Escape || (keyData.Key == Key.W && Keyboard.Modifiers == ModifierKeys.Control))
			{
				MessageBoxResult result = MessageBox.Show("Do you want to close without saving?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
				if(result == MessageBoxResult.Yes)
					Close();
			}
			else if(keyData.Key == Key.Enter)
				SaveAndExit();
			else if(keyData.Key == Key.S && Keyboard.Modifiers == ModifierKeys.Control)
				Save();
		}
		#endregion
	}
}
