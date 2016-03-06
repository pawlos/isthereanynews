using System.Web;

namespace IsThereAnyNews.Mvc.Dtos
{
    public class OpmlImporterIndexDto
    {
        public HttpPostedFileBase ImportFile { get; set; }
    }
}