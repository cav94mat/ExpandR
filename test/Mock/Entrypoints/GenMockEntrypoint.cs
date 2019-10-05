using cav94mat.ExpandR.Tests.Mock.Entrypoints;
using cav94mat.ExpandR.Tests.Mock.Host;

[assembly: GenEntrypoint(typeof(GenMockEntrypoint))]

namespace cav94mat.ExpandR.Tests.Mock.Entrypoints
{
    public class GenMockEntrypoint : IEntrypoint
    {
        public void Setup(IServiceCollectionExtender services)
        {
            services.Add(typeof(IMockGeneric<>), typeof(MockGenericImpl<>));
        }
    }
}
