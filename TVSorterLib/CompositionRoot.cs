using Ninject;

namespace TVSorter
{
    public static class CompositionRoot
    {
        private static IKernel kernel;

        public static T Get<T>()
        {
            return kernel.Get<T>();
        }

        public static void SetKernel(IKernel kernel)
        {
            CompositionRoot.kernel = kernel;
        }
    }
}
