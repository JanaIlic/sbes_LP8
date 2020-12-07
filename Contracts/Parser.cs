using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public class Parser
    {
        public static string Parse(string toParse)
        {
            string[] words = new string[] { };

            if (toParse.Contains("\\"))
            {
                words = toParse.Split('\\');
                return words[1];
            }
            else if (toParse.Contains("@"))
            {
                words = toParse.Split('@');
                return words[0];
            }
            else
                return toParse;
        }

    }
}
