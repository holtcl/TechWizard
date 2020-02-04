using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TechWizard
{
    public partial class Profile : ContentPage
    {
        public Profile()
        {
            InitializeComponent();
        }
        private async void ViewJobs_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Jobs());
        }

        private async void CreateJobs_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CreateJob());
        }
        private async void ChangePic_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CreateJob());
        }
    }
}