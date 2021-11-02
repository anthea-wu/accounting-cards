using System;
using System.Linq;
using accounting_cards.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace accounting_cards.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        /// <summary> 登入 </summary>
        [Route("Post")]
        [HttpPost]
        public IActionResult Login(UserLoginRequestBindingModel login)
        {
            if (login.Account == "anthea" && login.Password == "123")
            {
                return Ok("歡迎登入");
            }
            return BadRequest("登入失敗");
        }

        /// <summary> 註冊 </summary>
        [Route("Post/New")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public IActionResult Register(UserRegisterRequestBindingModel register)
        {
            using (var db = new AccountingContext())
            {
                var user = db.Users.FirstOrDefault(u => u.account == register.Account);
                if (user != null)
                {
                    db.Dispose();
                    return Conflict($"帳號 {register.Account} 已存在");
                }

                user = new User()
                {
                    guid = Guid.NewGuid(),
                    account = register.Account,
                    password = register.Password,
                    name = register.Name,
                    create_time = DateTimeOffset.Now
                };
                db.Users.Add(user);
                db.SaveChanges();
                db.Dispose();
            }
            
            return CreatedAtAction(nameof(Register), register);
        }
    }

    public class UserRegisterRequestBindingModel
    {
        public string Account { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
    }

    public class UserLoginRequestBindingModel
    {
        public string Account { get; set; }
        public string Password { get; set; }
    }
}