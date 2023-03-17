﻿using JwtAuthenticationManager;
using JwtAuthenticationManager.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AccountsWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly JwtTokenHandler _jwtTokenHandler;
        public AccountsController(JwtTokenHandler jwtTokenHandler)
        {
            _jwtTokenHandler = jwtTokenHandler;
        }

        //ActionResult<AuthenticationResponse?>
        [HttpPost]
        public ActionResult<mylist?> Authenticate([FromBody] AuthenticationRequest authenticationRequest)
        {
            //var user = await _context.users.FirstOrDefaultAsync(u => u.username == username && u.password == password);

            //if (user == null)
            //{
            //    return Unauthorized();
            //}

            //return Ok(user);
            var myList = _jwtTokenHandler.GenerateJWTToken(authenticationRequest);
            if (myList is null)
                return Unauthorized();
            return Ok(myList);
        }
    }
}
