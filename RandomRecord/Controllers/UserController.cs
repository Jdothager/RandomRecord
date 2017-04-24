using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using RandomRecord.Models;

namespace RandomRecord.Controllers
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
