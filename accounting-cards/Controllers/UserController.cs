using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using accounting_cards.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace accounting_cards.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly AccountingContext _db;
        private readonly IDbService _userService;
        private readonly IResultService _result;

        public UserController(AccountingContext db, IDbService userService, IResultService result)
        {
            _db = db;
            _userService = userService;
            _result = result;
        }

        /// <summary> 帳號驗證 </summary>
        [Route("Post/Session")]
        [HttpPost]
        public IActionResult Check(UserCheckRequestBindingModel account)
        {
            if (string.IsNullOrEmpty(account.Account))
            {
                return BadRequest("請填入帳號。");
            }
            
            var salt = GetSalt();
            var user = _userService.GetExistOrCreateNew(account, salt);
            var result = _result.Get(account, salt, user);

            return Ok(result);
        }


        /// <summary> 註冊 </summary>
        [Route("Post/New")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public IActionResult Register(UserRegisterRequestBindingModel register)
        {
            var user = _db.Users.FirstOrDefault(u => u.account == register.Account);
            if (user == null)
            {
                _db.Dispose();
                return BadRequest($"帳號 {register.Account} 的註冊流程不合法");
            }

            user.account = register.Account;
            user.password = register.Password;
            user.name = register.Name;
            user.create_time = DateTimeOffset.Now;

            _db.SaveChanges();
            _db.Dispose();

            return CreatedAtAction(nameof(Register), register);
        }

        /// <summary> 登入 </summary>
        [Route("Post")]
        [HttpPost]
        public IActionResult Login(UserLoginRequestBindingModel login)
        {
            var user = _db.Users.FirstOrDefault(u => u.account == login.Account);
            if (user == null)
            {
                _db.Dispose();
                return BadRequest("登入失敗！帳號或密碼錯誤。");
            }

            var hashStr = $"salt={user.temp_key}+password={login.Password}";

            var sha256 = SHA256.Create();
            var originalBytes = UTF8Encoding.Default.GetBytes(hashStr);
            var encodedBytes = sha256.ComputeHash(originalBytes);
            var passwordHash = BitConverter.ToString(encodedBytes).Replace("-", "");

            if (passwordHash != user.password)
            {
                _db.Dispose();
                return BadRequest("登入失敗！帳號或密碼錯誤。");
            }

            var result = new UserLoginResponseBindingModel()
            {
                Guid = user.guid,
                Name = user.name,
                Account = user.account
            };
            return Ok(result);
        }

        private static string GetSalt()
        {
            var bite = new byte[16];
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                rngCsp.GetBytes(bite);
            }

            var salt = Convert.ToBase64String(bite);
            return salt;
        }
    }
}