using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TechWizard
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {        
        public MainPage()
        {
            InitializeComponent();
        }

        private async void SignUpButton_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SignupPage());
        }

        private async void LoginButton_OnClicked(object sender, EventArgs e)
        {
            HttpClient httpClient = new HttpClient();

            var parameters = new Dictionary<string, string> { {"grant_type", "password" },{"UserName", email_entry.Text },{ "Password", password_entry.Text } };
            var content = new FormUrlEncodedContent(parameters);            

            var result = await httpClient.PostAsync("http://10.0.2.2:44371/token", content);

            if (result.StatusCode == HttpStatusCode.OK)
            {
                var jsonString = await result.Content.ReadAsStringAsync();

                JsonResponse jr = JsonConvert.DeserializeObject<JsonResponse>(jsonString);
                HttpAuthHandler.SetBearerToken(jr.access_token);
            }
            else
            {
                await DisplayAlert("Oops!", "Invalid username/password combo.", "Ok");
            }
            //       User user = new User (email_entry.text, password_entry.text);
        }

        private class JsonResponse
        {
            public string access_token { get; set; }
            public string token_type { get; set; }
            public int expires_in { get; set; }
        }
    }
}
