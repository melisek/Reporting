using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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
        private readonly IConfigurationRoot _cfg;
        private readonly int expiryMinutes = 10;

        public AuthController(IUserRepository userRepository, IUserJwtMapRepository userJwtMapRepository, IConfigurationRoot cfg)
        {
            _userRepository = userRepository;
            _userJwtMapRepository = userJwtMapRepository;
            _cfg = cfg;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody]CredentialDto credDto)
        {
            if (credDto == null)
                return BadRequest("Wrong data syntax.");

            if (!ModelState.IsValid)
                BadRequest(ModelState);

            User user = _userRepository.GetAll().FirstOrDefault(x => x.EmailAddress.Equals(credDto.EmailAddress));
            if (user == null)
            {
                return Unauthorized();
            }

            if (user.Password != credDto.Password)
            {
                return Unauthorized();
            }

            var token = new JwtTokenBuilder()
                                .AddSecurityKey(JwtSecurityKey.Create(_cfg["Tokens:Key"]))
                                .AddSubject(_cfg["Tokens:Subject"])
                                .AddIssuer(_cfg["Tokens:Issuer"])
                                .AddAudience(_cfg["Tokens:Audience"])
                                .AddClaim("reportGUID","canmodify")
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