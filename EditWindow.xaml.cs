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
using System.Windows.Shapes;
using BusOrderSystem.Models;
using static System.Net.Mime.MediaTypeNames;

namespace BusOrderSystem
{
    /// <summary>
    /// Логика взаимодействия для EditWindow.xaml
    /// </summary>
    public partial class EditWindow : Window
    {
        enum VarType
        {
            BusModel,
            BusFleet,
            Shift,
            Bus,
            Driver,
            Customer,
            Order
        }
        VarType type;
        BusOrderSystemContext bos;
        BusModel model;
        BusFleet fleet;
        Shift shift;
        Bus bus;
        Driver driver;
        Customer customer;
        Order order;
        bool IsNew;
        bool IsSaved = false; 
        private bool _isInternalChange = false;

        public EditWindow(BusModel busModel, BusOrderSystemContext bos, bool IsNew = true)
        {
            IsSaved = false;
            this.bos = bos;
            this.IsNew = IsNew;
            type = VarType.BusModel;
            if (IsNew)
            {
                model = busModel;
                model.ModelName = "";
                model.Capacity = 0;
                model.Price = 0;
                model.ManufacturerCompany = "";
                model.ManufacturerCountry = "";
            }
            else model = busModel;
            InitializeComponent();
            EditWindow1.Title = IsNew ? "Добавление модели" : "Редактирование модели";
            Label1.Visibility = Visibility.Visible;
            Label1.Content = "Имя модели";
            TextBox1.Visibility = Visibility.Visible;
            TextBox1.Text = model.ModelName;
            TextBox1.PreviewTextInput += ExtTextBox_PreviewTextInput;
            Label2.Visibility = Visibility.Visible;
            Label2.Content = "Компания производитель";
            TextBox2.Visibility = Visibility.Visible;
            TextBox2.Text = model.ManufacturerCompany;
            Label3.Visibility = Visibility.Visible;
            Label3.Content = "Страна производитель";
            TextBox3.Visibility = Visibility.Visible;
            TextBox3.Text = model.ManufacturerCountry;
            TextBox3.PreviewTextInput += ExtTextBox_PreviewTextInput;
            Label4.Visibility = Visibility.Visible;
            Label4.Content = "Количество мест";
            TextBox4.Visibility = Visibility.Visible;
            TextBox4.Text = model.Capacity.ToString();
            TextBox4.PreviewTextInput += DigitsOnly_PreviewTextInput;
            Label5.Visibility = Visibility.Visible;
            Label5.Content = "Цена";
            TextBox5.Visibility = Visibility.Visible;
            TextBox5.Text = model.Price.ToString();
            TextBox5.PreviewTextInput += DigitsOnly_PreviewTextInput;

            BindCommands();
        }
        public EditWindow(BusFleet busFleet, BusOrderSystemContext bos, bool IsNew = true)
        {
            IsSaved = false;
            this.bos = bos;
            this.IsNew = IsNew;
            type = VarType.BusFleet;
            if (IsNew)
            {
                fleet = busFleet;
                fleet.FleetName = "";
                fleet.Address = "";
                fleet.ContactPhone = "";
                fleet.Email = "";
                fleet.HourlyRate = 0;
                fleet.KilometerRate = 0;
                fleet.Region = "";
            }
            else fleet = busFleet;
            InitializeComponent();
            EditWindow1.Title = IsNew ? "Добавление автопарка" : "Редактирование автопарка";
            Label1.Visibility = Visibility.Visible;
            Label1.Content = "Имя автопарка";
            TextBox1.Visibility = Visibility.Visible;
            TextBox1.Text = fleet.FleetName;
            Label2.Visibility = Visibility.Visible;
            Label2.Content = "Адрес";
            TextBox2.Visibility = Visibility.Visible;
            TextBox2.Text = fleet.Address;
            Label3.Visibility = Visibility.Visible;
            Label3.Content = "Контактный телефон";
            TextBox3.Visibility = Visibility.Visible;
            TextBox3.Text = fleet.ContactPhone;
            TextBox3.PreviewTextInput += DigitsOnly_PreviewTextInput;
            Label4.Visibility = Visibility.Visible;
            Label4.Content = "Email";
            TextBox4.Visibility = Visibility.Visible;
            TextBox4.Text = fleet.Email;
            Label5.Visibility = Visibility.Visible;
            Label5.Content = "Почасовая ставка";
            TextBox5.Visibility = Visibility.Visible;
            TextBox5.Text = fleet.HourlyRate.ToString();
            TextBox6.PreviewTextInput += DigitsOnly_PreviewTextInput;
            Label6.Visibility = Visibility.Visible;
            Label6.Content = "Ставка по километрам";
            TextBox6.Visibility = Visibility.Visible;
            TextBox6.Text = fleet.KilometerRate.ToString();
            TextBox6.PreviewTextInput += DigitsOnly_PreviewTextInput;
            Label7.Visibility = Visibility.Visible;
            Label7.Content = "Регион";
            TextBox7.Visibility = Visibility.Visible;
            TextBox7.Text = fleet.Region;
            TextBox7.PreviewTextInput += ExtTextBox_PreviewTextInput;
            BindCommands();
        }
        public EditWindow(Shift shift, BusOrderSystemContext bos, bool IsNew = true)
        {
            IsSaved = false;
            this.bos = bos;
            this.IsNew = IsNew;
            type = VarType.Shift;
            if (IsNew)
            {
                this.shift = shift;
                this.shift.ShiftName = "";
                this.shift.FleetId = bos.BusFleets.First().FleetId;
                this.shift.StartTime = new();
                this.shift.EndTime = new();
            }
            else this.shift = shift;
            InitializeComponent();
            EditWindow1.Title = IsNew ? "Добавление смены" : "Редактирование смены";
            Label1.Visibility = Visibility.Visible;
            Label1.Content = "Имя смены";
            TextBox1.Visibility = Visibility.Visible;
            TextBox1.Text = shift.ShiftName;
            TextBox1.PreviewTextInput += LettersOnly_PreviewTextInput;
            Label2.Visibility = Visibility.Visible;
            Label2.Content = "Id Автопарка";
            ComboBox1.Visibility = Visibility.Visible;
            ComboBox1.ItemsSource = bos.BusFleets.Select(bf => bf.FleetId).ToList();
            ComboBox1.SelectedItem = shift.FleetId;
            Label5.Visibility = Visibility.Visible;
            Label5.Content = "Время начала";
            TextBox5.Visibility = Visibility.Visible;
            TextBox5.Text = shift.StartTime.ToString();
            Label6.Visibility = Visibility.Visible;
            Label6.Content = "Время окончания";
            TextBox6.Visibility = Visibility.Visible;
            TextBox6.Text = shift.EndTime.ToString();
            BindCommands();
        }
        public EditWindow(Bus bus, BusOrderSystemContext bos, bool IsNew = true)
        {
            IsSaved = false;
            this.bos = bos;
            this.IsNew = IsNew;
            type = VarType.Bus;
            if (IsNew)
            {
                this.bus = bus;
                this.bus.BusNumber = "";
                this.bus.ModelId = bos.BusModels.First().ModelId;
                this.bus.FleetId = bos.BusFleets.First().FleetId;
                this.bus.Mileage = 0;
            }
            else this.bus = bus;
            InitializeComponent();
            EditWindow1.Title = IsNew ? "Добавление автобуса" : "Редактирование автобуса";
            Label1.Visibility = Visibility.Visible;
            Label1.Content = "Номер автобуса";
            TextBox1.Visibility = Visibility.Visible;
            TextBox1.Text = bus.BusNumber;
            Label2.Visibility = Visibility.Visible;
            Label2.Content = "Id Модели";
            ComboBox1.Visibility = Visibility.Visible;
            ComboBox1.ItemsSource = bos.BusModels.Select(bm => bm.ModelId).ToList();
            ComboBox1.SelectedItem = bus.ModelId;
            Label3.Visibility = Visibility.Visible;
            Label3.Content = "Id Автопарка";
            ComboBox2.Visibility = Visibility.Visible;
            ComboBox2.ItemsSource = bos.BusFleets.Select(bf => bf.FleetId).ToList();
            ComboBox2.SelectedItem = bus.FleetId;
            Label4.Visibility = Visibility.Visible;
            Label4.Content = "Пробег";
            TextBox4.Visibility = Visibility.Visible;
            TextBox4.Text = bus.Mileage.ToString();
            BindCommands();
        }
        public EditWindow(Driver driver, BusOrderSystemContext bos, bool IsNew = true)
        {
            IsSaved = false;
            this.bos = bos;
            this.IsNew = IsNew;
            type = VarType.Driver;
            if (IsNew)
            {
                this.driver = driver;
                this.driver.FirstName = "";
                this.driver.LastName = "";
                this.driver.FleetId = bos.BusFleets.First().FleetId;
                this.driver.EmploymentDate = DateOnly.FromDateTime(DateTime.Now);
                this.driver.ExperienceYears = 0;
            }
            else this.driver = driver;
            InitializeComponent();
            EditWindow1.Title = IsNew ? "Добавление водителя" : "Редактирование водителя";
            Label1.Visibility = Visibility.Visible;
            Label1.Content = "Имя";
            TextBox1.Visibility = Visibility.Visible;
            TextBox1.Text = driver.FirstName;
            Label2.Visibility = Visibility.Visible;
            Label2.Content = "Фамилия";
            TextBox2.Visibility = Visibility.Visible;
            TextBox2.Text = driver.LastName;
            Label3.Visibility = Visibility.Visible;
            Label3.Content = "Id Автопарка";
            ComboBox2.Visibility = Visibility.Visible;
            ComboBox2.ItemsSource = bos.BusFleets.Distinct().Select(bf => bf.FleetId).ToList();
            ComboBox2.SelectedItem = driver.FleetId;
            Label5.Visibility = Visibility.Visible;
            Label5.Content = "Дата найма";
            DatePicker1.Visibility = Visibility.Visible;
            DatePicker1.Text = driver.EmploymentDate.ToString();
            Label6.Visibility = Visibility.Visible;
            Label6.Content = "Стаж работы(лет)";
            TextBox6.Visibility = Visibility.Visible;
            TextBox6.Text = driver.ExperienceYears.ToString();
            BindCommands();
        }
        public EditWindow(Customer customer, BusOrderSystemContext bos, bool IsNew = true)
        {
            IsSaved = false;
            this.bos = bos;
            type = VarType.Customer;
            if (IsNew)
            {
                this.customer = customer;
                this.customer.Name = "";
                this.customer.Phone = "";
                this.customer.Email = "";
            }
            else this.customer = customer;
            InitializeComponent();
            EditWindow1.Title = IsNew ? "Добавление клиента" : "Редактирование клиента";
            Label1.Visibility = Visibility.Visible;
            Label1.Content = "Имя";
            TextBox1.Visibility = Visibility.Visible;
            TextBox1.Text = customer.Name;
            Label2.Visibility = Visibility.Visible;
            Label2.Content = "Контактный телефон";
            TextBox2.Visibility = Visibility.Visible;
            TextBox2.Text = customer.Phone;
            Label3.Visibility = Visibility.Visible;
            Label3.Content = "Email";
            TextBox3.Visibility = Visibility.Visible;
            TextBox3.Text = customer.Email;
            BindCommands();
        }
        public EditWindow(Order order, BusOrderSystemContext bos, bool IsNew = true)
        {
            IsSaved = false;
            this.bos = bos;
            type = VarType.Order;
            if (IsNew)
            {
                this.order = order;
                this.order.BusId = bos.Buses.First().BusId;
                this.order.CustomerId = bos.Customers.First().CustomerId;
                this.order.DriverId = bos.Drivers.First().DriverId;
                this.order.StartTime = DateTime.Now;
                this.order.EndTime = DateTime.Now;
                this.order.DistanceKm = 0;
            }
            else this.order = order;
            InitializeComponent();
            EditWindow1.Title = IsNew ? "Добавление заказа" : "Редактирование заказа";
            Label2.Visibility = Visibility.Visible;
            Label2.Content = "Id автобуса";
            ComboBox1.Visibility = Visibility.Visible;
            ComboBox1.ItemsSource = bos.Buses.Distinct().Select(b => b.BusId).ToList();
            ComboBox1.SelectedItem = order.BusId;
            Label3.Visibility = Visibility.Visible;
            Label3.Content = "Id Клиента";
            ComboBox2.Visibility = Visibility.Visible;
            ComboBox2.ItemsSource = bos.Customers.Distinct().Select(c => c.CustomerId).ToList();
            ComboBox2.SelectedItem = order.CustomerId;
            Label4.Visibility = Visibility.Visible;
            Label4.Content = "Id Водителя";
            ComboBox3.Visibility = Visibility.Visible;
            ComboBox3.ItemsSource = bos.Drivers.Distinct().Select(d => d.DriverId).ToList();
            ComboBox3.SelectedItem = order.DriverId;
            Label5.Visibility = Visibility.Visible;
            Label5.Content = "Время начала";
            DatePicker1.Visibility = Visibility.Visible;
            DatePicker1.Text = DateOnly.FromDateTime(order.StartTime).ToString();
            TextBox5.Visibility = Visibility.Visible;
            TextBox5.Text = TimeOnly.FromDateTime(order.StartTime).ToString();
            Label6.Visibility = Visibility.Visible;
            Label6.Content = "Время окончания";
            DatePicker2.Visibility = Visibility.Visible;
            DatePicker2.Text = DateOnly.FromDateTime(order.EndTime).ToString();
            TextBox6.Visibility = Visibility.Visible;
            TextBox6.Text = TimeOnly.FromDateTime(order.EndTime).ToString();
            Label7.Visibility = Visibility.Visible;
            Label7.Content = "Дистанция поездки";
            TextBox7.Visibility = Visibility.Visible;
            TextBox7.Text = order.DistanceKm.ToString();
            BindCommands();
        }
        private void SaveClick(object sender, RoutedEventArgs e)
        {
            IsSaved = true;
            switch (type)
            {
                case VarType.BusModel:
                    {
                        model.ModelName = TextBox1.Text;
                        model.ManufacturerCompany = TextBox2.Text;
                        model.ManufacturerCountry = TextBox3.Text;
                        model.Capacity = int.Parse(TextBox4.Text);
                        model.Price = int.Parse(TextBox5.Text);
                        break;
                    }
                case VarType.BusFleet:
                    {
                        fleet.FleetName = TextBox1.Text;
                        fleet.Address = TextBox2.Text;
                        fleet.ContactPhone = TextBox3.Text;
                        fleet.Email = TextBox4.Text;
                        fleet.HourlyRate = decimal.Parse(TextBox5.Text);
                        fleet.KilometerRate = decimal.Parse(TextBox6.Text);
                        fleet.Region = TextBox7.Text;
                        break;
                    }
                case VarType.Shift:
                    {
                        shift.ShiftName = TextBox1.Text;
                        shift.FleetId = (int)ComboBox1.SelectedValue;
                        shift.StartTime = TimeOnly.Parse(TextBox5.Text);
                        shift.EndTime = TimeOnly.Parse(TextBox6.Text);
                        break;
                    }
                case VarType.Bus:
                    {
                        bus.BusNumber = TextBox1.Text;
                        bus.ModelId = (int)ComboBox1.SelectedValue;
                        bus.FleetId = (int)ComboBox2.SelectedValue;
                        bus.Mileage = int.Parse(TextBox4.Text);
                        break;
                    }
                case VarType.Driver:
                    {
                        driver.FirstName = TextBox1.Text;
                        driver.LastName = TextBox2.Text;
                        driver.FleetId = (int)ComboBox2.SelectedValue;
                        driver.EmploymentDate = DateOnly.Parse(DatePicker1.Text);
                        driver.ExperienceYears = int.Parse(TextBox6.Text);
                        break;
                    }
                case VarType.Customer:
                    {
                        customer.Name = TextBox1.Text;
                        customer.Phone = TextBox2.Text;
                        customer.Email = TextBox3.Text;
                        break;
                    }
                case VarType.Order:
                    {
                        order.BusId = (int)ComboBox1.SelectedValue;
                        order.CustomerId = (int)ComboBox2.SelectedValue;
                        order.DriverId = (int)ComboBox3.SelectedValue;
                        order.StartTime = DateOnly.Parse(DatePicker1.Text).ToDateTime(TimeOnly.Parse(TextBox5.Text));
                        order.EndTime = DateOnly.Parse(DatePicker2.Text).ToDateTime(TimeOnly.Parse(TextBox6.Text));
                        order.DistanceKm = int.Parse(TextBox7.Text);
                        break;
                    }
            }
        }
        private void EditWindow1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.DialogResult = IsSaved;
        }
        void CanExecuteSave(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
            switch(type)
            {
                case VarType.BusModel:
                {
                        if(string.IsNullOrEmpty(TextBox1.Text)) e.CanExecute = false;
                        if(string.IsNullOrEmpty(TextBox4.Text)) e.CanExecute = false;
                        if(string.IsNullOrEmpty(TextBox5.Text)) e.CanExecute = false;
                        break;
                }
                case VarType.BusFleet:
                    {
                        if (string.IsNullOrEmpty(TextBox1.Text)) e.CanExecute = false;
                        if (string.IsNullOrEmpty(TextBox5.Text)) e.CanExecute = false;
                        if (string.IsNullOrEmpty(TextBox6.Text)) e.CanExecute = false;
                        if (string.IsNullOrEmpty(TextBox7.Text)) e.CanExecute = false;
                        break;
                    }
                case VarType.Shift:
                    {
                        if (string.IsNullOrEmpty(TextBox1.Text)) e.CanExecute = false;
                        if (ComboBox1.SelectedIndex == -1) e.CanExecute = false;
                        if (string.IsNullOrEmpty(TextBox5.Text)) e.CanExecute = false;
                        if (string.IsNullOrEmpty(TextBox6.Text)) e.CanExecute = false;
                        break;
                    }
                case VarType.Bus:
                    {
                        if (string.IsNullOrEmpty(TextBox1.Text)) e.CanExecute = false;
                        if (ComboBox1.SelectedIndex == -1) e.CanExecute = false;
                        if (ComboBox2.SelectedIndex == -1) e.CanExecute = false;
                        if (string.IsNullOrEmpty(TextBox4.Text)) e.CanExecute = false;
                        break;
                    }
                case VarType.Driver:
                    {
                        if (string.IsNullOrEmpty(TextBox1.Text)) e.CanExecute = false;
                        if (string.IsNullOrEmpty(TextBox2.Text)) e.CanExecute = false;
                        if (ComboBox2.SelectedIndex == -1) e.CanExecute = false;
                        if (string.IsNullOrEmpty(DatePicker1.Text)) e.CanExecute = false;
                        if (string.IsNullOrEmpty(TextBox6.Text)) e.CanExecute = false;
                        break;
                    }
                case VarType.Customer:
                    {
                        if (string.IsNullOrEmpty(TextBox1.Text)) e.CanExecute = false;
                        if (string.IsNullOrEmpty(TextBox2.Text)) e.CanExecute = false;
                        break;
                    }
                case VarType.Order:
                    {
                        if (ComboBox1.SelectedIndex == -1) e.CanExecute = false;
                        if (ComboBox2.SelectedIndex == -1) e.CanExecute = false;
                        if (ComboBox3.SelectedIndex == -1) e.CanExecute = false;
                        if (string.IsNullOrEmpty(TextBox5.Text)) e.CanExecute = false;
                        if (string.IsNullOrEmpty(DatePicker1.Text)) e.CanExecute = false;
                        if (string.IsNullOrEmpty(TextBox6.Text)) e.CanExecute = false;
                        if (string.IsNullOrEmpty(DatePicker2.Text)) e.CanExecute = false;
                        if (string.IsNullOrEmpty(TextBox7.Text)) e.CanExecute = false;
                        break;
                    }
            }
        }
        public void CanExecuteCancel(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;

            switch (type)
            {
                case VarType.BusModel:
                    {
                        if (TextBox1.Text != model.ModelName) e.CanExecute = true;
                        if (TextBox2.Text != model.ManufacturerCompany) e.CanExecute = true;
                        if (TextBox3.Text != model.ManufacturerCountry) e.CanExecute = true;
                        if (TextBox4.Text != model.Capacity.ToString()) e.CanExecute = true;
                        if (TextBox5.Text != model.Price.ToString()) e.CanExecute = true;
                        break;
                    }
                case VarType.BusFleet:
                    {
                        if (TextBox1.Text != fleet.FleetName) e.CanExecute = true;
                        if (TextBox2.Text != fleet.Address) e.CanExecute = true;
                        if (TextBox3.Text != fleet.ContactPhone) e.CanExecute = true;
                        if (TextBox4.Text != fleet.Email) e.CanExecute = true;
                        if (TextBox5.Text != fleet.HourlyRate.ToString()) e.CanExecute = true;
                        if (TextBox6.Text != fleet.KilometerRate.ToString()) e.CanExecute = true;
                        if (TextBox7.Text != fleet.Region) e.CanExecute = true;
                        break;
                    }
                case VarType.Shift:
                    {
                        if (TextBox1.Text != shift.ShiftName) e.CanExecute = true;

                        if (ComboBox1.SelectedValue is int selectedFleetId && selectedFleetId != shift.FleetId)
                            e.CanExecute = true;

                        if (TextBox5.Text != shift.StartTime.ToString()) e.CanExecute = true;
                        if (TextBox6.Text != shift.EndTime.ToString()) e.CanExecute = true;
                        break;
                    }
                case VarType.Bus:
                    {
                        if (TextBox1.Text != bus.BusNumber) e.CanExecute = true;

                        if (ComboBox1.SelectedValue is int selectedModelId && selectedModelId != bus.ModelId)
                            e.CanExecute = true;

                        if (ComboBox2.SelectedValue is int selectedFleetId && selectedFleetId != bus.FleetId)
                            e.CanExecute = true;

                        if (TextBox4.Text != bus.Mileage.ToString()) e.CanExecute = true;
                        break;
                    }
                case VarType.Driver:
                    {
                        if (TextBox1.Text != driver.FirstName) e.CanExecute = true;
                        if (TextBox2.Text != driver.LastName) e.CanExecute = true;

                        if (ComboBox2.SelectedValue is int selectedFleetId && selectedFleetId != driver.FleetId)
                            e.CanExecute = true;

                        if (DatePicker1.Text != driver.EmploymentDate.ToString()) e.CanExecute = true;
                        if (TextBox6.Text != driver.ExperienceYears.ToString()) e.CanExecute = true;
                        break;
                    }
                case VarType.Customer:
                    {
                        if (TextBox1.Text != customer.Name) e.CanExecute = true;
                        if (TextBox2.Text != customer.Phone) e.CanExecute = true;
                        if (TextBox3.Text != customer.Email) e.CanExecute = true;
                        break;
                    }
                case VarType.Order:
                    {
                        if (ComboBox1.SelectedValue is int selectedBusId && selectedBusId != order.BusId)
                            e.CanExecute = true;

                        if (ComboBox2.SelectedValue is int selectedCustomerId && selectedCustomerId != order.CustomerId)
                            e.CanExecute = true;

                        if (ComboBox3.SelectedValue is int selectedDriverId && selectedDriverId != order.DriverId)
                            e.CanExecute = true;

                        if (DatePicker1.Text != DateOnly.FromDateTime(order.StartTime).ToString()) e.CanExecute = true;
                        if (TextBox5.Text != TimeOnly.FromDateTime(order.StartTime).ToString()) e.CanExecute = true;
                        if (DatePicker2.Text != DateOnly.FromDateTime(order.EndTime).ToString()) e.CanExecute = true;
                        if (TextBox6.Text != TimeOnly.FromDateTime(order.EndTime).ToString()) e.CanExecute = true;
                        if (TextBox7.Text != order.DistanceKm.ToString()) e.CanExecute = true;
                        break;
                    }
            }
        }
        void CancelClick(object sender, RoutedEventArgs e)
        {
            switch (type)
            {
                case VarType.BusModel:
                    {
                        TextBox1.Text = model.ModelName;
                        TextBox2.Text = model.ManufacturerCompany;
                        TextBox3.Text = model.ManufacturerCountry;
                        TextBox4.Text = model.Capacity.ToString();
                        TextBox5.Text = model.Price.ToString();
                        break;
                    }
                case VarType.BusFleet:
                    {
                        TextBox1.Text = fleet.FleetName;
                        TextBox2.Text = fleet.Address;
                        TextBox3.Text = fleet.ContactPhone;
                        TextBox4.Text = fleet.Email;
                        TextBox5.Text = fleet.HourlyRate.ToString();
                        TextBox6.Text = fleet.KilometerRate.ToString();
                        TextBox7.Text = fleet.Region;
                        break;
                    }
                case VarType.Shift:
                    {
                        TextBox1.Text = shift.ShiftName;
                        ComboBox1.SelectedValue = shift.FleetId;
                        TextBox5.Text = shift.StartTime.ToString();
                        TextBox6.Text = shift.EndTime.ToString();
                        break;
                    }
                case VarType.Bus:
                    {
                        TextBox1.Text = bus.BusNumber;
                        ComboBox1.SelectedValue = bus.ModelId;
                        ComboBox2.SelectedValue = bus.FleetId;
                        TextBox4.Text = bus.Mileage.ToString();
                        break;
                    }
                case VarType.Driver:
                    {
                        TextBox1.Text = driver.FirstName;
                        TextBox2.Text = driver.LastName;
                        ComboBox2.SelectedValue = driver.FleetId;
                        DatePicker1.Text = driver.EmploymentDate.ToString();
                        TextBox6.Text = driver.ExperienceYears.ToString();
                        break;
                    }
                case VarType.Customer:
                    {
                        TextBox1.Text = customer.Name;
                        TextBox2.Text = customer.Phone;
                        TextBox3.Text = customer.Email;
                        break;
                    }
                case VarType.Order:
                    {
                        ComboBox1.SelectedValue = order.BusId;
                        ComboBox2.SelectedValue = order.CustomerId;
                        ComboBox3.SelectedValue = order.DriverId;
                        DatePicker1.Text = DateOnly.FromDateTime(order.StartTime).ToString();
                        TextBox5.Text = TimeOnly.FromDateTime(order.StartTime).ToString();
                        DatePicker2.Text = DateOnly.FromDateTime(order.EndTime).ToString();
                        TextBox6.Text = TimeOnly.FromDateTime(order.EndTime).ToString();
                        TextBox7.Text = order.DistanceKm.ToString();
                        break;
                    }
            }
        }
        void BindCommands()
        {
            this.CommandBindings.Add(new CommandBinding(
                ApplicationCommands.Save,
                SaveClick,
                CanExecuteSave
            ));
            this.CommandBindings.Add(new CommandBinding(
                ApplicationCommands.Undo,
                CancelClick,
                CanExecuteCancel
            ));

            cancelButton.Command = ApplicationCommands.Undo;
            saveButton.Command = ApplicationCommands.Save;
        }
        private void LettersOnly_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text, @"^[a-zA-Zа-яА-Я]+$");
        }

        private void ExtTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !System.Text.RegularExpressions.Regex.IsMatch(e.Text, @"^[a-zA-Zа-яА-ЯёЁ\s\-]+$");
        }

        private void DigitsOnly_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text, @"^[0-9+.,]+$");
        }
        private static bool IsTextAllowed(string text, string pattern)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(text, pattern);
        }

    }
}

