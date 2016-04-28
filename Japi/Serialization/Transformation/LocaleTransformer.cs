using System.Globalization;
using GoorooMania.Japi.Serialization.Data;
using GoorooMania.Japi.Serialization.Data.Path;

namespace GoorooMania.Japi.Serialization.Transformation {

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