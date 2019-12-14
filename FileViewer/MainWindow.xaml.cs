using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
                try
                {
                    lines = File.ReadLines(file.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Read error");
                }
                PrintText(lines);
            }
        }

        private void PrintText(IEnumerable<string> array)
        {
            txtEditor.Text = string.Join("\n", array);
            this.Title = new FileInfo(file.FileName).FullName;
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

                    MessageBox.Show(ex.Message, "Filter error");
                }
            }
            else
            {
                MessageBox.Show("Select file first.", "Input error");
            }
        }

        private async Task ApplyFilter()
        {
            string op1 = textOption1.Text, op2 = textOption2.Text, op3 = textOption3.Text, op4 = textOption4.Text;
            try
            {
                var sort = from str in lines where (str.Contains(op1) || (op2.Length == 0 ? false : str.Contains(op2))) && (str.Contains(op3) || (op4.Length == 0 ? false : str.Contains(op4))) select str;
                PrintText(sort);
            }
            catch(Exception ex)
            {
                throw ex;
            }
            await Task.FromResult<object>(null);
        }
    }
}
