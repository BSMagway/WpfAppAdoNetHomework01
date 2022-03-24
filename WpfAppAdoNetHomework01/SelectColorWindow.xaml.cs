using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows;

namespace WpfAppAdoNetHomework01
{
    /// <summary>
    /// Interaction logic for SelectColorWindow.xaml
    /// </summary>
    public partial class SelectColorWindow : Window
    {
        private readonly string SELECT_COLOR_FROM_DB = "SELECT DISTINCT COLOR FROM PRODUCTS";
        private string connectionString;
        public string ColorForSearch { get; private set; }

        public SelectColorWindow()
        {
            InitializeComponent();

            connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }

        private void WindowSelectColor_Loaded(object sender, RoutedEventArgs e)
        {
            SqlConnection connection = null;

            try
            {             
                using (connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    var command = new SqlCommand(SELECT_COLOR_FROM_DB, connection);
                    var reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        ListColor.Items.Add(reader.GetString(0));
                    }
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Fail");
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                }
            }
        }

        private void ComboBoxColor_SelectionChanged(object sender, RoutedEventArgs e)
        {
            ColorForSearch = ListColor.SelectedItem.ToString();
            CloseButton.IsEnabled = true;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
