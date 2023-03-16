using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSLib.Tables
{
    public class User
    {
        public User(int id, string login, string roleName)
        {
            Id = id;
            Login = login;
            RoleName = roleName;
        }

        public int Id { get; set; }
        public string Login { get; set; }
        public string RoleName { get; set; }
        public string FullName { get; set; }
        public override string ToString()
        {
            return Id + " " + Login + " " + RoleName;
        }
    }
}
