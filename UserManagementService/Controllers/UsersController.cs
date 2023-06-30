using Contracts.DataContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.Resource;
using Microsoft.Identity.Client;

namespace UserManagementService.Controllers
{
   
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
       
        private readonly IConfiguration configuration;
        private readonly AuthConfig _authConfig;
        public UsersController(IConfiguration configuration)
        {

            this.configuration = configuration;
            _authConfig = AuthConfig.ReadJsonFromFile("securesettings.json");

        }
        [HttpPost]
        public async Task <ActionResult<UserModel>> AuthenticateUser(UserModel user)
        {
            IConfidentialClientApplication app;

            app = ConfidentialClientApplicationBuilder.Create(_authConfig.ClientID).WithClientSecret(_authConfig.ClientSecret)
           .WithAuthority(new Uri(_authConfig.Authority)).Build();

            string[] ResourceIds = new string[] { _authConfig.ResourceId };
            AuthenticationResult result =null;

            UserModel userModel = null;
            if (user.UserName == "admin" && user.Password == "Password123")
            {
                userModel = new UserModel { UserName = "Authenticated User" };
                try
                {

                    result = await app.AcquireTokenForClient(ResourceIds).ExecuteAsync();
                    userModel.Token = result.AccessToken;
                    return Ok(userModel);




                    //using (HttpClient client = new HttpClient())
                    //{
                    //    var defaultRequestHeader = client.DefaultRequestHeaders;
                    //    defaultRequestHeader.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", result.AccessToken);

                    //    var FormContent = new MultipartFormDataContent();
                    //    FormContent.Add(new StringContent("Description"), "description");
                    //    FormContent.Add(new StreamContent(System.IO.File.OpenRead("C:\\test\\asd.png")), "image", "image");
                    //    client.BaseAddress = new Uri(_authConfig.BaseAddress);
                    //    // HttpResponseMessage response = await client.PostAsJsonAsync("api/Posts", FormContent);
                    //    HttpResponseMessage response = await client.PostAsync("api/Posts", FormContent);
                    //}

                }
                catch (Exception ex)
                { 
                
                }
            }
            return Unauthorized();
        }
           
        }
    
}
