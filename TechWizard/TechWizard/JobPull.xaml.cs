using System;
using System.Net.Http;
using TechWizard.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

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

            if (isReferredFromWizardJobPage)
            {
                acceptButton.IsVisible = bool.Parse(Application.Current.Properties["user_iswizard"] + "");
                acceptButton.IsVisible &= thisRequest.acceptDate == null;
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

            if (thisRequest.acceptDate == null)
            {
                jobDateStartedHeaderLabel.IsVisible = false;
                jobDateStartedLabel.IsVisible = false;
            }
            else
            {
                jobDateStartedLabel.Text = thisRequest.acceptDate.Value.ToString("MMMM d, yyyy") +
                    " (" + calculateDaysAgo(thisRequest.acceptDate.Value) + ")";
            }

            if (thisRequest.completedDate == null)
            {
                jobDateCompletedHeaderLabel.IsVisible = false;
                jobDateCompletedLabel.IsVisible = false;
            }
            else
            {
                jobDateCompletedLabel.Text = thisRequest.completedDate.Value.ToString("MMMM d, yyyy") +
                    " (" + calculateDaysAgo(thisRequest.completedDate.Value) + ")";
            }
        }

        private void populateContactSection() 
        {
            if (hasContactInfo)
            {
                contactHeader.Text = (contact.isWizard ? "Wizard " : "Client ") + "Contact Info";
                contactContactTypeLabel.Text = thisRequest.contactMethod;
                contactFirstName.Text = contact.FirstName;

                if (thisRequest.contactMethod == "Phone") contactContactInfo.Text = contact.Phone;
                else if (thisRequest.contactMethod == "Email") contactContactInfo.Text = contact.Email;
                else if (thisRequest.contactMethod == "In-Person") contactContactInfo.Text = contact.Address +
                        "\n" + contact.City + ", " + contact.State + " " + contact.Zip;
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

                HttpClient client = new HttpClient();
                client = HttpAuthHandler.addTokenHeader(client);

                HttpResponseMessage requestsResponse = await client.PostAsync(HttpAuthHandler.API_URL + "api/WizardJob/"+thisRequest.requestID, null);


                if (requestsResponse.IsSuccessStatusCode && bool.Parse(await requestsResponse.Content.ReadAsStringAsync()))
                {
                    await DisplayAlert("Job Accepted!", "Check your orders page for the job info!", "Ok");
                    acceptButton.IsVisible = false;
                } else
                {
                    await DisplayAlert("Error!", "Something went wrong processing your request.  Please try again later", "Ok");
                }
            }
        }
    }
}