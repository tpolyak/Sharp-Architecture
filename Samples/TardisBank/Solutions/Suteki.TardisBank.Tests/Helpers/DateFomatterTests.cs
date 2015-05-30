// ReSharper disable InconsistentNaming
using NUnit.Framework;

namespace Suteki.TardisBank.Tests.Helpers
{
    using global::Suteki.TardisBank.Web.Mvc.Utilities;

    [TestFixture]
    public class DateFomatterTests 
    {
        [Test]
        public void Should_return_the_correct_date_format()
        {
            GetJQueryDateFormatFor("en-GB").ShouldEqual("dd/mm/yy");
            GetJQueryDateFormatFor("en-US").ShouldEqual("m/d/yy");
            GetJQueryDateFormatFor("fr").ShouldEqual("dd/mm/yy");
            GetJQueryDateFormatFor("de-CH").ShouldEqual("dd.mm.yy");
            GetJQueryDateFormatFor("de").ShouldEqual("dd.mm.yy");
        }

        static string GetJQueryDateFormatFor(string language)
        {
            var culture = new System.Globalization.CultureInfo(language);
            System.Threading.Thread.CurrentThread.CurrentCulture = culture;

            return DateFormatter.CurrentJQuery;
        }
    }
}
// ReSharper restore InconsistentNaming