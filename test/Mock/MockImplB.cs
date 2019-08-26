using System;
using System.Collections.Generic;
using System.Text;

namespace cav94mat.ExpandR.Tests.Mock
{
    class MockImplB : IMockServiceB
    {
        public MockImplB(IMockServiceA svcA)
        {
            
        }
        public void MethodB() { }
    }
}
