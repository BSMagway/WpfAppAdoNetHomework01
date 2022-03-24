using System.Windows;
using System.Windows.Controls;

namespace WpfAppAdoNetHomework01
{
    /// <summary>
    /// Interaction logic for SelectOneCaloriesWindow.xaml
    /// </summary>
    public partial class SelectOneCaloriesWindow : Window
    {
        public double CaloriesSearch { get; private set; }
        public SelectOneCaloriesWindow()
        {
            InitializeComponent();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            CaloriesSearch = double.Parse(CaloriesEnter.Text);
            DialogResult = true;
        }

        private void CaloriesEnter_TextChanged(object sender, TextChangedEventArgs e)
        {
            CloseButton.IsEnabled = true;
        }
    }
}
