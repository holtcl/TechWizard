using Xamarin.Forms;

namespace TechWizard
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            if (HttpAuthHandler.isBearerTokenSet())
            {
                Profile profile = new Profile();
                MainPage = new NavigationPage(profile);
            }
            else
            {
                MainPage = new NavigationPage(new MainPage());
            }
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
