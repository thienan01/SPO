using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SPO
{
    public static class SPOTest
    {
        public static async Task Run()
        {
            string siteUrl = "https://75v04z.sharepoint.com";
            string azureAdTenantId = "df0b6f54-2a96-44cf-b4e0-090cf88d18e8";
            string clientId = "2c0ab9bc-5c11-4b7b-867f-68fef23c53f8";
            string clientSecret = "jX08Q~qnvf7wElb1FeEjylXzXqv93iODESRO9bUm";

            string authority = $"https://login.microsoftonline.com/{azureAdTenantId}";
            string resource = "https://75v04z.sharepoint.com";

            string accessToken = await GetAccessToken(authority, resource, clientId, clientSecret);
            Console.WriteLine("token: " + accessToken);
            await CreateSiteCollection(siteUrl, accessToken);
        }

        public static async Task<string> GetAccessToken(string authority, string resource, string clientId, string clientSecret)
        {
            using (var httpClient = new HttpClient())
            {
                var tokenEndpoint = $"{authority}/oauth2/token";
                var content = new FormUrlEncodedContent(new[]
                {
                new KeyValuePair<string, string>("grant_type", "client_credentials"),
                new KeyValuePair<string, string>("client_id", clientId),
                new KeyValuePair<string, string>("client_secret", clientSecret),
                new KeyValuePair<string, string>("resource", resource)
            });

                var response = await httpClient.PostAsync(tokenEndpoint, content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Failed to retrieve access token: {responseContent}");
                }

                dynamic tokenResult = Newtonsoft.Json.JsonConvert.DeserializeObject(responseContent);
                return tokenResult.access_token;
            }
        }

        public static async Task CreateSiteCollection(string siteUrl, string accessToken)
        {
            //using (var httpClient = new HttpClient())
            //{
            //    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            //    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //    var endpointUrl = $"{siteUrl}/_api/site/create";
            //    var requestContent = new StringContent("{\"parameters\":{\"__metadata\":{\"type\":\"SP.SiteCreationRequest\"},\"Title\":\"AnleTestSite\",\"Url\":\"/sites/AnleTestSite\",\"Template\":\"STS#0\",\"Owner\":\"thienanhello@75v04z.onmicrosoft.com\"}}");
            //    requestContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            //    var response = await httpClient.PostAsync(endpointUrl, requestContent);
            //    var responseContent = await response.Content.ReadAsStringAsync();

            //    if (!response.IsSuccessStatusCode)
            //    {
            //        throw new Exception($"Failed to create site collection: {responseContent}");
            //    }

            //    Console.WriteLine("Site collection created successfully.");
            //}

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var endpointUrl = $"{siteUrl}/_api/SPSiteManager/create";

                // Define the properties for the new site collection
                var requestBody = new
                {
                    request = new
                    {
                        Title = "New Site Collection",
                        Url = "/sites/newsitecollection",
                        Template = "STS#0", // Template for Team Site
                        Owner = "thienanhello@75v04z.onmicrosoft.com",
                        StorageMaximumLevel = 100,
                        UserCodeMaximumLevel = 100
                    }
                };

                var jsonRequestBody = Newtonsoft.Json.JsonConvert.SerializeObject(requestBody);
                var requestContent = new StringContent(jsonRequestBody, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync(endpointUrl, requestContent);

                if (!response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Failed to create site collection: {responseContent}");
                }
            }
            }
    }
}
