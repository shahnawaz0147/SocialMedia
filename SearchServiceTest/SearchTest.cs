using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using System.Net.Http;
using System;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Contracts.DataContracts;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace SearchServiceTest
{
    [TestClass]
    public class SearchTest
    {
        // In order to run the below test(s), 
        // please follow the instructions from https://docs.microsoft.com/en-us/microsoft-edge/webdriver-chromium
        // to install Microsoft Edge WebDriver.
        string SearchManagementService = "https://localhost:7238/";
        private EdgeDriver _driver;

        [TestInitialize]
        public void EdgeDriverInitialize()
        {
        }

      
        [TestMethod]
        public async Task SearchPostOnFullText()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(SearchManagementService);
            string QueryToSearch = "Test";
            HttpResponseMessage response = await client.GetAsync(string.Format("api/Search/{0}", QueryToSearch));
            if (response.IsSuccessStatusCode)
            {
                Assert.IsTrue(response.IsSuccessStatusCode);
                var responseContent = await response.Content.ReadAsStringAsync();
                var Posts = JsonConvert.DeserializeObject<List<PostModel>>(responseContent);
                Assert.IsTrue(Posts.Count > 0, "No Posts Found for Query " + QueryToSearch);
            }
            else
            {
                Assert.Fail("Search Failed: " + response.StatusCode + " for Search Query: " + QueryToSearch);
            }
        }

        [TestMethod]
        public async Task SearchPostOnFullText1()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(SearchManagementService);
            string QueryToSearch = "Test12312";
            HttpResponseMessage response = await client.GetAsync(string.Format("api/Search/{0}", QueryToSearch));
            if (response.IsSuccessStatusCode)
            {
                Assert.IsTrue(response.IsSuccessStatusCode);
                var responseContent = await response.Content.ReadAsStringAsync();
                var Posts = JsonConvert.DeserializeObject<List<PostModel>>(responseContent);
                Assert.IsTrue(Posts.Count > 0, "No Posts Found for Query " + QueryToSearch);
            }
            else
            {
                Assert.Fail("Search Failed: " + response.StatusCode + " for Search Query: " + QueryToSearch);
            }
        }

        [TestCleanup]
        public void EdgeDriverCleanup()
        {
          
        }
    }
}
