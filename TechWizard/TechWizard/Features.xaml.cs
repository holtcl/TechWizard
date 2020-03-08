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
        //MATT!!! aside from the info entered into the first three btn_clicked event handles the rest of the code should be fine for what we need
        public Features()
        {
            InitializeComponent();
        }
        
        //GPS feature- replace entered address info with user info from work request. 
        //Should pull up driving directions from Wizard's Phone (Though according to my emulator the phone is around San Jose @ Google Headquarters)
        //Please note that the variable names: Thoroughfare, Locality, AdminArea, and PostalCode are from Xamarin Essentials and should not be changed
        public async void btnGPS_clicked(object sender, System.EventArgs e)
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

        //SMS feature- replace txtNumber.Text with Phone # of user in work request
        async void btnSMS_Clicked(object sender, System.EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtNumber.Text))
            {
                await SendSms(txtMessage.Text, txtNumber.Text);
            }

        }

        //Phone Dialer- feature replace txtNumber.Text with Phone # of user in work request
        async void btnCall_Clicked(object sender, System.EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtNumber.Text))
            {
                await Call(txtNumber.Text);
            }

        }
        //Email feature- replace txtTo.Text with Email of user in work request. Maybe replace txtSubject.Text with Job Title???? 
        async void btnEmail_Clicked(object sender, System.EventArgs e)
        {
            List<string> toAddress = new List<string>();
            toAddress.Add(txtTo.Text);
            await SendEmail(txtSubject.Text, txtBody.Text, toAddress);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
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

        public async Task SendEmail(string subject, string body, List<string> recipients)
        {
            try
            {
                var message = new EmailMessage
                {
                    Subject = subject,
                    Body = body,
                    To = recipients,
                };
                await Email.ComposeAsync(message);
            }
            catch (FeatureNotSupportedException fbsEx)
            {
                await DisplayAlert("Failed", "Email is not supported on this device.", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Failed", ex.Message, "OK");
            }
        }
    }
}