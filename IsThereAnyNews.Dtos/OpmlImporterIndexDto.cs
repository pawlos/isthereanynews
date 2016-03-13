using System.Web;

namespace IsThereAnyNews.Dtos
{
    public class OpmlImporterIndexDto
    {
        public HttpPostedFileBase ImportFile { get; set; }
    }
}