using JwtAuthenticationManager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Data;
using Newtonsoft.Json.Linq;
using System.Globalization;

namespace JwtAuthenticationManager
{
    
    public class JwtTokenHandler
    {
        public const string JWT_SECRET_KEY = "KDSJFVHAFGASFVASJFVSADFHBAKBJSDJBFXD";
        private const int JWT_TOKEN_VALIDITY_MINS = 5;

        private readonly List<UserAccounts> useraccountlist;
        
        public JwtTokenHandler()
        {
            //useraccountlist = new List<UserAccounts>
            //{
            //    new UserAccounts{ username="admin", password="123", role="administrator" },
            //    new UserAccounts{ username="user", password="123", role="user" }
            //};
        }
        public AuthenticationResponse? GenerateJWTToken(AuthenticationRequest authenticationRequest)
        {
            //Validating username and password
            //if (string.IsNullOrEmpty(authenticationRequest.username) || string.IsNullOrEmpty(authenticationRequest.password))
            //    return null;

            //var useraccount = useraccountlist.Where(x => x.username == authenticationRequest.username && x.password == authenticationRequest.password).FirstOrDefault();
            //if (useraccount == null) return null;


            var tokenExpiryTimestamp = DateTime.Now.AddMinutes(JWT_TOKEN_VALIDITY_MINS);
           
            var tokenKey = Encoding.ASCII.GetBytes(JWT_SECRET_KEY);
            //var claimsIdentity = new ClaimsIdentity(new List<Claim>
            //{
            //    new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Name, authenticationRequest.uautoid.ToString()),
            //    new Claim(ClaimTypes.Role, authenticationRequest.role),
            //    new Claim(ClaimTypes.UserData, authenticationRequest.cid),
            //    new Claim(ClaimTypes.Anonymous, authenticationRequest.uid)
            //});

            var claims = new List<Claim>
            {
                    new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Name, authenticationRequest.uautoid.ToString()),
                    //new Claim(ClaimTypes.Role, authenticationRequest.role),
                    new Claim(ClaimTypes.UserData, authenticationRequest.cid),
                    new Claim(ClaimTypes.Anonymous, authenticationRequest.uid),
                    new Claim(ClaimTypes.Locality, authenticationRequest.role)
            };
            if (authenticationRequest.role == "user")
            {
                claims.AddRange(authenticationRequest.list_permissions.Select(role => new Claim(ClaimTypes.Role, role)));
            }
            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature);

            var securityTokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = tokenExpiryTimestamp,
                SigningCredentials = signingCredentials
            };

            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var securityToken = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);
            var token = jwtSecurityTokenHandler.WriteToken(securityToken);

            return new AuthenticationResponse
            {
                uid = authenticationRequest.uautoid,
                ExpiresIn = (int)tokenExpiryTimestamp.Subtract(DateTime.UtcNow).TotalSeconds,
                JWTToken = token
            };

            //return new
            //{
            //    list_mylist = authenticationRequest.list_permissions
            //};

        }

        public ClaimResponse GetCustomClaims(ClaimRequest claimRequest)
        {
            bool isauth = false;
            int count= 0;
            string secret = "KDSJFVHAFGASFVAS" + "JFVSADFHBAKBJSDJBFXD";
            //KDSJFVHAFGASFVASJFVSADFHBAKBJSDJBFXD
            var key = Encoding.ASCII.GetBytes(secret);
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(claimRequest.token);
            Console.WriteLine("Token"+jwtToken);
            string claim1 = jwtToken.Claims.First(c => c.Type == "name").Value;
            //jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);
            List<string> claim2list = new List<string>();
            //var claim2 = jwtToken.Claims.FirstOrDefault(c => c.Type == "role").Value;
            var claim2 = jwtToken.Claims
                    .Where(c => c.Type == "role")
                    .Select(c => c.Value)
                    .ToList();
            
            string claim3 = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.UserData).Value;
            string claim4 = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Anonymous).Value;
            string claim5 = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Locality).Value;

            if (claim5 == "admin")
            {
                count += 1;
            }
            else if (claim5 == "user")
            {

                foreach (var item in claim2)
                {
                    if (item == claimRequest.controller_action_name)
                    {
                        count += 1;

                    }
                }
            }
            
            if(count>0)
            {
                isauth = true;
            }
            else
            {
                isauth = false;
            }
            return new ClaimResponse
            {
                uautoid = int.Parse(claim1),
                role = claim2,
                companyid = claim3,
                uid = claim4,
                approle = claim5,
                isauth=isauth
            };

        }

        public TokenResponse MakeTokenInvalid(TokenRequest tokenRequest)
        {
            string keyString = "KDSJFVHAFGASFVAS" + "JFVSADFHBAKBJSDJBFXD";
            byte[] keyBytes = Encoding.UTF8.GetBytes(keyString);
            SymmetricSecurityKey signingKey = new SymmetricSecurityKey(keyBytes);

            var tokenHandler = new JwtSecurityTokenHandler();
            var existingToken = tokenHandler.ReadJwtToken(tokenRequest.token);
            var now = DateTime.UtcNow;
            var notBefore = now;
            var expires = now.AddMinutes(10);

            // Set the expiration time to a date in the past
            //var newExp = DateTime.UtcNow.AddMinutes(-1); // set the new expiration time

            // Create a new token with the updated expiration time
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(existingToken.Subject),
                NotBefore = notBefore,
                Expires = expires,
                SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
            };

            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var securityToken = jwtSecurityTokenHandler.CreateToken(tokenDescriptor);
            var token = jwtSecurityTokenHandler.WriteToken(securityToken);
            Console.WriteLine(token);
            // Return the new token to the client
            return new TokenResponse
            {
                token = token.ToString()
            };
        }
    }
}

