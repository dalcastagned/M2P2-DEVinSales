using DevInSales.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevInSales.Core.Data.Dtos
{
    public class ReadUser
    {
        public ReadUser(User user)
        {
            Id = user.Id;
            Email = user.Email;
            Name = user.UserName;
            BirthDate = user.BirthDate;
        }
        
        public int Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
