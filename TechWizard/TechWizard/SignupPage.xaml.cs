using System;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

using Xamarin.Forms;
using System.Net;

namespace TechWizard
{
    public partial class SignupPage : ContentPage
    {
        public SignupPage()
        {
            InitializeComponent();
        }
        private async void SignUpButton_OnClicked(object sender, EventArgs e)
        {
            User user = new User()
            {
                UserName = EmailEnt.Text,
                FirstName = FirstNameEnt.Text,
                LastName = LastNameEnt.Text,
                Email = EmailEnt.Text,
                Password = PasswordEnt.Text
            };
            var json = JsonConvert.SerializeObject(user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpClient client = new HttpClient();

            var result = await client.PostAsync("http://10.0.2.2:44371/api/User/", content);

            if (result.StatusCode == HttpStatusCode.Created)
            {
                await DisplayAlert("Success", "Your account has been created", "Find a Wizard");
            }

            await Navigation.PushAsync(new MainPage());



            //await Navigation.PushAsync(new Profile());

        }
    }
}
