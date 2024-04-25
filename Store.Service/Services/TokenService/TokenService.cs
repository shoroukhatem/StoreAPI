﻿using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Store.Data.Entities.IdentityEntities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service.Services.TokenService
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration configuration;
        private readonly SymmetricSecurityKey _key ;

        public TokenService(IConfiguration configuration) {
            this.configuration = configuration;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Token:Key"]));
        }
        public string GenerateToken(AppUser appUser)
        {
            var claims = new List<Claim> {
            new Claim(ClaimTypes.Email, appUser.Email),
            new Claim(ClaimTypes.GivenName, appUser.DisplayName)
            };
            var cred = new SigningCredentials(_key,SecurityAlgorithms.HmacSha256);
            var tokenDescriptor = new SecurityTokenDescriptor { 
            Subject = new ClaimsIdentity(claims),
            Issuer = configuration["Token:Issuer"],
            IssuedAt = DateTime.Now,
            Expires = DateTime.Now.AddDays(1),
            SigningCredentials = cred,
            Audience = configuration["Token:Issuer"]
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
