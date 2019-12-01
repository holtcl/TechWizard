using System;
using System.Collections.Generic;

using Xamarin.Forms;

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
            await Navigation.PushAsync(new Profile());
        }
    }
}
