using System.Web;

namespace IsThereAnyNews.Mvc.Controllers
{
    public class OpmlImporterIndexDto
    {
        public HttpPostedFileBase ImportFile { get; set; }
    }
}