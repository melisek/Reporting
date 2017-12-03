using System;
using System.IdentityModel.Tokens.Jwt;

namespace szakdoga.Others
{
    public class JWT
    {
        private JwtSecurityToken token;

        public JWT(JwtSecurityToken token)
        {
            this.token = token;
        }

        public DateTime ValidTo => token.ValidTo;
        public string Value => new JwtSecurityTokenHandler().WriteToken(this.token);
    }
}