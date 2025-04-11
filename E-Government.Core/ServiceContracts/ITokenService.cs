using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Government.Core.ServiceContracts
{
    public interface ITokenService
    {
        Task<string> GenerateToken(IdentityUser user, UserManager<IdentityUser> userManager);
    }
}
