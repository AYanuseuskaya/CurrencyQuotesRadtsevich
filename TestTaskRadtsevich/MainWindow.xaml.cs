using ModelTestTask;
using ModelTestTask.Interfaces;
using System;
using System.Collections.Generic;
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
using ViewModelTestTask;

namespace TestTaskRadtsevich
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            IModelWPF model = new Model();
            DataContext = new ViewModel(model);
        }
        void textBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (!Char.IsDigit(e.Text, 0) && e.Text!=",")
            { 
                e.Handled = true; 
            }
            else if (e.Text == "," && (tb.Text.IndexOf(",") != -1 || tb.Text == ""))
            {
                e.Handled = true;
            }
        }
    }
}
