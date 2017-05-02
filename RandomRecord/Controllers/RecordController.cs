using System.Collections.Generic;
using System.Web.Http;
using RandomRecords.Models;



namespace RandomRecords.Controllers
{
    public class RecordController : ApiController
    {
        private static RecordRepository CsvData = new RecordRepository();
        private static ResultCreator Creator = new ResultCreator(CsvData);

        public IEnumerable<Record> Get(int qty = 1)
        {
            return Creator.GetRecords(qty);
        }

    }
}
