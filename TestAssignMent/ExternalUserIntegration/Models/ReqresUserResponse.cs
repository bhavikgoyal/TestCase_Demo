using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternalUserIntegration.Models
{
public class ReqresUserResponse
{
    public UserData Data { get; set; }

    public class UserData
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
    }
}
}
