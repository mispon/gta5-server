using Ninject.Syntax;

namespace gta_mp_server.IoC {
    /// <summary>
    /// Инкапсуляция кернела
    /// </summary>
    internal class ServerKernel {
        public static IResolutionRoot Kernel { get; set; }
    }
}