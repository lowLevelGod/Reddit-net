﻿namespace RedditNet.Models.UserModel
{
    public class UserUpdateModel : UserModel
    {
        public String Password { get; set; }
        public int? Role { get; set; }
    }
}
