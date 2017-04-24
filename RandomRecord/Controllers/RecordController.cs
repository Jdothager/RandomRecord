using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using RandomRecords.Models;

namespace RandomRecords.Controllers
{
    public class RecordController : ApiController
    {
        public IEnumerable<Record> Get()
        {
            ResultCreator RecordsList = new ResultCreator();

            return RecordsList.GetRecords();
        }

    }
}
