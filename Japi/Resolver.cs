using System;
using System.IO;
using System.Reflection;

namespace NightlyCode.Japi {

    /// <summary>
    /// resolves embedded assemblies
    /// </summary>
    public static class Resolver {
        static bool initialized;

        static Assembly OnResolveAssembly(object sender, ResolveEventArgs args) {
            Stream assemblystream=null;
            if(args.Name.Contains("NightlyCode.Core"))
                assemblystream = typeof(Resolver).Assembly.GetManifestResourceStream("GoorooMania.Japi.Extern.NightlyCode.Core.dll");

            if(assemblystream == null)
                return null;

            using(MemoryStream ms = new MemoryStream()) {
                assemblystream.CopyTo(ms);
                return Assembly.Load(ms.ToArray());
            }
        }

        public static void Resolve() {
            // this actually does not need to do anything
            // since static ctor does all the work
            // still something has to be done for this method not to be "optimized"
            if(!initialized) {
                AppDomain.CurrentDomain.AssemblyResolve += OnResolveAssembly;
                initialized = true;
            }
        }
    }
}