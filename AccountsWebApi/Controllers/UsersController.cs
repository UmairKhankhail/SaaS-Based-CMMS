using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AccountsWebApi.Models;
using JwtAuthenticationManager;
using JwtAuthenticationManager.Models;
using System.Xml.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using NuGet.Common;
using Newtonsoft.Json.Linq;
using Microsoft.IdentityModel.Tokens;
using System.Text;
//using Org.BouncyCastle.Asn1.Ocsp;
//using Ubiety.Dns.Core;
using NuGet.Protocol;
using System.Net.Http;
using Microsoft.DotNet.Scaffolding.Shared.CodeModifier.CodeChange;
using System.Data;
using System.Reflection.Metadata;
using Newtonsoft.Json;

namespace AccountsWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserDbContext _context;
        private readonly JwtTokenHandler _JwtTokenHandler;
        private readonly HttpClient _httpClient;
        private readonly ILogger<UsersController> _logger;
        public UsersController(UserDbContext context, JwtTokenHandler jwtTokenHandler, HttpClient httpClient, ILogger<UsersController> logger)
        {
            _context = context;
            _JwtTokenHandler = jwtTokenHandler;
            _httpClient = httpClient;
            _logger = logger;
        }


        //string accessToken = Request.Headers[@"Authorization"].ToString().Replace(@"Bearer ", "");
        //Console.WriteLine(accessToken);
        //ClaimRequest cr = new ClaimRequest();
        //cr.token = accessToken;
        //var claimresponse =_JwtTokenHandler.GetCustomClaims(cr);
        //if (claimresponse is null)
        //    return Unauthorized();
        //return Ok(claimresponse);
        //return claimresponse is null ? (IActionResult)Unauthorized() : Ok(claimresponse);
        // GET: api/Users
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUser()
        {
            var actionname = base.ControllerContext.ActionDescriptor.ActionName;
            var controller = RouteData.Values["controller"].ToString();
            var result = $"{controller}Controller.{actionname}";

            if (!Request.Headers.TryGetValue("Authorization", out var authHeaderValues))
            {
                return Unauthorized();
            }

            var accessToken = authHeaderValues.FirstOrDefault()?.Replace("Bearer ", "");
            if (string.IsNullOrEmpty(accessToken))
            {
                return Unauthorized();
            }

            var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken });
            return Ok(claimresponse);
            //if (claimresponse is null || claimresponse.role != "user")
            //{
            //    return Unauthorized();
            //}

            //var url = $"http://localhost:5088/api/Permissions/GetPermission?uid={claimresponse.uid}&uautoid={claimresponse.uautoid}&cid={claimresponse.companyid}&caresult={result}";
            //var response = await _httpClient.GetAsync(url);
            //response.EnsureSuccessStatusCode();
            //var responseContent = await response.Content.ReadAsStringAsync();
            //Console.WriteLine(responseContent);

            //return Ok();
        }
        




        //public async Task<IActionResult> GetUser()
        //{
        //    var actionname = base.ControllerContext.ActionDescriptor.ActionName;
        //    var controller = RouteData.Values["controller"].ToString();
        //    var result = controller + "Controller" + "." + actionname;
        //    string accessToken = Request.Headers[@"Authorization"].ToString().Replace(@"Bearer ", "");
        //    var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken });
        //    if (claimresponse is null){return Unauthorized();}else{
        //        if (claimresponse.role == "user")
        //        {
        //            string url = $"http://localhost:5088/api/Permissions/GetPermission?uid={claimresponse.uid}&uautoid={claimresponse.uautoid}&cid={claimresponse.companyid}&caresult={result}";
        //            HttpResponseMessage response = await _httpClient.GetAsync(url);
        //            response.EnsureSuccessStatusCode();
        //            var res = response.Content.ReadAsStringAsync().Result;
        //            Console.WriteLine(res);
        //        }
        //        else if(claimresponse.role=="admin")
        //        {
        //            Console.WriteLine("Admin");
        //        }
        //    }
        //    return Ok();


        //}
        //public static void getclaims(string accessToken)
        //{

        //    string secret = "KDSJFVHAFGASFVAS" + "JFVSADFHBAKBJSDJBFXD";
        //    var key = Encoding.ASCII.GetBytes(secret);
        //    var handler = new JwtSecurityTokenHandler();
        //    var jwtToken = handler.ReadJwtToken(accessToken);
        //    Console.WriteLine(jwtToken);
        //    string claim1 = jwtToken.Claims.First(c => c.Type == "name").Value;
        //    string claim2 = jwtToken.Claims.First(c => c.Type == "role").Value;
        //    string claim3 = jwtToken.Claims.FirstOrDefault(c=>c.Type==ClaimTypes.UserData).Value;

        //    Console.WriteLine(claim1);
        //    Console.WriteLine(claim2);
        //    Console.WriteLine(claim3);


        //}
        // GET: api/Users/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<User>> GetUser(int id)
        //{
        //    var user = await _context.users.FindAsync(id);

        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    return user;
        //}

        //[HttpGet("{username}")]
        //public async Task<IActionResult> GetUser(string username, string password)
        //{
        //    var user = await _context.users.FirstOrDefaultAsync(u => u.username == username && u.password==password);

        //    if (user == null)
        //    {
        //        return Unauthorized();
        //    }

        //    return Ok(user);
        //}

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(string id, User user)
        {
            if (id != user.userId)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<User>> PostUser(User user)
        //{
        //    _context.users.Add(user);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetUser", new { id = user.userid }, user);
        //}


        //POST: api/Users
        //To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<User>> PostUser(User user)
        //{
        //    _context.users.Add(user);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetUser", new { id = user.userid }, user);
        //}

        //[HttpPost]
        //public ActionResult<AuthenticationResponse?> Authenticate([FromBody] AuthenticationRequest authenticationRequest)
        //{
        //if()
        //var authenticationresponse = _JwtTokenHandler.GenerateJWTToken(authenticationRequest);
        //if (authenticationresponse is null)
        //    return Unauthorized();
        //return authenticationresponse;
        //}

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        //[HttpPost]
        //public ActionResult Authenticate(string username, string password)
        //{
        //    var user = _context.users.FirstOrDefaultAsync(u => u.username == username && password == u.password).Result;

        //    if (user == null)
        //    {
        //        return Unauthorized();
        //    }

        //    return Ok(user);
        //}

        [HttpPost]
        public async Task<ActionResult> Authenticate(string userName, string password)
        {
            try
            {
                var user = _context.companies.FirstOrDefaultAsync(u => u.companyEmail == userName && password == u.password).Result;

                if (user != null)
                {
                    AuthenticationRequest authenticationRequest = new AuthenticationRequest();
                    authenticationRequest.uAutoId = 0;
                    authenticationRequest.uId = "uadmin";
                    authenticationRequest.password = user.password;
                    authenticationRequest.role = "admin";
                    authenticationRequest.cid = user.companyId;

                    var authenticationresponse = _JwtTokenHandler.GenerateJWTToken(authenticationRequest);
                    if (authenticationresponse is null)
                        return Unauthorized();
                    return Ok(authenticationresponse);
                }
                else if (user == null)
                {
                    var appUser = _context.users.FirstOrDefaultAsync(u => u.userName == userName && password == u.password).Result;
                    if (appUser != null)
                    {
                        AuthenticationRequest authenticationRequest = new AuthenticationRequest();
                        authenticationRequest.uAutoId = appUser.userAutoId;
                        authenticationRequest.uId = appUser.userId;
                        authenticationRequest.password = appUser.password;
                        authenticationRequest.role = "user";
                        authenticationRequest.cid = appUser.companyId;

                        var url = $"http://localhost:5088/api/Permissions/GetPermission?uid={appUser.userId}&uautoid={appUser.userAutoId}&cid={appUser.companyId}";
                        var response = await _httpClient.GetAsync(url);
                        var responseContent = await response.Content.ReadAsStringAsync();
                        List<string> responseList = new List<string>();
                        var myNewList = JsonConvert.DeserializeObject<List<string>>(responseContent);
                        foreach (var item in myNewList)
                        {
                            responseList.Add(item);
                        }
                        authenticationRequest.listPermissions = responseList;
                        var authenticationResponse = _JwtTokenHandler.GenerateJWTToken(authenticationRequest);
                        if (authenticationResponse is null)
                            return Unauthorized();
                        return Ok(authenticationResponse);
                    }
                    else
                    {
                        return Unauthorized();
                    }
                }
                else
                {
                    return Unauthorized();
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            
        }


        private bool UserExists(string id)
        {
            return _context.users.Any(e => e.userId == id);
        }
    }
}
