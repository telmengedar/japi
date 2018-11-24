using System.Globalization;
using NightlyCode.Japi.Serialization.Data;
using NightlyCode.Japi.Serialization.Data.Path;

namespace NightlyCode.Japi.Serialization.Transformation {

    /// <summary>
    /// writer for locale information (culture-info)
    /// </summary>
    public class LocaleTransformer : IDataTransformer {

        public IJavaData Convert(JavaObject @object) {

            string code = @object.SelectValue<string>("language");
            string country = @object.SelectValue<string>("country");
            if(!string.IsNullOrEmpty(country))
                code += "-" + country;
            return new JavaValue(CultureInfo.GetCultureInfo(code));
        }
    }
}