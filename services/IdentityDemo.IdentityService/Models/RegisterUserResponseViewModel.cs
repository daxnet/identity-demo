using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityDemo.IdentityService.Models
{
    public class RegisterUserResponseViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }

        public RegisterUserResponseViewModel(AppUser user)
        {
            Id = user.Id;
            UserName = user.UserName;
            DisplayName = user.DisplayName;
            Email = user.Email;
        }
    }
}
