using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace NightlyCode.Japi.Tests {
    public static class Resources {

        public static IEnumerable<ResourceData<Stream>> Raw
        {
            get
            {
                foreach (string resource in typeof(ObjectStreamTests).Assembly.GetManifestResourceNames().Where(n => n.StartsWith("NightlyCode.Japi.Tests.Data.Raw")))
                    yield return new ResourceData<Stream>(resource, typeof(ObjectStreamTests).Assembly.GetManifestResourceStream(resource));
            }
        }

        public static IEnumerable<ResourceData<Stream>> Encoded
        {
            get
            {
                foreach (string resource in typeof(ObjectStreamTests).Assembly.GetManifestResourceNames().Where(n => n.StartsWith("NightlyCode.Japi.Tests.Data.Encoded")))
                    yield return new ResourceData<Stream>(resource, typeof(ObjectStreamTests).Assembly.GetManifestResourceStream(resource));
            }
        }

        public static IEnumerable<ResourceData<byte[]>> Base64
        {
            get
            {
                foreach (string resource in typeof(ObjectStreamTests).Assembly.GetManifestResourceNames().Where(n => n.StartsWith("NightlyCode.Japi.Tests.Data.Base64")))
                {
                    using (Stream data = typeof(ObjectStreamTests).Assembly.GetManifestResourceStream(resource))
                    {
                        using (StreamReader reader = new StreamReader(data))
                        {
                            StringBuilder sb = new StringBuilder();
                            string line;
                            while ((line = reader.ReadLine()) != null)
                                sb.Append(line);
                            yield return new ResourceData<byte[]>(resource, Convert.FromBase64String(sb.ToString()));
                        }
                    }
                }
            }
        }

        public static IEnumerable<ResourceData<string>> JSon
        {
            get
            {
                foreach (string resource in typeof(ObjectStreamTests).Assembly.GetManifestResourceNames().Where(n => n.StartsWith("NightlyCode.Japi.Tests.Data.Json")))
                {
                    using (Stream data = typeof(ObjectStreamTests).Assembly.GetManifestResourceStream(resource))
                    {
                        using (StreamReader reader = new StreamReader(data))
                        {
                            yield return new ResourceData<string>(resource, reader.ReadToEnd());
                        }
                    }
                }
            }
        } 
    }
}