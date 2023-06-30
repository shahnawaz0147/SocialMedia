using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using System.Net.Http;
using System;
using System.Threading.Tasks;
using System.Threading.Tasks;
using Contracts.DataContracts;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http.Json;

namespace PostManagementServiceTest
{
    [TestClass]
    public class PostsTest
    {
        // In order to run the below test(s), 
        // please follow the instructions from https://docs.microsoft.com/en-us/microsoft-edge/webdriver-chromium
        // to install Microsoft Edge WebDriver.
        string PostManagementService = "https://localhost:7094/";
        private EdgeDriver _driver;

        [TestInitialize]
        public void PostsInitialize()
        {
            
        }

        [TestMethod]
        public async Task TestCorrectCredentials()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:7199/");

            UserModel model = new UserModel();
            model.UserName = "admin";
            model.Password = "Password123";


            HttpResponseMessage response = await client.PostAsJsonAsync("api/Users", model);
            Assert.IsTrue(response.IsSuccessStatusCode, "Not able to Login with these Credentials");
            if (response.IsSuccessStatusCode)
            {
               
                var responseContent = await response.Content.ReadAsStringAsync();
                var user = JsonConvert.DeserializeObject<UserModel>(responseContent);

                HttpClient postsClient = new HttpClient();
                postsClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", user.Token);
                postsClient.BaseAddress = new Uri(PostManagementService);
                response = await postsClient.GetAsync("api/Posts");
                Assert.IsTrue(response.IsSuccessStatusCode, response.StatusCode.ToString());
                if (response.IsSuccessStatusCode)
                {

                }
                
           }
        }

        [TestCleanup]
        public void PostCleanup()
        {
           
        }
    }
}
