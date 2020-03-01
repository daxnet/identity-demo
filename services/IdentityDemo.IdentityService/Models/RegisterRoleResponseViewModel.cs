using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityDemo.IdentityService.Models
{
    public class RegisterRoleResponseViewModel
    {
        public RegisterRoleResponseViewModel(AppRole role)
        {
            Id = role.Id;
            Name = role.Name;
            Description = role.Description;
        }

        public string Id { get; }

        public string Name { get; }

        public string Description { get; }
    }
}
