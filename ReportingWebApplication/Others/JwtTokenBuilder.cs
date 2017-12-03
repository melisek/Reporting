using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace szakdoga.Others
{
    public class JWTCreator
    {
        private SecurityKey securityKey = null;

        private string subject = "";
        private string issuer = "";
        private string audience = "";
        private Dictionary<string, string> claims = new Dictionary<string, string>();
        private int expiryInMinutes = 15;

        public string Subject { get => subject; set => subject = value; }
        public string Issuer { get => issuer; set => issuer = value; }
        public string Audience { get => audience; set => audience = value; }
        public int ExpiryInMinutes { get => expiryInMinutes; set => expiryInMinutes = value; }
        public SecurityKey SecurityKey { get => securityKey; set => securityKey = value; }


        public JWTCreator AddClaim(string type, string value)
        {
            this.claims.Add(type, value);
            return this;
        }

        public JWTCreator AddClaims(Dictionary<string, string> claims)
        {
            this.claims.Union(claims);
            return this;
        }

        public JWT Build()
        {
            CheckTokenElements();
            var claims = new List<Claim>
            {
              new Claim(JwtRegisteredClaimNames.Sub, this.Subject),
              new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            }
            .Union(this.claims.Select(item => new Claim(item.Key, item.Value)));
            var token = new JwtSecurityToken(Issuer, Audience, claims, null, DateTime.UtcNow.AddMinutes(ExpiryInMinutes), new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256));
            return new JWT(token);
        }

        private void CheckTokenElements()
        {
            if (this.SecurityKey == null) throw new BasicException("Empty Security Key");
            if (string.IsNullOrEmpty(this.Subject)) throw new BasicException("Empty Subject");
            if (string.IsNullOrEmpty(this.Issuer)) throw new BasicException("Empty Issuer");
            if (string.IsNullOrEmpty(this.Audience)) throw new BasicException("Empty Audience");
        }
    }
}