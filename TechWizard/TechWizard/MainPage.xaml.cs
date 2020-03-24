using System;
using System.ComponentModel;
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
            var success = await HttpAuthHandler.login(email_entry.Text, password_entry.Text);

            if (success)
            {
                Profile profile = new Profile();
                Application.Current.MainPage = profile;
            }
            else
            {
                await DisplayAlert("Oops!", "Invalid username/password combo.", "Ok");
            }

            //       User user = new User (email_entry.text, password_entry.text);
        }
    }
}
