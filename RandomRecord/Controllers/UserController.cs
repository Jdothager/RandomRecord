using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using RandomRecords.Models;

namespace RandomRecords.Controllers
{
    public class UserController : ApiController
    {
        public IEnumerable<User> Get()
        {
            UserList usersList = new UserList();

            return usersList.GetList();
        }

    }
}
