using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using szakdoga.Models;
using szakdoga.Models.Dtos;
using szakdoga.Models.RepositoryInterfaces;
using szakdoga.Others;

namespace szakdoga.Controllers
{
    [AllowAnonymous]
    [Route("api/auth")]
    public class AuthController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserJwtMapRepository _userJwtMapRepository;
        private readonly int expiryMinutes = 10;
        public AuthController(IUserRepository userRepository, IUserJwtMapRepository userJwtMapRepository)
        {
            _userRepository = userRepository;
            _userJwtMapRepository = userJwtMapRepository;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody]CredentialDto credDto)
        {
            if (credDto == null)
            {
                return BadRequest("Wrong data syntax.");
            }

            User user = _userRepository.GetAll().FirstOrDefault(x => x.Name.Equals(credDto.Name));
            if (user == null)
            {
                return Unauthorized();
            }

            if (user.Password != credDto.Password)
            {
                return Unauthorized();
            }

            var token = new JwtTokenBuilder()
                                .AddSecurityKey(JwtSecurityKey.Create("zhenwang123!.123"))
                                .AddSubject("ToDoListServerUser")
                                .AddIssuer("ToDoListServer")
                                .AddAudience("ToDoListServer")
                                .AddClaim("User", "ToDoListServerUser")
                                .AddExpiry(expiryMinutes)
                                .Build();

            //clean expried json
            DateTime curruntTime = DateTime.Now;
            _userJwtMapRepository.RemoveRecordBefore(curruntTime);

            //store jwt and userid in userjwtmap
            _userJwtMapRepository.AddUserJwtMapRecord(token.Value, user, curruntTime.AddMinutes(expiryMinutes));

            return Ok(Json(new
            {
                message = "success",
                jwt = token.Value
            }));
        }
    }
}
