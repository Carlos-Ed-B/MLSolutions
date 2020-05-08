using System;
using Xunit;

namespace ML.Test
{
    public class InfrastructureTest : BaseTest
    {
        [Fact]
        public void ConsoleWriteLineToTxt()
        {
            this._logHelper.DoTest();

            Console.WriteLine($"ConsoleWriteLineToTxt feito.1.2.3.4.5.6.7.{DateTime.Now}");

        }
    }
}
