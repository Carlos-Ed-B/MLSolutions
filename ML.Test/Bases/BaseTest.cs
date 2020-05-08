using ML.Infrastructure;
using System;

namespace ML.Test
{
    public class BaseTest : IDisposable
    {
        public readonly LogHelper _logHelper;
        public BaseTest()
        {
            this._logHelper = new LogHelper();
        }

        public void Dispose()
        {
            this._logHelper.Flush();
        }
    }
}
