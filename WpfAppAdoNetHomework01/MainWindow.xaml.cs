using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WpfAppAdoNetHomework01.Models.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace WpfAppAdoNetHomework01
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string connectionString;
        private SqlConnection connection = null;
        private SqlDataReader reader = null;
        private int selectedItemIndex;

        private readonly string ALL_LOAD = "SELECT * FROM PRODUCTS";
        private readonly string NAME_LOAD = "SELECT NAME FROM PRODUCTS";
        private readonly string COLOR_LOAD = "SELECT DISTINCT COLOR FROM PRODUCTS";
        private readonly string MAX_CALORIES_LOAD = "SELECT MAX(CALORIES) FROM PRODUCTS";
        private readonly string MIN_CALORIES_LOAD = "SELECT MIN(CALORIES) FROM PRODUCTS";
        private readonly string AVERAGE_CALORIES_LOAD = "SELECT AVG(CALORIES) FROM PRODUCTS";
        private readonly string QUANTITY_VEGETABLES_LOAD = "SELECT COUNT(TYPE) FROM PRODUCTS WHERE TYPE='Vegetable'";
        private readonly string QUANTITY_FRUITS_LOAD = "SELECT COUNT(TYPE) FROM PRODUCTS WHERE TYPE='Fruit'";
        private readonly string PRODUCT_QUANTITY_BY_COLOR_LOAD = "SELECT COUNT(COLOR) FROM PRODUCTS WHERE COLOR = @COLORE";
        private readonly string PRODUCT_BY_COLOR_LOAD = "SELECT COLOR, COUNT(COLOR) FROM PRODUCTS GROUP BY COLOR";
        private readonly string PRODUCT_LESS_CALORIES_LOAD = "SELECT * FROM PRODUCTS WHERE CALORIES < @CALORIES";
        private readonly string PRODUCT_MORE_CALORIES_LOAD = "SELECT * FROM PRODUCTS WHERE CALORIES > @CALORIES";
        private readonly string PRODUCT_LESS_AND_MORE_CALORIES_LOAD = "SELECT * FROM PRODUCTS WHERE CALORIES < @CALORIES_LESS AND CALORIES > @CALORIES_MORE";
        private readonly string PRODUCT_RED_OR_YELLOW_COLOR = "SELECT * FROM PRODUCTS WHERE COLOR = 'Red' OR COLOR = 'YELLOW'";



        public MainWindow()
        {
            InitializeComponent();

            connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }

        private void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                connection = new SqlConnection(connectionString);
                connection.Open();
                Status.Text = "Connected";
                Status.Foreground = Brushes.Green;
                ConnectButton.IsEnabled = false;
                DisconnectButton.IsEnabled = true;
                SelectListForShow.IsEnabled = true;

                var command = new SqlCommand(ALL_LOAD, connection);

                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var vegetablesAndFruit = VegetablesAndFruit.GetVegetablesAndFruitr(reader);
                    MainText.Items.Add(vegetablesAndFruit.ToString());
                }

                reader.Close();
            }
            catch (Exception)
            {
                Status.Text = "Error";
            }

        }
        private void DisconnectButton_Click(object sender, RoutedEventArgs e)
        {
            if (connection != null)
            {
                connection.Close();
                Status.Text = "Disconnected";
                Status.Foreground = Brushes.Red;
                MainText.Items.Clear();
                ConnectButton.IsEnabled = true;
                DisconnectButton.IsEnabled = false;
                SelectListForShow.IsEnabled = false;
                ShowButton.IsEnabled = false;
            }
        }

        private void ShowButton_Click(object sender, RoutedEventArgs e)
        {
            MainText.Items.Clear();

            switch (selectedItemIndex)
            {
                case 0:
                    LoadProducts(ALL_LOAD);
                    break;
                case 1:
                    MainText.Items.Add("Name:");
                    LoadString(NAME_LOAD);
                    break;
                case 2:
                    MainText.Items.Add("Color:");
                    LoadString(COLOR_LOAD);
                    break;
                case 3:
                    MainText.Items.Add("Maximum calories:");
                    LoadDouble(MAX_CALORIES_LOAD);
                    break;
                case 4:
                    MainText.Items.Add("Minimum calories:");
                    LoadDouble(MIN_CALORIES_LOAD);
                    break;
                case 5:
                    MainText.Items.Add("Average calories:");
                    LoadDouble(AVERAGE_CALORIES_LOAD);
                    break;
                case 6:
                    MainText.Items.Add("Quantity vegetables:");
                    LoadInt(QUANTITY_VEGETABLES_LOAD);
                    break;
                case 7:
                    MainText.Items.Add("Quantity fruits:");
                    LoadInt(QUANTITY_FRUITS_LOAD);
                    break;
                case 8:
                    SelectColorWindow selectQuantityColorWindow = new SelectColorWindow();
                    selectQuantityColorWindow.ShowDialog();
                    MainText.Items.Add($"Quantity {selectQuantityColorWindow.ColorForSearch} fruits:");
                    LoadQuantityByColor(PRODUCT_QUANTITY_BY_COLOR_LOAD, selectQuantityColorWindow.ColorForSearch);
                    break;
                case 9:
                    LoadQuantityProductsByColor(PRODUCT_BY_COLOR_LOAD);
                    break;
                case 10:
                    SelectOneCaloriesWindow selectOneLessCaloriesWindow = new SelectOneCaloriesWindow();
                    selectOneLessCaloriesWindow.TextBlockLessOrMore.Text = "Less than: ";
                    selectOneLessCaloriesWindow.ShowDialog();
                    LoadProductsByOneCalories(PRODUCT_LESS_CALORIES_LOAD, selectOneLessCaloriesWindow.CaloriesSearch);
                    break;
                case 11:
                    SelectOneCaloriesWindow selectOneMoreCaloriesWindow = new SelectOneCaloriesWindow();
                    selectOneMoreCaloriesWindow.TextBlockLessOrMore.Text = "More than: ";
                    selectOneMoreCaloriesWindow.ShowDialog();
                    LoadProductsByOneCalories(PRODUCT_MORE_CALORIES_LOAD, selectOneMoreCaloriesWindow.CaloriesSearch);
                    break;
                case 12:
                    SelectOneCaloriesWindow selectTwoMoreCaloriesWindow = new SelectOneCaloriesWindow();
                    selectTwoMoreCaloriesWindow.TextBlockLessOrMore.Text = "More than: ";
                    selectTwoMoreCaloriesWindow.ShowDialog();
                    SelectOneCaloriesWindow selectTwoLessCaloriesWindow = new SelectOneCaloriesWindow();
                    selectTwoLessCaloriesWindow.TextBlockLessOrMore.Text = "Less than: ";
                    selectTwoLessCaloriesWindow.ShowDialog();
                    LoadProductsByTwoCalories(PRODUCT_LESS_AND_MORE_CALORIES_LOAD, selectTwoMoreCaloriesWindow.CaloriesSearch, selectTwoLessCaloriesWindow.CaloriesSearch);
                    break;
                case 13:
                    LoadProducts(PRODUCT_RED_OR_YELLOW_COLOR);
                    break;
                default:
                    break;
            }
        }

        private void SelectListForShow_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowButton.IsEnabled = true;
            selectedItemIndex = SelectListForShow.SelectedIndex;
        }

        private void LoadProducts(string readQuery)
        {
            var command = new SqlCommand(readQuery, connection);
            reader = command.ExecuteReader();

            while (reader.Read())
            {
                var vegetablesAndFruit = VegetablesAndFruit.GetVegetablesAndFruitr(reader);
                MainText.Items.Add(vegetablesAndFruit.ToString());
            }

            reader.Close();
        }

        private void LoadString(string readQuery)
        {
            var command = new SqlCommand(readQuery, connection);
            reader = command.ExecuteReader();

            while (reader.Read())
            {
                MainText.Items.Add(reader.GetString(0));
            }

            reader.Close();
        }

        private void LoadDouble(string readQuery)
        {
            var command = new SqlCommand(readQuery, connection);
            reader = command.ExecuteReader();

            while (reader.Read())
            {
                MainText.Items.Add(reader.GetDouble(0));
            }

            reader.Close();
        }

        private void LoadInt(string readQuery)
        {
            var command = new SqlCommand(readQuery, connection);
            reader = command.ExecuteReader();

            while (reader.Read())
            {
                MainText.Items.Add(reader.GetInt32(0));
            }

            reader.Close();
        }
        private void LoadQuantityProductsByColor(string readQuery)
        {
            var command = new SqlCommand(readQuery, connection);
            reader = command.ExecuteReader();

            while (reader.Read())
            {
                MainText.Items.Add($"Color: {reader.GetFieldValue<string>(0)}\t Quantity: {reader.GetFieldValue<int>(1)}");
            }

            reader.Close();
        }

        private void LoadQuantityByColor(string readQuery, string color)
        {
            var command = new SqlCommand(readQuery, connection);
            command.Parameters.AddWithValue("@COLORE", color);
            reader = command.ExecuteReader();

            while (reader.Read())
            {
                MainText.Items.Add(reader.GetInt32(0));
            }

            reader.Close();
        }

        private void LoadProductsByOneCalories(string readQuery, double calories)
        {
            var command = new SqlCommand(readQuery, connection);
            command.Parameters.AddWithValue("@CALORIES", calories);
            reader = command.ExecuteReader();

            while (reader.Read())
            {
                var vegetablesAndFruit = VegetablesAndFruit.GetVegetablesAndFruitr(reader);
                MainText.Items.Add(vegetablesAndFruit.ToString());
            }

            reader.Close();
        }

        private void LoadProductsByTwoCalories(string readQuery, double caloriesMore, double caloriesLess)
        {
            var command = new SqlCommand(readQuery, connection);
            command.Parameters.AddWithValue("@CALORIES_MORE", caloriesMore);
            command.Parameters.AddWithValue("@CALORIES_LESS", caloriesLess);
            reader = command.ExecuteReader();

            while (reader.Read())
            {
                var vegetablesAndFruit = VegetablesAndFruit.GetVegetablesAndFruitr(reader);
                MainText.Items.Add(vegetablesAndFruit.ToString());
            }

            reader.Close();
        }
    }
}
