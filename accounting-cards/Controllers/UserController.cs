using System;
using System.Linq;
using System.Security.Cryptography;
using accounting_cards.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace accounting_cards.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        /// <summary> 帳號驗證 </summary>
        [Route("Post/Session")]
        [HttpPost]
        public IActionResult Check(UserCheckRequestBindingModel check)
        {
            var result = new UserCheckResponseBindingModel()
            {
                Account = check.Account,
                Step = 0
            };
            
            using (var db = new AccountingContext())
            {
                var user = db.Users.FirstOrDefault(u => u.account == check.Account);
                if (user == null)
                {
                    var bite = new byte[16];
                    using (var rngCsp = new RNGCryptoServiceProvider())
                    {
                        rngCsp.GetBytes(bite);
                    }
                    var salt = Convert.ToBase64String(bite);
                    
                    result.Salt = salt;
                    user = new User()
                    {
                        guid = Guid.NewGuid(),
                        account = check.Account,
                        temp_key = salt
                    };
                    db.Users.Add(user);
                    db.SaveChanges();
                }
                else
                {
                    result.Step = 1;
                }
                
                db.Dispose();
            }
            
            return Ok(result);
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
                if (user == null)
                {
                    db.Dispose();
                    return BadRequest($"帳號 {register.Account} 的註冊流程不合法");
                }

                user.account = register.Account;
                user.password = register.Password;
                user.name = register.Name;
                user.create_time = DateTimeOffset.Now;
                
                db.SaveChanges();
                db.Dispose();
            }
            
            return CreatedAtAction(nameof(Register), register);
        }

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
    }

    public class UserCheckResponseBindingModel
    {
        public string Account { get; set; }
        public int Step { get; set; }
        public string Salt { get; set; }
    }

    public class UserCheckRequestBindingModel
    {
        public string Account { get; set; }
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