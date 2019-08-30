using System.Collections.Generic;
using cav94mat.ExpandR.Host;
using cav94mat.ExpandR.Tests.Base;
using cav94mat.ExpandR.Tests.Mock;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace cav94mat.ExpandR.Tests
{
    public class CoreTest : TestBase
    {
        /// <summary>
        ///  During this test, <see cref="IMockServiceA"/> and <see cref="IMockServiceB"/> are both exposed to the plugin framework.
        ///  <para>
        ///  1. <see cref="IMockServiceA"/> has <see cref="FailingImplA"/> as its default implementation, whose <see cref="FailingImplA.MethodA"/> method
        ///  would cause the test to fail if the test plugin (see <see cref="MockEntrypoint"/>) will not register a different implementation.
        ///  </para>
        ///  <para>
        ///  2. <see cref="IMockServiceB"/> has <see cref="MockImplB"/> as its default implementation, and the test plugin (see <see cref="MockEntrypoint"/>)
        ///  won't implement it. The test will fail if the default implementation isn't returned.
        ///  </para>
        /// </summary>
        [Fact]
        public void TestSingleImplementation()
        {
            var ioc = new ServiceCollection();
            ioc.AddExpandR(configure =>
            {
                // Services
                configure.ExposeSingleton<IMockServiceA, FailingImplA>();
                configure.ExposeTransient<IMockServiceB, MockImplB>();
                // Plugins
                configure.LoadPlugin(TestAssembly); // Test plugin
            });
            using var services = ioc.BuildServiceProvider();
            // Call the services
            services.GetService<IMockServiceA>().MethodA();    // --> MockImplA.SomeMethod()
            services.GetService<IMockServiceB>().MethodB(); // --> MockImplB.SomeMethod()
        }
        /// <summary>
        /// During this test, <see cref="IMockServiceA"/> and <see cref="IMockServiceB"/> are both exposed to the plugin framework,
        /// and both support multiple implementations.
        ///  <para>
        ///  1. <see cref="IMockServiceA"/> has <see cref="FailingImplA"/> as its default implementation; in addition to that,
        ///  the test plugin (see <see cref="MockEntrypoint"/>) will provide its own implementation (<see cref="MockImplA"/>).
        ///  </para>
        ///  <para>
        ///  2. <see cref="IMockServiceB"/> has no implementations.
        ///  </para>
        /// </summary>
        [Fact]
        public void TestMultiImplementations()
        {
            var ioc = new ServiceCollection();
            ioc.AddExpandR(configure =>
            {
                // Services
                configure.ExposeMultiSingleton<IMockServiceA, FailingImplA>();
                configure.ExposeMultiTransient<IMockServiceB>();
                // Plugins
                configure.LoadPlugin(TestAssembly); // Test plugin
            });
            using var services = ioc.BuildServiceProvider();
            // Call the services
            var aImpls = new List<IMockServiceA>(services.GetServices<IMockServiceA>());
            Assert.Equal(2, aImpls.Count);
            Assert.Equal(typeof(MockImplA), aImpls[0].GetType());
            Assert.Equal(typeof(FailingImplA), aImpls[1].GetType());

            var bImpls = new List<IMockServiceB>(services.GetServices<IMockServiceB>());
            Assert.Empty(bImpls);
        }

    }
}
