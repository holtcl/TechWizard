using System;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

using Xamarin.Forms;
using System.Net;
using System.Text.RegularExpressions;

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
            if (!isValidEmail(EmailEnt.Text)) 
            {
                await DisplayAlert("Error", "Email is not valid.", "OK");
                
                return;
            }
            
            //todo: validate zip
            
            
            //todo: validate phone

            
            //todo: validate password

            
            //todo: validate zip


            User user = new User()
            {
                UserName = EmailEnt.Text,
                FirstName = FirstNameEnt.Text,
                LastName = LastNameEnt.Text,
                Address = Address_ent.Text,
                City = City_ent.Text,
                State = state_ent.SelectedItem.ToString(),
                Zip = Int32.Parse(zip_ent.Text),
                Email = EmailEnt.Text,
                Phone = Phone_ent.Text,
                Password = PasswordEnt.Text
            };

            
            var json = JsonConvert.SerializeObject(user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpClient client = new HttpClient();
            
            var result = await client.PostAsync(HttpAuthHandler.API_URL + "api/User/", content);

            var loginsuccess = await HttpAuthHandler.login(user.UserName, user.Password);
            
            if (result.StatusCode == HttpStatusCode.Created && loginsuccess)
            {
                await DisplayAlert("Success", "Your account has been created", "Find a Wizard");
            }
            else {
                await DisplayAlert("Error", result.StatusCode + result.ReasonPhrase, "ok");
            }
            await Navigation.PushAsync(new Profile());



            //await Navigation.PushAsync(new Profile());

        }

        private bool isValidEmail(string email) { 
            // email pattern from emailregex.com
            var emailPattern = "^(?(\")(\".+?(?<!\\\\)\"@)|(([0-9a-z]((\\.(?!\\.))|[-!#\\$%&'\\*\\+/=\\?\\^`\\{\\}\\|~\\w])*)(?<=[0-9a-z])@))(?(\\[)(\\[(\\d{1,3}\\.){3}\\d{1,3}\\])|(([0-9a-z][-\\w]*[0-9a-z]*\\.)+[a-z0-9][\\-a-z0-9]{0,22}[a-z0-9]))$";

            return Regex.IsMatch(email, emailPattern);   
        }

        private bool isValidPassword(string password) {

            return false;
        }
    }
}
