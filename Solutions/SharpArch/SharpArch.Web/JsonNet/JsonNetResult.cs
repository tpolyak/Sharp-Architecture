namespace SharpArch.Web.JsonNet
{
    using System;
    using System.Text;
    using System.Web.Mvc;

    using Newtonsoft.Json;

    /// <summary>
    ///     An ActionResult to return JSON from ASP.NET MVC to the browser using Json.NET.
    ///     Taken from http://james.newtonking.com/archive/2008/10/16/asp-net-mvc-and-json-net.aspx
    /// </summary>
    public class JsonNetResult : ActionResult
    {
        public JsonNetResult()
            : this(null, null, null)
        {
        }

        public JsonNetResult(object data)
            : this(data, null, null)
        {
        }

        public JsonNetResult(object data, string contentType)
            : this(data, contentType, null)
        {
        }

        public JsonNetResult(object data, string contentType, Encoding encoding)
        {
            this.SerializerSettings = new JsonSerializerSettings();

            this.Data = data;
            this.ContentType = contentType;
            this.ContentEncoding = encoding;
        }

        public Encoding ContentEncoding { get; set; }

        public string ContentType { get; set; }

        public object Data { get; set; }

        [CLSCompliant(false)]
        public Formatting Formatting { get; set; }
        [CLSCompliant(false)]
        public JsonSerializerSettings SerializerSettings { get; set; }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var response = context.HttpContext.Response;

            response.ContentType = !string.IsNullOrEmpty(this.ContentType) ? this.ContentType : "application/json";

            if (this.ContentEncoding != null)
            {
                response.ContentEncoding = this.ContentEncoding;
            }

            if (this.Data != null)
            {
                var writer = new JsonTextWriter(response.Output) { Formatting = this.Formatting };
                var serializer = JsonSerializer.Create(this.SerializerSettings);
                serializer.Serialize(writer, this.Data);
                writer.Flush();
            }
        }
    }
}