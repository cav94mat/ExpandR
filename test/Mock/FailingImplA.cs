using System;

namespace cav94mat.ExpandR.Tests.Mock
{
    class FailingImplA : IMockServiceA
    {
        public void MethodA() => throw new InvalidOperationException("Failing implementation of " + nameof(IMockServiceA));
    }
}
