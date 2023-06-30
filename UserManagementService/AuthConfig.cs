using System;
using System.IO;
using System.Globalization;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace UserManagementService
{
    public class AuthConfig
    {
        public string ClientID { get; set; }
        public string InstanceId { get; set; }
        public string TenantId { get; set; }
        public string ClientSecret { get; set; }
        public string BaseAddress { get; set; }
        public string ResourceId { get; set; }
        public string Authority
        {
            get {
                return string.Format(CultureInfo.InvariantCulture, InstanceId, TenantId);
            }
        }
        public static AuthConfig ReadJsonFromFile(string Path)
        {
            IConfiguration configuration; ;
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile(Path);
            configuration = builder.Build();
            return configuration.Get<AuthConfig>();
        }

    }
}
