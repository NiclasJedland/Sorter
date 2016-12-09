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
	public partial class MainWindow : Window
	{
		List<string> dontAdd = new List<string>() { "thumbs.db", "desktop.ini" };

		public MainWindow()
		{
			InitializeComponent();
			this.PreviewKeyDown += new KeyEventHandler(HandleKey);
			
			Properties.Settings.Default._temp = "";
			LoadDefault();
		}
		
		#region Add
		private void btn_defaultFolders_Click(object sender, RoutedEventArgs e)
		{
			listBox1.Items.Clear();
			Properties.Settings.Default._temp = "";
			LoadDefault();
			CountFiles();
		}

		private void btn_addNewFolder_Click(object sender, RoutedEventArgs e)
		{
			var newFolder = FolderBrowserDialog();
			if(newFolder.Length > 0)
			{
				foreach(var name in Directory.GetFiles(newFolder))
				{
					if(name.ToLower().Contains(tbox_searchBox.Text.ToLower()) && !dontAdd.Any(name.ToLower().Contains))
						listBox1.Items.Add(new FileListBoxItem { FileFullName = name });
				}
			}
			CountFiles();
		}
		#endregion

		#region Run
		private void btn_Open_Click(object sender, RoutedEventArgs e)
		{
			RunItem();
		}

		private void listBox1_doubleClick(object sender, MouseButtonEventArgs e)
		{
			RunItem();
		}

		private void RunItem()
		{
			if(listBox1.SelectedItem != null)
			{
				var fileFullname = ((FileListBoxItem)listBox1.SelectedItem).FileFullName;
				Process.Start(fileFullname);
			}
		}

		private void LoadFolder(string folder)
		{
			if(folder.Length > 0)
			{
				foreach(var name in Directory.GetFiles(folder))
				{
					if(name.ToLower().Contains(tbox_searchBox.Text.ToLower()) && !dontAdd.Any(name.ToLower().Contains))
						listBox1.Items.Add(new FileListBoxItem { FileFullName = name });
				}
			}
		}

		public void LoadDefault()
		{
			listBox1.Items.Clear();
			LoadFolder(Properties.Settings.Default._folder1);
			LoadFolder(Properties.Settings.Default._folder2);
			LoadFolder(Properties.Settings.Default._folder3);
			LoadFolder(Properties.Settings.Default._folder4);
			LoadFolder(Properties.Settings.Default._folder5);
			CountFiles();
		}
		#endregion

		#region Close
		private void MenuItem_Click_Exit(object sender, RoutedEventArgs e)
		{
			Application.Current.Shutdown();
		}

		private void btn_Exit_Click(object sender, RoutedEventArgs e)
		{
			Application.Current.Shutdown();
		}
		#endregion

		#region Keys
		private void HandleKey(object sender, KeyEventArgs keyData)
		{
			if((keyData.Key == Key.Escape && tbox_searchBox.Text == "" && listBox1.SelectedItem == null) || (keyData.Key == Key.W && Keyboard.Modifiers == ModifierKeys.Control))
				Application.Current.Shutdown();
			else if(tbox_searchBox.Text != "" && ((keyData.Key == Key.Escape && tbox_searchBox.IsFocused) || (keyData.Key == Key.Escape && listBox1.SelectedItem == null)))
				tbox_searchBox.Text = "";
			else if(keyData.Key == Key.Escape && listBox1.SelectedItem != null)
			{
				listBox1.SelectedItem = null;
				tbox_searchBox.Focus();
			}
			else if(keyData.Key == Key.Enter && listBox1.SelectedItem != null)
				RunItem();
			else if(keyData.Key == Key.Back && !tbox_searchBox.IsFocused)
			{
				tbox_searchBox.Focus();
				Send(Key.Back);
				CountFiles();
			}
			else if((keyData.Key == Key.Down || keyData.Key == Key.Up || keyData.Key == Key.Enter) && listBox1.SelectedItem == null && tbox_searchBox.Text == "")
			{
				listBox1.SelectedIndex = 0;
				listBox1.Focus();
			}
		}

		private void Send(Key key)
		{
			if(Keyboard.PrimaryDevice != null)
			{
				if(Keyboard.PrimaryDevice.ActiveSource != null)
				{
					var e1 = new KeyEventArgs(Keyboard.PrimaryDevice, Keyboard.PrimaryDevice.ActiveSource, 0, Key.Down) { RoutedEvent = Keyboard.KeyDownEvent };
					InputManager.Current.ProcessInput(e1);
				}
			}
		}
		#endregion

		#region Misc
		private void tbox_searchBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			listBox1.Items.Clear();
			LoadDefault();
			LoadFolder(Properties.Settings.Default._temp);
			CountFiles();
		}

		private void btn_ClearAll_Click(object sender, RoutedEventArgs e)
		{
			listBox1.Items.Clear();
			tbox_searchBox.Text = "";
			CountFiles();
		}

		private string FolderBrowserDialog()
		{
			string folder = "";
			var fbd = new CommonOpenFileDialog();

			fbd.Title = "Select a Folder.";
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
			return folder;
		}

		private void CountFiles()
		{
			lbl_count.Content = listBox1.Items.Count.ToString() + " Files";
		}

		private void MenuItem_Click_Settings(object sender, RoutedEventArgs e)
		{
			var window = new settingsWindow();
			
			window.Owner = this;
			window.ShowDialog();
		}
		#endregion
	}
	
	public class FileListBoxItem
	{
		public string FileFullName { get; set; }
		public override string ToString()
		{
			return System.IO.Path.GetFileName(FileFullName);
		}
	}
}
