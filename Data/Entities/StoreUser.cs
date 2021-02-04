using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tigers.Data.Entities
{
    public class StoreUser : IdentityUser
    {
        public string FristName { get; set; }
        public string LastName { get; set; }

    }
}
