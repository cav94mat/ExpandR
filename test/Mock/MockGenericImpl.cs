using System;
using System.Collections.Generic;
using System.Text;

namespace cav94mat.ExpandR.Tests.Mock
{
    class MockGenericImpl<T> : IMockGeneric<T>
    {
        public string GetTypeName() => typeof(T).Name;
    }
}
