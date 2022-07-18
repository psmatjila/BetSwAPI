using System.Threading.Tasks;
using BetSoftware.CustomExceptions;
using BetSoftware.Data;
using BetSoftware.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BetSoftware.Controller
{
    
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : ControllerBase
    {
       private readonly IUserService _userService;

    public AuthenticationController(IUserService userService)
       {
           _userService = userService;
       }
       
       [HttpPost("signup")] 
       public async Task<IActionResult> SignUp(User user)
       {
          try
          {
              var result = await _userService.SignUp(user);

              return Created("",result);
          }
          catch (UsernameAlreadyExistsException e)
          {
              
             return StatusCode(409, e.Message); // 409 > user conflict
          }
  
       }
        
        [Authorize]
        [HttpPost("signin")] 
        public async Task<IActionResult> SignIn(User user)
        {
           try
           {
               var result = await _userService.SignIn(user);
               return Ok(result);
           }
           catch (InvalidUsernamePasswordException e)
           {
               
               return StatusCode(401, e.Message);   //401 > Unauthorised
           } 
        }
    }
}