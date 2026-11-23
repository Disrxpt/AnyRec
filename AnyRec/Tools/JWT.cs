using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace AnyRec.Tools
{
    internal class JWT
    {
        public static string Create(Dictionary<string, string> data)
        {
            var claims = new List<Claim>();
            foreach (var item in data)
            {
                claims.Add(new Claim(item.Key, item.Value));
            }
            var token = new JwtSecurityToken(claims: claims);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public static Dictionary<string, string> Decode(string token)
        {
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            var data = new Dictionary<string, string>();
            foreach (var claim in jwt.Claims)
            {
                data[claim.Type] = claim.Value;
            }
            return data;
        }
    }
}
