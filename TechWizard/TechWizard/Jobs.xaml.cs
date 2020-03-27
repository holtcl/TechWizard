using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using TechWizard.Models;
using Xamarin.Forms;

namespace TechWizard
{
    //[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Jobs : ContentPage
    {
        List<JobListObject> JobsForDisplay { get; set; }

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
                JobsForDisplay = JsonConvert.DeserializeObject<List<JobListObject>>(data);
                collectionView.ItemsSource = JobsForDisplay;
            }
        }

        private async void collectionView_SelectionChangedAsync(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.Count == 0)
                return;

            await Navigation.PushAsync(new JobPull((JobListObject)e.CurrentSelection[0]));
        }

        private void ContentPage_Appearing(object sender, EventArgs e)
        {
            collectionView.SelectedItem = null;
        }

        private async void FeaturesButton_Clicked(object sender, EventArgs e)
        {
           // await Navigation.PushAsync(new Features());
        }
    }
}