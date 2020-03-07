using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TechWizard
{
    //[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Jobs : ContentPage
    {
        ArrayList JobsForDisplay;
        
        public Jobs()
        {
            populateJobs();

            InitializeComponent();
        }

        private async void populateJobs()
        {
            HttpClient client = new HttpClient();
            client = HttpAuthHandler.addTokenHeader(client);

            var requestsResponse = await client.GetAsync(HttpAuthHandler.API_URL + "api/Request");

            if (requestsResponse.IsSuccessStatusCode)
            {
                string data = await requestsResponse.Content.ReadAsStringAsync();
                JobsForDisplay = JsonConvert.DeserializeObject<ArrayList>(data);
            }

        }

    }
}