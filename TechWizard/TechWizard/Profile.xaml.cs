using Plugin.Media;
using System;
using Xamarin.Forms;

namespace TechWizard
{
    public partial class Profile : ContentPage
    {

        public Profile()
        {
            InitializeComponent();

            string fullname = Application.Current.Properties["user_firstname"] + " " + Application.Current.Properties["user_lastname"];
            namelabel.Text = fullname;
            phonelabel.Text = Application.Current.Properties["user_phone"].ToString();
            emaillabel.Text = Application.Current.Properties["user_email"].ToString();

            addresslabel.Text = Application.Current.Properties["user_address"] + "\n" +
            Application.Current.Properties["user_city"] + ", " +
            Application.Current.Properties["user_state"] + " " +
            Application.Current.Properties["user_zip"];

            if (Application.Current.Properties["user_iswizard"] == null || Application.Current.Properties["user_iswizard"] + "" == "")
            {
                HttpAuthHandler.logout(Navigation);
            }
            else
            {
                ViewJobButton.IsVisible = Boolean.Parse(Application.Current.Properties["user_iswizard"] + "");
            }
            //CreateJobButton.IsVisible = ! (bool) Application.Current.Properties["user_iswizard"];
        }
        private async void ViewJobs_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new WizardJobs());
        }

        private async void CreateJobs_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CreateJob());
        }
        private async void ChangePic_OnClicked(object sender, EventArgs e)
        {

            await CrossMedia.Current.Initialize();
        }

        private async void ViewJobsAsUser_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Jobs());
        }

        private async void Logout_Clicked(object sender, EventArgs e)
        {
            await HttpAuthHandler.logout(Navigation);
        }


    }
}