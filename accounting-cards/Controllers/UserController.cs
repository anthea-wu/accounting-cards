using Microsoft.AspNetCore.Mvc;

namespace accounting_cards.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        /// <summary> 使用者登入 </summary>
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

    public class UserLoginRequestBindingModel
    {
        public string Account { get; set; }
        public string Password { get; set; }
    }
}