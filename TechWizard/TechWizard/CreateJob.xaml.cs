using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TechWizard
{
    //[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateJob : ContentPage
    {
        private Dictionary<string, int> skillMap = new Dictionary<string, int>();
        private Dictionary<string, int> contactMethodMap = new Dictionary<string, int>();
        public CreateJob()
        {
            InitializeComponent();

            populateDropdowns();
        }

        private async void populateDropdowns() {


            HttpClient client = new HttpClient();
            client = HttpAuthHandler.addTokenHeader(client);

            var skillsResponse = await client.GetAsync(HttpAuthHandler.API_URL + "api/Skills");
            var contactMethodsResponse = await client.GetAsync(HttpAuthHandler.API_URL + "api/contactMethods");

            if (skillsResponse.IsSuccessStatusCode)
            {
                string data = await skillsResponse.Content.ReadAsStringAsync();
                Skill[] skills = JsonConvert.DeserializeObject<Skill[]>(data);

                foreach (Skill skill in skills)
                {
                    SkillEnt.Items.Add(skill.name);
                    skillMap.Add(skill.name, skill.id);
                }
            }

            if (contactMethodsResponse.IsSuccessStatusCode)
            {
                string data = await contactMethodsResponse.Content.ReadAsStringAsync();
                ContactMethod[] contactMethods = JsonConvert.DeserializeObject<ContactMethod[]>(data);

                foreach (ContactMethod contactMethod in contactMethods)
                {
                    ContactEnt.Items.Add(contactMethod.MethodName);
                    contactMethodMap.Add(contactMethod.MethodName, contactMethod.id);
                }
            }
        }

        private int parsePrice(string pricestr) {
            int ioutput = 0;
            double doutput = 0;

            // Make sure the input is at least a number
            if(!Double.TryParse(pricestr, out doutput)) 
                throw new FormatException("Valid price was not entered"); ;

            //check if there's a decimal and whether it has one or two numbers following it
            int decimalIndex = pricestr.IndexOf('.');

            if (decimalIndex == -1)
            {
                Int32.TryParse(pricestr, out ioutput);
                return ioutput*100;
            }
            else {

                switch (pricestr.Length - decimalIndex) {
                    case 1:
                        pricestr = pricestr.Remove(pricestr.Length - 1, 1);
                        Int32.TryParse(pricestr, out ioutput);
                        return (ioutput *100);
                    case 2:
                        pricestr = pricestr.Remove(pricestr.Length - 2, 1);
                        Int32.TryParse(pricestr, out ioutput);
                        return (ioutput*10);
                    case 3:
                        pricestr = pricestr.Remove(pricestr.Length - 3, 1);
                        Int32.TryParse(pricestr, out ioutput);
                        return ioutput;
                    default:
                        throw new FormatException("Valid price was not entered");
                }
            }
        }

        private async void submitBtn_Clicked(object sender, EventArgs e)
        {
            string price = priceEnt.Text;
            Request request;
            try
            {
                request = new Request()
                {
                    title = jobtitleent.Text,
                    description = DescriptionEnt.Text,
                    contactMethod = contactMethodMap[ContactEnt.SelectedItem.ToString()],
                    skill = skillMap[SkillEnt.SelectedItem.ToString()],
                    priceInCents = parsePrice(priceEnt.Text),
                };
            }
            catch (FormatException ex) {
                //make an alert
                return;
            }
            var json = JsonConvert.SerializeObject(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpClient client = HttpAuthHandler.addTokenHeader(new HttpClient());

            var result = await client.PostAsync(HttpAuthHandler.API_URL + "api/Request/", content);

            if (result.StatusCode == HttpStatusCode.Created)
            {
                await DisplayAlert("Your help request has been sent", "A Wizard will reach out to you shortly", "Horray!");
            }

            await Navigation.PushAsync(new MainPage());

        }
    }
}