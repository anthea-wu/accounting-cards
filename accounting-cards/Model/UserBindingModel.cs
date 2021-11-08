using System;

namespace accounting_cards.Model
{
    public class UserLoginResponseBindingModel
    {
        public Guid Guid { get; set; }
        public string Name { get; set; }
        public string Account { get; set; }
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