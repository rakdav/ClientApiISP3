using ClientApi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
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

namespace ClientApi
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private HttpClient client;
        public MainWindow()
        {
            InitializeComponent();
            client = new HttpClient();
            Task.Run(() => Load());
        }
        private async Task Load()
        {
            List<User>? list = await client.GetFromJsonAsync<List<User>>
                ("https://localhost:7063/api/users");
            Dispatcher.Invoke(()=>ListUsers.ItemsSource=list);
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            await Save();
        }
        private async Task Save()
        {
            UserWithooutId user = new UserWithooutId();
            user.Name = NameUser.Text;
            user.Age = int.Parse(AgeUser.Text);
            JsonContent content = JsonContent.Create(user);
            using var response = await client.
                PostAsync("https://localhost:7063/api/users", content);
            string responseText = await response.Content.ReadAsStringAsync();
        }
    }
}
