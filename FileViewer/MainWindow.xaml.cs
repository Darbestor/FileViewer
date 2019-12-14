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
        IEnumerator<string> enumerator;
        bool canIterate;

        public MainWindow()
        {
            InitializeComponent();
            file = null;
            lines = null;
            enumerator = null;
            canIterate = false;
        }

        private void btnFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text Files (*.txt)|*.txt|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                file = openFileDialog;
                this.Title = new FileInfo(file.FileName).FullName;
                try
                {
                    lines = File.ReadLines(file.FileName);
                    SetFirstBlock();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Read error");
                }
            }
        }

        private void SetFirstBlock()
        {
            enumerator = lines.GetEnumerator();
            docBox.Document.Blocks.Clear();
            docBox.ScrollToVerticalOffset(0);
            canIterate = true;
            AddTextBlock();
        }

        private void AddTextBlock()
        {
            Paragraph paragraph = new Paragraph();
            StringBuilder str = new StringBuilder();
            int numOfLines = (int)(docBox.ActualHeight / FontSize / FontFamily.LineSpacing) + 1;
            while (numOfLines > 0)
            {
                if (enumerator.MoveNext())
                {
                    str.AppendLine(enumerator.Current);
                    numOfLines--;
                }
                else
                {
                    canIterate = false;
                    break;
                }
            }
            str.Remove(str.Length - 2, 2);
            paragraph.Inlines.Add(str.ToString());
            docBox.Document.Blocks.Add(paragraph);
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
                lines = from str in File.ReadLines(file.FileName) where (str.Contains(op1) || (op2.Length == 0 ? false : str.Contains(op2))) && (str.Contains(op3) || (op4.Length == 0 ? false : str.Contains(op4))) select str;
                SetFirstBlock();
            }
            catch(Exception ex)
            {
                throw ex;
            }
            await Task.FromResult<object>(null);
        }

        private void docBox_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (e.VerticalChange > 0 && canIterate)
            {
                if (e.VerticalOffset + e.ViewportHeight == e.ExtentHeight)
                {
                    AddTextBlock();
                }
            }
        }

        private void docBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
