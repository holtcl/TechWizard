using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TechWizard
{
    class HttpAuthHandler
    {
        public static readonly string API_URL = "http://10.0.2.2:44371/";
        private static readonly string[] PersistedValues = {"token", "user_firstname", "user_lastname", "user_id", 
            "user_email", "user_address", "user_city", "user_state", "user_zip", "user_phone", "user_iswizard"};
        
        private static void SetBearerToken(String token) {
            Application.Current.Properties["token"] = token;
        }

        public static bool isBearerTokenSet() {
            return !String.IsNullOrEmpty(Application.Current.Properties["token"].ToString());
        }

        public static HttpClient addTokenHeader(HttpClient httpClient) {
            string token = Application.Current.Properties["token"].ToString();
            
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            return httpClient;
        }

        public static async Task<bool> logout(INavigation Navigation) {
            foreach (string key in PersistedValues) 
            {
                Application.Current.Properties[key] = "";
            }

            await Navigation.PushAsync(new MainPage());

            return true;
        }

        private static async Task<bool> PopulateUserData() {
            HttpClient httpClient = new HttpClient();

            httpClient = addTokenHeader(httpClient);

            var result = await httpClient.GetAsync(API_URL + "api/User/");

            var jsonString = await result.Content.ReadAsStringAsync();

            User user = JsonConvert.DeserializeObject<User>(jsonString);

            Application.Current.Properties["user_firstname"] = user.FirstName;
            Application.Current.Properties["user_lastname"] = user.LastName;
            Application.Current.Properties["user_id"] = user.ID;
            Application.Current.Properties["user_email"] = user.Email;
            Application.Current.Properties["user_address"] = user.Address;
            Application.Current.Properties["user_city"] = user.City;
            Application.Current.Properties["user_state"] = user.State;
            Application.Current.Properties["user_zip"] = user.Zip;
            Application.Current.Properties["user_phone"] = user.Phone;
            Application.Current.Properties["user_iswizard"] = user.isWizard+"";

            return true;
        }

    public static async Task<bool> login(string username, string password)
        {
            HttpClient httpClient = new HttpClient();

            var parameters = new Dictionary<string, string> { { "grant_type", "password" }, { "UserName", username }, { "Password", password } };
            var content = new FormUrlEncodedContent(parameters);
            var result = await httpClient.PostAsync(API_URL + "token", content);

            if (result.StatusCode != HttpStatusCode.OK)
            {
                return false;
            }

            var jsonString = await result.Content.ReadAsStringAsync();

            JsonResponse jr = JsonConvert.DeserializeObject<JsonResponse>(jsonString);
            SetBearerToken(jr.access_token);
            return await PopulateUserData();
        }

        private class JsonResponse
        {
            public string access_token { get; set; }
            public string token_type { get; set; }
            public int expires_in { get; set; }
        }

    }
}
