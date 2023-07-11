using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPO
{
    public static class setting
    {
        public static string tenantId { get; set; } = "df0b6f54-2a96-44cf-b4e0-090cf88d18e8";
        public static string siteUrl { get; set; } = "https://75v04z.sharepoint.com";

        public static string clientId { get; set; } = "45c1e6d5-0f9c-4627-a8de-98cf4b535890";
        public static string clientSecret { get; set; } = "4bl8Q~y7ozKsBCOoZoHsKEcZQvY1x7F2SZUeFaq9";
        public static string endPoint { get; set; } = "https://login.microsoftonline.com/df0b6f54-2a96-44cf-b4e0-090cf88d18e8/oauth2/v2.0/token";

        public static string scope { get; set; } = "https://75v04z.sharepoint.com/Sites.FullControl.All";


        public static string userName { get; set; } = "thienanhello@75v04z.onmicrosoft.com";
        public static string password { get; set; } = "newpasS160201!!!";




    }
}
