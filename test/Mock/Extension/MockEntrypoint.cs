using cav94mat.ExpandR;

[assembly: Entrypoint(typeof(cav94mat.ExpandR.Tests.Mock.MockEntrypoint))]

namespace cav94mat.ExpandR.Tests.Mock
{
    public class MockEntrypoint : IEntrypoint
    {
        public void Setup(IServiceCollectionExtender services)
        {
            services.Add<IMockServiceA, MockImplA>();
        }
    }
}
