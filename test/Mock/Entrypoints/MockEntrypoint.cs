using cav94mat.ExpandR;
using cav94mat.ExpandR.Tests.Mock.Entrypoints;

[assembly: Entrypoint(typeof(MockEntrypoint))]

namespace cav94mat.ExpandR.Tests.Mock.Entrypoints
{
    public class MockEntrypoint : IEntrypoint
    {
        public void Setup(IServiceCollectionExtender services)
        {
            services.Add<IMockServiceA, MockImplA>();
        }
    }
}
