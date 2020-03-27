using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using TechWizard.Models;
using Xamarin.Forms;

namespace TechWizard
{
    //[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WizardJobs : ContentPage
    {
        List<JobListObject> JobsForDisplay { get; set; }

        public WizardJobs()
        {
            InitializeComponent();

            populateJobs();

        }

        private async void populateJobs()
        {
            HttpClient client = new HttpClient();
            client = HttpAuthHandler.addTokenHeader(client);

            var requestsResponse = await client.GetAsync(HttpAuthHandler.API_URL + "api/WizardJob");

            if (requestsResponse.IsSuccessStatusCode)
            {
                string data = await requestsResponse.Content.ReadAsStringAsync();
                JobsForDisplay = JsonConvert.DeserializeObject<List<JobListObject>>(data);
                collectionView.ItemsSource = JobsForDisplay;
            }
        }

        private async void collectionView_SelectionChangedAsync(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.Count == 0)
                return;

            await Navigation.PushAsync(new JobPull((JobListObject)e.CurrentSelection[0],true));
        }

        private void ContentPage_Appearing(object sender, EventArgs e)
        {
            collectionView.SelectedItem = null;
        }
    }
}