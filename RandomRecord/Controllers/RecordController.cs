using RandomRecords.Models;
using System.Collections.Generic;
using System.Web.Http;

namespace RandomRecords.Controllers
{
    public class RecordController : ApiController
    {
        private static RecordRepository CsvData = new RecordRepository();
        private static ResultCreator Creator = new ResultCreator(CsvData);

        public IEnumerable<Record> Get(int qty = 1)
        {
            // safety check to ensure the requested amount is positive and less than max limit
            int max_Limit = 3000;
            if (qty > max_Limit || qty < 1)
            {
                qty = 1;
            }

            return Creator.GetRecords(qty);
        }
    }
}