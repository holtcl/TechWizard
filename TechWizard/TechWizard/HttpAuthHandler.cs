using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using Xamarin.Forms;

namespace TechWizard
{
    class HttpAuthHandler
    {
        HttpClient httpClient;
        public HttpAuthHandler()
        {
            httpClient = new HttpClient();
        }

        public static void SetBearerToken(String token) {
            Application.Current.Properties["token"] = token;
        }

    }
}
