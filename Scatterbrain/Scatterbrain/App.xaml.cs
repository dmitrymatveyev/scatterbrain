﻿using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Scatterbrain
{
    public partial class App : Application
    {
        public App()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NDMwNzBAMzEzNjJlMzMyZTMwZmVlUXlneGZ5Qk8xNlVwUjJNS1Q5QUlmWmFqTDAzQ3N1bFZXeXlCODlRVT0=");

            InitializeComponent();

            MainPage = new TheListPage
            {
                BindingContext = new TheListVM()
            };
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
