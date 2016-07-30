using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynParseHelper.Classes
{
    public class APIClass
    {
        public string TestMethod(int cnt, string[] parameters)
        {
            return null;
        }

        public string TestMethod2(int cnt)
        {
            return null;
        }

        private void ToBeIgnored()
        {
        }
    }
}
