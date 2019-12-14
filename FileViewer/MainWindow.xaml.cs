using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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

namespace FileViewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        OpenFileDialog file;
        IEnumerable<string> lines;

        public MainWindow()
        {
            InitializeComponent();
            file = null;
            lines = null;
        }

        private void btnFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                file = openFileDialog;
                lines = File.ReadLines(file.FileName);
                txtEditor.Text = String.Join("\n", lines);
            }
        }

        private void txtEditor_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private async void btnApply_Click(object sender, RoutedEventArgs e)
        {
            if (file != null)
            {
                try
                {
                    await ApplyFilter();
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message, "File error");
                }
            }
            else
            {
                MessageBox.Show("Select file first.", "Input error");
            }
        }

        private async Task ApplyFilter()
        {
            try
            {
                var sort = from str in lines where (str.Contains(textOption1.Text) || (textOption2.Text.Length == 0 ? false : str.Contains(textOption2.Text))) && (str.Contains(textOption3.Text) || (textOption4.Text.Length == 0 ? false : str.Contains(textOption4.Text))) select str;
                txtEditor.Text = string.Join("\n", sort);
            }
            catch
            {
                throw new ApplicationException("File read access is denied");
            }
            await Task.FromResult<object>(null);
        }
    }
}
