using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Contracts.DataContracts
{
    public class UserModel
    {
        [Required]
        public string UserName { set; get; }
        [Required]
        public string Password { set; get; }

        [NotMapped]
        public string? Token { get; set; }
    }
}
