using System;
using System.Text;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace IsThereAnyNews.Web.Controllers
{
    using Newtonsoft.Json.Serialization;

    public class JsonNetResult: ActionResult
    {
        public JsonNetResult(object data)
        {
            this.Data = data;
            this.Settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                TypeNameHandling = TypeNameHandling.Objects,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
        }

        public JsonSerializerSettings Settings { get; set; }

        public Encoding ContentEncoding { get; set; }

        public string ContentType { get; set; }

        public object Data { get; set; }

        public Formatting Formatting { get; set; }

        public override void ExecuteResult(ControllerContext context)
        {
            if(context == null)
            {
                throw new ArgumentNullException("context");
            }

            var response = context.HttpContext.Response;

            response.ContentType = !string.IsNullOrEmpty(this.ContentType)
                                       ? this.ContentType
                                       : "application/json";

            if(this.ContentEncoding != null)
            {
                response.ContentEncoding = this.ContentEncoding;
            }

            if(this.Data != null)
            {
                var writer = new JsonTextWriter(response.Output) { Formatting = this.Formatting };
                var serializer = JsonSerializer.Create(this.Settings);
                serializer.Serialize(writer, this.Data);
                writer.Flush();
            }
        }
    }
}