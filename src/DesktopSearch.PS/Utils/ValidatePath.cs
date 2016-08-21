using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace DesktopSearch.PS.Utils
{
    class ValidatePathAttribute : ValidateArgumentsAttribute
    {
        protected override void Validate(object arguments, EngineIntrinsics engineIntrinsics)
        {
            var listOfArguments = new List<string>();

            var stringArray = arguments as IEnumerable<string>;
            if (stringArray != null)
            {
                listOfArguments.AddRange(stringArray);
            }
            var simpleString = arguments as string;
            if (simpleString != null)
            {
                listOfArguments.Add(simpleString);
            }

            Validate(listOfArguments);
        }

        private void Validate(List<string> listOfArguments)
        {
            if (!listOfArguments.Any())
            {
                throw new Exception("No valid path specified!");
            }
            foreach (var item in listOfArguments)
            {
                if (!Directory.Exists(item))
                {
                    throw new Exception($"Path does not exist: {item}!");
                }
            }
        }
    }
}
