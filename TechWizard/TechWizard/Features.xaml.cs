using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TechWizard
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Features : ContentPage
    {
        public Features()
        {
            InitializeComponent();
        }
        public async void btn_clicked(object sender, System.EventArgs e)
        {
            try
            {
                var placemark = new Placemark
                {

                    Thoroughfare = entAddress.Text,
                    Locality = entCity.Text,
                    AdminArea = entState.Text,
                    PostalCode = entZip.Text
                };

                await Map.OpenAsync(

                    placemark, new MapLaunchOptions
                    { NavigationMode = NavigationMode.Driving });
                {

                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                await DisplayAlert("Faild", fnsEx.Message, "OK");
            }
            catch (PermissionException pEx)
            {
                await DisplayAlert("Faild", pEx.Message, "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Faild", ex.Message, "OK");
            }
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        async void btnCall_Clicked(object sender, System.EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtNumber.Text))
            {
                await Call(txtNumber.Text);
            }

        }
        public async Task Call(string number)
        {
            try
            {
                PhoneDialer.Open(number);
            }
            catch (ArgumentNullException anEx)
            {
                await DisplayAlert("Not Valid", "Number Entered was not valid", "Failed");
            }
            catch (FeatureNotSupportedException ex)
            {
                await DisplayAlert("Not Supported", "Phone Dialer is not supported on this device", "Failed");
            }
            catch (System.Exception ex)
            {
                await DisplayAlert("???", "Something else has happened", "Failed");
            }
        }
        async void btnSendSms_Clicked(object sender, System.EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtNumber.Text))
            {
                await SendSms(txtMessage.Text, txtNumber.Text);
            }

        }

        public async Task SendSms(string messageText, string recipient)
        {
            try
            {
                var message = new SmsMessage(messageText, recipient);
                await Sms.ComposeAsync(message);
            }
            catch (FeatureNotSupportedException ex)
            {
                await DisplayAlert("Failed", "Sms is not supported on this device.", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Failed", ex.Message, "OK");
            }
        }
    }
}