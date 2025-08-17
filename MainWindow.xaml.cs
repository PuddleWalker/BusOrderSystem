using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Theme.WPF.Themes;
using BusOrderSystem.Models;
using System.ComponentModel;
using System.Globalization;

namespace BusOrderSystem
{
    public class FleetIdToNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int fleetId)
            {
                var mainWindow = Application.Current.MainWindow as MainWindow;
                var fleet = mainWindow?.BusFleets.FirstOrDefault(c => c.FleetId == fleetId);

                return fleet != null ? $"{fleet.FleetName}" : $"не найдено";
            }
            return "Н/Д";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class ModelIdToNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int customerId)
            {
                var mainWindow = Application.Current.MainWindow as MainWindow;
                var customer = mainWindow?.BusModels.FirstOrDefault(c => c.ModelId == customerId);

                return customer != null ? $"{customer.ModelName}" : $"не найдено";
            }
                return "Н/Д";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class BusIdToNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int busId)
            {
                var mainWindow = Application.Current.MainWindow as MainWindow;
                var bus = mainWindow?.Buses.FirstOrDefault(c => c.BusId == busId);

                return bus != null ? $"{bus.BusNumber}" : $"не найдено";
            }
            return "Н/Д";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class DriverIdToNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int driverId)
            {
                var mainWindow = Application.Current.MainWindow as MainWindow;
                var driver = mainWindow?.Drivers.FirstOrDefault(c => c.DriverId == driverId);

                return driver != null ? $"{driver.FirstName} {driver.LastName}" : $"не найдено";
            }
            return "Н/Д";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class CustomerIdToNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int customerId)
            {
                var mainWindow = Application.Current.MainWindow as MainWindow;
                var customer = mainWindow?.Customers.FirstOrDefault(c => c.CustomerId == customerId);

                return customer != null ? $"{customer.Name}" : $"не найдено";
            }
            return "Н/Д";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public BusOrderSystemContext bos;
        public ObservableCollection<Bus> Buses { get; set; }
        public ObservableCollection<BusFleet> BusFleets { get; set; }
        public ObservableCollection<BusModel> BusModels { get; set; }
        public ObservableCollection<Shift> Shifts { get; set; }
        public ObservableCollection<Customer> Customers { get; set; }
        public ObservableCollection<Order> Orders { get; set; }
        public ObservableCollection<Driver> Drivers { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            bos = new();
            Buses = new(bos.Buses.ToList().Distinct());
            BusFleets = new(bos.BusFleets.ToList().Distinct());
            BusModels = new(bos.BusModels.ToList().Distinct());
            BusFleets = new(bos.BusFleets.ToList().Distinct());
            Customers = new(bos.Customers.ToList().Distinct());
            Orders = new(bos.Orders.ToList().Distinct());
            Drivers = new(bos.Drivers.ToList().Distinct());
            Shifts = new(bos.Shifts.ToList().Distinct());
            DataContext = this;
        }

        private void AddClick(object sender, RoutedEventArgs e)
        {
            EditWindow editWindow;
            switch (TCBusOrderSystem.SelectedIndex)
            {
                case 0:
                    {
                        var model = new BusModel()
                        { ModelId = IdSearch(bos.BusModels.Distinct().Select(v => v.ModelId)) };
                        editWindow = new(model, bos, false);
                        if (editWindow.ShowDialog() == true)
                        {
                            BusModels.Add(model);
                            bos.Add(model);
                            Status.Text = "Добавлена новая модель";
                        }
                        break;
                    }
                case 1:
                    {
                        var fleet = new BusFleet()
                        { FleetId = IdSearch(bos.BusFleets.Distinct().Select(v => v.FleetId)) };
                        editWindow = new(fleet, bos, false);
                        if (editWindow.ShowDialog() == true)
                        {
                            BusFleets.Add(fleet);
                            bos.Add(fleet);
                            Status.Text = "Добавлен новый автопарк";
                        }
                        break;
                    }
                case 2:
                    {
                        var shift = new Shift()
                        { ShiftId = IdSearch(bos.Shifts.Distinct().Select(v => v.ShiftId)) };
                        editWindow = new(shift, bos, false);
                        if (editWindow.ShowDialog() == true)
                        {
                            Shifts.Add(shift);
                            bos.Add(shift);
                            Status.Text = "Добавлена новая смена";
                        }
                        break;
                    }
                case 3:
                    {
                        var bus = new Bus()
                        { BusId = IdSearch(bos.Buses.Distinct().Select(v => v.BusId)) };
                        editWindow = new(bus, bos, false);
                        if (editWindow.ShowDialog() == true)
                        {
                            Buses.Add(bus);
                            bos.Add(bus);
                            Status.Text = "Добавлен новый автобус";
                        }
                        break;
                    }
                case 4:
                    {
                        var driver = new Driver()
                        { DriverId = IdSearch(bos.Drivers.Distinct().Select(v => v.DriverId)) };
                        editWindow = new(driver, bos, false);
                        if (editWindow.ShowDialog() == true)
                        {
                            Drivers.Add(driver);
                            bos.Add(driver);
                            Status.Text = "Добавлен новый водитель";
                        }
                        break;
                    }
                case 5:
                    {
                        var customer = new Customer()
                        { CustomerId = IdSearch(bos.Customers.Distinct().Select(v => v.CustomerId)) };
                        editWindow = new(customer, bos, false);
                        if (editWindow.ShowDialog() == true)
                        {
                            Customers.Add(customer);
                            bos.Add(customer);
                            Status.Text = "Добавлен новый клиент";
                        }
                        break;
                    }
                case 6:
                    {
                        var order = new Order()
                        { OrderId = IdSearch(bos.Orders.Distinct().Select(v => v.OrderId)) };
                        editWindow = new(order, bos, false);
                        if (editWindow.ShowDialog() == true)
                        {
                            Orders.Add(order);
                            bos.Add(order);
                            Status.Text = "Добавлен новый заказ";
                        }
                        break;
                    }
            }
        }
        private int IdSearch(IQueryable<int> ids)
        {
            int nextMissingId = 1;
            foreach (var id in ids)
            {
                if (id == nextMissingId)
                    nextMissingId++;
                else
                    break;
            }
            return nextMissingId;
        }
        private void DeleteClick(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите удалить данные ?",
                "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Warning,
                MessageBoxResult.Yes) == MessageBoxResult.Yes)
            {
                switch (TCBusOrderSystem.SelectedIndex)
                {
                    case 0:
                        {
                            var model = ModelsDataGrid.SelectedItem as BusModel;
                            if (model is not null)
                            {
                                bos.BusModels.Remove(model);
                                BusModels.Remove(model);
                                Status.Text = "Модель удалена";
                            }
                            else
                            {
                                Status.Text = "Для удаления модели выберите её !!!";
                            }
                            break;
                        }
                    case 1:
                        {
                            var fleet = FleetsDataGrid.SelectedItem as BusFleet;
                            if (fleet is not null)
                            {
                                bos.BusFleets.Remove(fleet);
                                BusFleets.Remove(fleet);
                                Status.Text = "Автопарк удалён";
                            }
                            else
                            {
                                Status.Text = "Для удаления автопарка выберите его !!!";
                            }
                            break;
                        }
                    case 2:
                        {
                            var shift = ShiftsDataGrid.SelectedItem as Shift;
                            if (shift is not null)
                            {
                                bos.Shifts.Remove(shift);
                                Shifts.Remove(shift);
                                Status.Text = "Смена удалена";
                            }
                            else
                            {
                                Status.Text = "Для удаления смены выберите её !!!";
                            }
                            break;
                        }
                    case 3:
                        {
                            var bus = BusesDataGrid.SelectedItem as Bus;
                            if (bus is not null)
                            {
                                bos.Buses.Remove(bus);
                                Buses.Remove(bus);
                                Status.Text = "Автобус удалён";
                            }
                            else
                            {
                                Status.Text = "Для удаления автобуса выберите его !!!";
                            }
                            break;
                        }
                    case 4:
                        {
                            var driver = DriversDataGrid.SelectedItem as Driver;
                            if (driver is not null)
                            {
                                bos.Drivers.Remove(driver);
                                Drivers.Remove(driver);
                                Status.Text = "Водитель удалён";
                            }
                            else
                            {
                                Status.Text = "Для удаления водителя выберите его !!!";
                            }
                            break;
                        }
                    case 5:
                        {
                            var customer = CustomersDataGrid.SelectedItem as Customer;
                            if (customer is not null)
                            {
                                bos.Customers.Remove(customer);
                                Customers.Remove(customer);
                                Status.Text = "Клиент удалён";
                            }
                            else
                            {
                                Status.Text = "Для удаления клиента выберите его !!!";
                            }
                            break;
                        }
                    case 6:
                        {
                            var order = OrdersDataGrid.SelectedItem as Order;
                            if (order is not null)
                            {
                                bos.Orders.Remove(order);
                               Orders.Remove(order);
                                Status.Text = "Заказ удалён";
                            }
                            else
                            {
                                Status.Text = "Для удаления заказа выберите его !!!";
                            }
                            break;
                        }
                }
            }
        }
        private void EditClick(object sender, RoutedEventArgs e)
        {
            EditWindow editWindow;
            switch (TCBusOrderSystem.SelectedIndex)
            {
                case 0:
                    {
                        var model = ModelsDataGrid.SelectedItem as BusModel;
                        editWindow = new(model, bos, false);
                        if (editWindow.ShowDialog() == true)
                        {
                            bos.Update(model);
                            ModelsDataGrid.Items.Refresh();
                        }
                        break;
                    }
                case 1:
                    {
                        var fleet = FleetsDataGrid.SelectedItem as BusFleet;
                        editWindow = new(fleet, bos, false);
                        if (editWindow.ShowDialog() == true)
                        {
                            bos.Update(fleet);
                            FleetsDataGrid.Items.Refresh();
                        }
                        break;
                    }
                case 2:
                    {
                        var shift = ShiftsDataGrid.SelectedItem as Shift;
                        editWindow = new(shift, bos, false);
                        if (editWindow.ShowDialog() == true)
                        {
                            bos.Update(shift);
                            ShiftsDataGrid.Items.Refresh();
                        }
                        break;
                    }
                case 3:
                    {
                        var bus = BusesDataGrid.SelectedItem as Bus;
                        editWindow = new(bus, bos, false);
                        if (editWindow.ShowDialog() == true)
                        {
                            bos.Update(bus);
                            BusesDataGrid.Items.Refresh();
                        }
                        break;
                    }
                case 4:
                    {
                        var driver = DriversDataGrid.SelectedItem as Driver;
                        editWindow = new(driver, bos, false);
                        if (editWindow.ShowDialog() == true)
                        {
                            bos.Update(driver);
                            DriversDataGrid.Items.Refresh();
                        }
                        break;
                    }
                case 5:
                    {
                        var customer = CustomersDataGrid.SelectedItem as Customer;
                        editWindow = new(customer, bos, false);
                        if (editWindow.ShowDialog() == true)
                        {
                            bos.Update(customer);
                            CustomersDataGrid.Items.Refresh();
                        }
                        break;
                    }
                case 6:
                    {
                        var order = OrdersDataGrid.SelectedItem as Order;
                        editWindow = new(order, bos, false);
                        if (editWindow.ShowDialog() == true)
                        {
                            bos.Update(order);
                            OrdersDataGrid.Items.Refresh();
                        }
                        break;
                    }
            }
        }
        private void AboutClick(object sender, RoutedEventArgs e)
        {
            var about = new AboutWindow();
            about.ShowDialog();
        }
        private void ExitClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void SQLiteClick(object sender, RoutedEventArgs e)
        {
            bos.SaveChanges();
            bos.Dispose();
            bos = new();
            Buses = new(bos.Buses.ToList().Distinct());
            BusFleets = new(bos.BusFleets.ToList().Distinct());
            BusModels = new(bos.BusModels.ToList().Distinct());
            Customers = new(bos.Customers.ToList().Distinct());
            Orders = new(bos.Orders.ToList().Distinct());
            Drivers = new(bos.Drivers.ToList().Distinct());
            Shifts = new(bos.Shifts.ToList().Distinct());

            ModelsDataGrid.ItemsSource = BusModels;
            FleetsDataGrid.ItemsSource = BusFleets;
            BusesDataGrid.ItemsSource = Buses;
            CustomersDataGrid.ItemsSource = Customers;
            OrdersDataGrid.ItemsSource = Orders;
            DriversDataGrid.ItemsSource = Drivers;
            ShiftsDataGrid.ItemsSource = Shifts;
            Status.Text = "Подключено к SQLite";
        }
        private void SQLServerClick(object sender, RoutedEventArgs e)
        {
            bos.SaveChanges();
            bos.Dispose();
            bos = new(1);
            Buses = new(bos.Buses.ToList().Distinct());
            BusFleets = new(bos.BusFleets.ToList().Distinct());
            BusModels = new(bos.BusModels.ToList().Distinct());
            Customers = new(bos.Customers.ToList().Distinct());
            Orders = new(bos.Orders.ToList().Distinct());
            Drivers = new(bos.Drivers.ToList().Distinct());
            Shifts = new(bos.Shifts.ToList().Distinct());

            ModelsDataGrid.ItemsSource = BusModels;
            FleetsDataGrid.ItemsSource = BusFleets;
            BusesDataGrid.ItemsSource = Buses;
            CustomersDataGrid.ItemsSource = Customers;
            OrdersDataGrid.ItemsSource = Orders;
            DriversDataGrid.ItemsSource = Drivers;
            ShiftsDataGrid.ItemsSource = Shifts;
            Status.Text = "Подключено к SQL Server";
        }
        private void ChangeTheme(object sender, RoutedEventArgs e)
        {
            switch (((MenuItem)sender).Uid)
            {
                case "0":
                    ThemesController.SetTheme(ThemeType.DeepDark);
                    break;
                case "1":
                    ThemesController.SetTheme(ThemeType.SoftDark);
                    break;
                case "2":
                    ThemesController.SetTheme(ThemeType.DarkGreyTheme);
                    break;
                case "3":
                    ThemesController.SetTheme(ThemeType.GreyTheme);
                    break;
                case "4":
                    ThemesController.SetTheme(ThemeType.LightTheme);
                    break;
                case "5":
                    ThemesController.SetTheme(ThemeType.RedBlackTheme);
                    break;
                case "6":
                    break;
            }

            e.Handled = true;
        }
        private void ToolTipOpening_KeepOpen(object sender, ToolTipEventArgs e)
        {
            if (sender is TextBlock block)
            {
                block.ToolTipOpening -= ToolTipOpening_KeepOpen;
                if (block.ToolTip is ToolTip toolTip)
                {
                    toolTip.StaysOpen = true;
                    toolTip.Closed += (s, args) => block.ToolTipOpening += ToolTipOpening_KeepOpen;
                }
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            bos.SaveChanges();
        }
    }
}