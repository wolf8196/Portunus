using System.Collections.Generic;
using System.Linq;

namespace DemoApps.Shared.Identity
{
    public class UserStore
    {
        private readonly List<User> users;

        public UserStore()
        {
            users = new List<User>
            {
                new User
                {
                    Id = 1,
                    UserName = "user1",
                    Password = "password1"
                },
                new User
                {
                    Id = 2,
                    UserName = "user2",
                    Password = "password2"
                }
            };
        }

        public User FindByUserName(string username)
        {
            return users.FirstOrDefault(x => x.UserName == username);
        }
    }
}