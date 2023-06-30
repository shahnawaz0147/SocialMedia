using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using System.Net.Http;
using System;
using Contracts.DataContracts;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using System.Net.Http.Json;

namespace UserManagementServiceTest
{
    [TestClass]
    public class UserManagementsTest
    {
        // In order to run the below test(s), 
        // please follow the instructions from https://docs.microsoft.com/en-us/microsoft-edge/webdriver-chromium
        // to install Microsoft Edge WebDriver.

        string UserManagementService = "https://localhost:7199/";
        string PostManagementService = "https://localhost:7094/";
        string CommentManagementService = "https://localhost:7133/";


        private EdgeDriver _driver;

        [TestInitialize]
        public void EdgeDriverInitialize()
        {
           
        }

        [TestMethod]
        public async Task TestLogin()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(UserManagementService);

            Contracts.DataContracts.UserModel model = new Contracts.DataContracts.UserModel();
            model.UserName = "admin";
            model.Password = "Password123";


            HttpResponseMessage response = await client.PostAsJsonAsync("api/Users", model);
            if (response.IsSuccessStatusCode)
            {
                Assert.IsTrue(response.IsSuccessStatusCode, "User Logged In");
                var responseContent = await response.Content.ReadAsStringAsync();
                var user = JsonConvert.DeserializeObject<Contracts.DataContracts.UserModel>(responseContent);
                
            }
            else
            {
                Assert.Fail("User Logged In", model.UserName);
            }
        }
        [TestMethod]
        public async Task TestBadLogin()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(UserManagementService);

            Contracts.DataContracts.UserModel model = new Contracts.DataContracts.UserModel();
            model.UserName = "admin";
            model.Password = "Password123@";


            HttpResponseMessage response = await client.PostAsJsonAsync("api/Users", model);
            if (response.IsSuccessStatusCode)
            {
                Assert.IsTrue(response.IsSuccessStatusCode, "User Logged In");
                var responseContent = await response.Content.ReadAsStringAsync();
                var user = JsonConvert.DeserializeObject<Contracts.DataContracts.UserModel>(responseContent);

            }
            else
            {
                Assert.Fail("User Logged In", model.UserName);
            }
        }

        [TestMethod]
        public async Task TestToken()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(UserManagementService);

            Contracts.DataContracts.UserModel model = new Contracts.DataContracts.UserModel();
            model.UserName = "admin";
            model.Password = "Password123";


            HttpResponseMessage response = await client.PostAsJsonAsync("api/Users", model);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var user = JsonConvert.DeserializeObject<Contracts.DataContracts.UserModel>(responseContent);
              
                Assert.IsTrue(!string.IsNullOrEmpty(user.Token), "User Got Token");
                if (string.IsNullOrWhiteSpace(user.Token))
                {
                    Assert.Fail("User Got Token", user.Token);
                }
                
            }
           
        }
        [TestMethod]
        public async Task TestBadToken()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(UserManagementService);

            Contracts.DataContracts.UserModel model = new Contracts.DataContracts.UserModel();
            model.UserName = "admin";
            model.Password = "Password123@";


            HttpResponseMessage response = await client.PostAsJsonAsync("api/Users", model);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var user = JsonConvert.DeserializeObject<Contracts.DataContracts.UserModel>(responseContent);

                Assert.IsTrue(!string.IsNullOrEmpty(user.Token), "User Got Token");
                if (string.IsNullOrWhiteSpace(user.Token))
                {
                    Assert.Fail("User Got Token", user.Token);
                }

            }
            else
            {
                if (string.IsNullOrWhiteSpace(model.Token))
                {
                    Assert.Fail("User Got Token", model.Token);
                }
            }

        }
        [TestCleanup]
        public void EdgeDriverCleanup()
        {
            
        }
    }
}
