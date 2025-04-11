using E_Government.Core.Domain.Entities;
using E_Government.Core.Domain.RepositoryContracts.Persistence;
using E_Government.Core.DTO;
using E_Government.Core.ServiceContracts;
using E_Government.UI.Controllers.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace E_Government.UI.Controllers.Account
{
 
    public class AccountController : ApiControllerBase
    {
        private readonly UserManager<IdentityUser> _user;
        private readonly ITokenService _token;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IUnitOfWork _unitOfWork;

        public AccountController(UserManager<IdentityUser> user, ITokenService token, SignInManager<IdentityUser> signInManager , IUnitOfWork unitOfWork)
        {
            this._user = user;
            this._token = token;
            this._signInManager = signInManager;
            this._unitOfWork = unitOfWork;
        }


        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> Register([FromForm] RegisterDTO registerDTO)
        {
           var user = _unitOfWork.GetRepository<Citizens>().GetNID(registerDTO.NID);

            if (user == null)
            {
                return NotFound();
            }
            var Appuser = new IdentityUser
            {
                Email = registerDTO.Email,
                UserName = registerDTO.Email.Split("@")[0],
               
            };

            var result = await _user.CreateAsync(Appuser, registerDTO.Password);

            if (!result.Succeeded)
            {
                return BadRequest();
            }

            var Returned = new UserDTO
            {
                Email = Appuser.Email,
               
                Token = await _token.GenerateToken(Appuser, _user)
            };
            return Ok(Returned);

        }
        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login([FromForm] loginDTO loginDTO)
        {
            var user = await _user.FindByEmailAsync(loginDTO.Email);
            if (user == null) return Unauthorized();
            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDTO.Password, false);
            if (!result.Succeeded)
            {
                return Unauthorized();
            }

            var returned = new UserDTO
            {
                Email = user.Email,
                Token = await _token.GenerateToken(user, _user)
            };

            return Ok(returned);

        }
    }
}
