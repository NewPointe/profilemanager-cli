using System;
using System.Threading.Tasks;

using org.newpointe.util;
using org.newpointe.profilemanager.api;
using org.newpointe.profilemanager.cli;

namespace ProfileManagerCli
{
    class Program
    {

        static void Main(string[] args)
        {
            new CliClient().Execute(args, true).GetAwaiter().GetResult();
        }

    }
}
