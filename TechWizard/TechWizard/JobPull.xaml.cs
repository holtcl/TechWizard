using System;
using System.Net.Http;
using TechWizard.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TechWizard
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class JobPull : ContentPage
    {
        private RequestForDisplay thisRequest = new RequestForDisplay();
        private User contact = new User();

        public JobPull()
        {
            InitializeComponent();
        }
        public JobPull(JobListObject jlo) : this(jlo, false) {  }

        public JobPull(JobListObject jlo, bool isReferredFromWizardJobPage)
        {
            thisRequest = jlo.r4d;
            contact = jlo.contact;
            InitializeComponent();
            populateJobSection();
            populateContactSection();

            if (isReferredFromWizardJobPage && thisRequest.completedDate != null)
            {
                acceptButton.IsVisible = bool.Parse(Application.Current.Properties["user_iswizard"] + "");
                acceptButton.IsVisible &= thisRequest.acceptDate == null;

                actionHeader.IsVisible = !acceptButton.IsVisible;
                actionGrid.IsVisible = actionHeader.IsVisible;

                if (thisRequest.hours != null)
                {
                    saveButton.IsVisible = false;
                    hoursWorkedEnt.Text = thisRequest.hours.Value.ToString();
                    hoursWorkedEnt.IsEnabled = false;
                }
            }
            else {
                if (thisRequest.hours != null && thisRequest.completedDate==null)
                {
                    approvalHeader.IsVisible = true;
                    approvalGrid.IsVisible = true;
                    hoursWorkedLabel.Text = thisRequest.hours.Value.ToString();
                    totalCostLabel.Text = thisRequest.totalCost;
                }
            }
        }

        public bool hasContactInfo {
            get {
                return contact.ID != null;
            }
        }

        private void populateJobSection()
        {
            jobTitleLabel.Text = thisRequest.title;
            jobStatusLabel.Text = thisRequest.status;
            jobStatusLabel.BackgroundColor = Color.FromHex(thisRequest.statusColor);
            jobDescriptionLabel.Text = thisRequest.description;
            jobWageLabel.Text = thisRequest.formattedPrice;
            jobContactMethodLabel.Text = thisRequest.contactMethod;
            jobRequiredSkillLabel.Text = thisRequest.skill;
            jobDateOpenedLabel.Text = thisRequest.openDate.ToString("MMMM d, yyyy") +
                " (" + calculateDaysAgo(thisRequest.openDate) + ")";

            if (thisRequest.acceptDate != null)
            {
                jobDateStartedHeaderLabel.IsVisible = true;
                jobDateStartedLabel.IsVisible = true;
                jobDateStartedLabel.Text = thisRequest.acceptDate.Value.ToString("MMMM d, yyyy") +
                    " (" + calculateDaysAgo(thisRequest.acceptDate.Value) + ")";
            }

            if (thisRequest.completedDate != null)
            {
                jobDateCompletedHeaderLabel.IsVisible = true;
                jobDateCompletedLabel.IsVisible = true;
                jobDateCompletedLabel.Text = thisRequest.completedDate.Value.ToString("MMMM d, yyyy") +
                    " (" + calculateDaysAgo(thisRequest.completedDate.Value) + ")";
                finalPriceHeaderLabel.IsVisible = true;
                finalPriceLabel.IsVisible = true;
                finalPriceLabel.Text = thisRequest.totalCost;
            }
        }

        private void populateContactSection() 
        {
            if (hasContactInfo)
            {
                contactHeader.Text = (contact.isWizard ? "Wizard " : "Client ") + "Contact Info";
                contactContactTypeLabel.Text = thisRequest.contactMethod;
                contactFirstName.Text = contact.FirstName;

                if (thisRequest.contactMethod == "Phone")
                {
                    contactContactInfo.Text = contact.Phone;
                    phoneButton.IsVisible = true;

                }
                else if (thisRequest.contactMethod == "Text")
                {
                    contactContactInfo.Text = contact.Phone;
                    textButton.IsVisible = true;
                }
                else if (thisRequest.contactMethod == "Email")
                {
                    contactContactInfo.Text = contact.Email;
                    emailButton.IsVisible = true;
                }

                else if (thisRequest.contactMethod == "In-Person")
                {
                    contactContactInfo.Text = contact.Address +
                        "\n" + contact.City + ", " + contact.State + " " + contact.Zip +
                        "\n" + contact.Phone;
                    phoneButton.IsVisible = true;
                    GPSButton.IsVisible = true;
                }
            }
            else 
            {
                contactHeader.IsVisible = false;
                contactGrid.IsVisible = false;
            }
        }

        private string calculateDaysAgo(DateTime dt)
        {
            int daysago = (int)(DateTime.Now - dt).TotalDays;

            return daysago == 1 ? daysago + " day ago" : daysago + " days ago";
        }

        private async void acceptButton_Clicked(object sender, EventArgs e)
        {
            bool confirmed = await DisplayAlert("Are you sure?", "Would you like to accept this " + thisRequest.skill + " job for " + thisRequest.formattedPrice +" per hour?", "Yes", "No");

            if (confirmed) {

                HttpClient client = HttpAuthHandler.addTokenHeader(new HttpClient());

                HttpResponseMessage requestsResponse = await client.PostAsync(HttpAuthHandler.API_URL + "api/WizardJob/"+thisRequest.requestID,null);


                if (requestsResponse.IsSuccessStatusCode && bool.Parse(await requestsResponse.Content.ReadAsStringAsync()))
                {
                    await DisplayAlert("Job Accepted!", "Check your orders page for the job info!", "Ok");

                    thisRequest.acceptDate = DateTime.Now;

                    Navigation.PopAsync();
                } 
                else
                {
                    await DisplayAlert("Error!", "Something went wrong processing your request.  Please try again later", "Ok");
                }
            }
        }


        private async void GPSButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                var placemark = new Placemark
                {

                    Thoroughfare = contact.Address,
                    Locality = contact.City,
                    AdminArea = contact.State,
                    PostalCode = contact.Zip.ToString()
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
    

        private async void textButton_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(contact.Phone))
            {
                await SendSms("Hello " + contact.FirstName + " this is your TechWizard here to help you with your issue " + thisRequest.title, contact.Phone);
            }
        }

        private async void phoneButton_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(contact.Phone))
            {
                await Call(contact.Phone);
            }
        }

        private async void emailButton_Clicked(object sender, EventArgs e)
        {
            {
                List<string> toAddress = new List<string>();
                toAddress.Add(contact.Email);
                await SendEmail(thisRequest.title, "Hello " +contact.FirstName + "," + "\n", toAddress);
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

        private async void saveButton_Clicked(object sender, EventArgs e)
        {
            HttpClient client = HttpAuthHandler.addTokenHeader(new HttpClient());

            HttpResponseMessage response = await client.PutAsync(HttpAuthHandler.API_URL + "api/WizardJob/" + thisRequest.requestID + "/" + hoursWorkedEnt.Text, null);

            if (response.IsSuccessStatusCode && bool.Parse(await response.Content.ReadAsStringAsync()))
            {
                await DisplayAlert("Job Accepted!", "Please wait for your client to accept the Hours.", "Ok");

                hoursWorkedEnt.IsEnabled = false;
                saveButton.IsVisible = false;
                thisRequest.hours = int.Parse(hoursWorkedEnt.Text);
                jobStatusLabel.Text = thisRequest.status;

            }
            else
            {
                await DisplayAlert("Error!", "Something went wrong processing your request.  Please try again later", "Ok");
            }
        }

        private async void reportButton_Clicked(object sender, EventArgs e)
        {
            List < string > toaddress= new List<string>();

            toaddress.Add("admin@techwizard.net");

            if ((Button)sender == wizReportButton)
            {
                await SendEmail("Report filed by Wizard for Requst Number " + thisRequest.requestID, "describe your issue below:\n\n", toaddress);
            }
            else if ((Button)sender == userReportButton) 
            {
                await SendEmail("Report filed by User for Requst Number " + thisRequest.requestID, "describe your issue below:\n\n", toaddress);
            }
        }

        private async void approveButton_Clicked(object sender, EventArgs e)
        {
            HttpClient client = HttpAuthHandler.addTokenHeader(new HttpClient());

            HttpResponseMessage response = await client.PutAsync(HttpAuthHandler.API_URL + "api/Request/" + thisRequest.requestID, null);

            if (response.IsSuccessStatusCode && bool.Parse(await response.Content.ReadAsStringAsync()))
            {
                await DisplayAlert("Job Approved!", "This is where payment logic would go.", "Ok");

                approvalHeader.IsVisible = false;
                approvalGrid.IsVisible = false;

                thisRequest.completedDate = DateTime.Now;

                jobDateCompletedHeaderLabel.IsVisible = true;
                jobDateCompletedLabel.IsVisible = true;
                jobDateCompletedLabel.Text= thisRequest.completedDate.Value.ToString("MMMM d, yyyy") +
                    " (" + calculateDaysAgo(thisRequest.completedDate.Value) + ")";

                jobStatusLabel.BackgroundColor = Color.FromHex(thisRequest.statusColor);
                jobStatusLabel.Text = thisRequest.status;
            }
            else
            {
                await DisplayAlert("Error!", "Something went wrong processing your request.  Please try again later", "Ok");
            }
        }
    }
}