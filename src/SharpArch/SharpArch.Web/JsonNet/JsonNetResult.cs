using System.Web.Mvc;
using System.Text;
using System.Web;
using Newtonsoft.Json;
using System;

namespace SharpArch.Web.JsonNet
{
    /// <summary>
    /// An ActionResult to return JSON from ASP.NET MVC to the browser using Json.NET.
    /// Taken from http://james.newtonking.com/archive/2008/10/16/asp-net-mvc-and-json-net.aspx
    /// </summary>
    public class JsonNetResult : ActionResult
    {
        public JsonNetResult() : this(null, null, null) { }
        public JsonNetResult(Object data) : this(data, null, null) { }
        public JsonNetResult(Object data, String contentType) : this(data, contentType, null) { }

        public JsonNetResult(Object data, String contentType, Encoding encoding) {
            SerializerSettings = new JsonSerializerSettings();

            Data = data;
            ContentType = contentType;
            ContentEncoding = encoding;
        }

        public override void ExecuteResult(ControllerContext context) {
            if (context == null)
                throw new ArgumentNullException("context");

            HttpResponseBase response = context.HttpContext.Response;

            response.ContentType = !string.IsNullOrEmpty(ContentType)
              ? ContentType
              : "application/json";

            if (ContentEncoding != null)
                response.ContentEncoding = ContentEncoding;

            if (Data != null) {
                JsonTextWriter writer = new JsonTextWriter(response.Output) { Formatting = Formatting };
                JsonSerializer serializer = JsonSerializer.Create(SerializerSettings);
                serializer.Serialize(writer, Data);
                writer.Flush();
            }
        }

        public Encoding ContentEncoding { get; set; }
        public string ContentType { get; set; }
        public object Data { get; set; }

		[CLSCompliant(false)]
		public JsonSerializerSettings SerializerSettings { get; set; }

		[CLSCompliant(false)]
		public Formatting Formatting { get; set; }
    }
}
