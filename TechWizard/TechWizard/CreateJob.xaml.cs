using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TechWizard
{
    //[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateJob : ContentPage
    {
        public CreateJob()
        {
            InitializeComponent();
        }

        private async void submitBtn_Clicked(object sender, EventArgs e)
        {
            Request request = new Request()
            {
                description = DescriptionEnt.Text,
                supportType = SupportPk.SelectedItem.ToString(),
                status = 1,
                openDate = DateTime.Now,
                 //user=,
                //wizard=,
                //acceptDate =,
                //completedDate =
        
        
            };
            var json = JsonConvert.SerializeObject(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpClient client = new HttpClient();

            var result = await client.PostAsync("httpsriojk  ://10.0.2.2:44371/api/Request/", content);

            if (result.StatusCode == HttpStatusCode.Created)
            {
                await DisplayAlert("Your help request has been sent", "A Wizard will reach out to you shortly");
            }

            await Navigation.PushAsync(new MainPage());

        }

        private Task DisplayAlert(string v1, string v2)
        {
            throw new NotImplementedException();
        }
    }
}