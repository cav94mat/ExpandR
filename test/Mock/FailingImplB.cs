using System;

namespace cav94mat.ExpandR.Tests.Mock
{
    class FailingImplB : IMockServiceB
    {
        public void MethodB() => throw new InvalidOperationException("Failing implementation of " + nameof(IMockServiceB));
    }
}
