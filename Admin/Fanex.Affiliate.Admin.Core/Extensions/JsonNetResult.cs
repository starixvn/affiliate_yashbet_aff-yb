using Newtonsoft.Json;
using System;
using System.Text;
using System.Web.Mvc;

namespace Fanex.Affiliate.Admin.Core.Extensions
{
    public class JsonNetResult : ActionResult
    {
        public Encoding ContentEncoding { get; set; }
        public string ContentType { get; set; }
        public object Data { get; set; }
        public JsonSerializerSettings SerializerSettings { get; set; }
        public Formatting Formatting { get; set; }

        public JsonNetResult()
        {
            SerializerSettings = new JsonSerializerSettings();
            Formatting = Formatting.None;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var response = context.HttpContext.Response;

            response.ContentType = !string.IsNullOrEmpty(ContentType) ? ContentType : "application/json";

            if (ContentEncoding != null)
            {
                response.ContentEncoding = ContentEncoding;
            }

            if (Data != null)
            {
                using (var writer = new JsonTextWriter(response.Output))
                {
                    writer.Formatting = Formatting;
                    var serializer = JsonSerializer.Create(SerializerSettings);
                    serializer.Serialize(writer, Data);

                    writer.Flush();
                }
            }
        }
    }
}
