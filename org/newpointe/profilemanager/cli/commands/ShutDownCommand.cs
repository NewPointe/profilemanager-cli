using System;
using System.Threading.Tasks;

using org.newpointe.util;
using org.newpointe.profilemanager.api;

namespace org.newpointe.profilemanager.cli.commands
{

    class ShutDownCommand : Command
    {
        private CliClient parent;
        public ShutDownCommand(CliClient parent)
        {
            this.parent = parent;
        }

        public override async Task Execute(string[] args, bool isRoot)
        {

            switch (CliParseUtil.TryShiftString(args, out args))
            {
                case "device":

                    var deviceId = CliParseUtil.TryShiftId(args, out args);
                    if (deviceId.HasValue)
                    {

                        var client = await parent.GetLoggedInClient();

                        Console.WriteLine("Starting a ShutDown task for Device " + deviceId.ToString());
                        await client.ShutDownDevice(deviceId.Value);

                        if (args.Length > 0) await parent.Execute(args, false);

                    }
                    else
                    {
                        Console.WriteLine("Usage: profilemanager ShutDown device <id>");
                    }

                    break;
                case "group":

                    var groupId = CliParseUtil.TryShiftId(args, out args);
                    if (groupId.HasValue)
                    {

                        var client = await parent.GetLoggedInClient();

                        Console.WriteLine("Starting a ShutDown task for Group " + groupId.ToString());
                        await client.ShutDownDeviceGroup(groupId.Value);

                        if (args.Length > 0) await parent.Execute(args, false);

                    }
                    else
                    {
                        Console.WriteLine("Usage: profilemanager ShutDown group <id>");
                    }

                    break;
                default:

                    Console.WriteLine("Usage: profilemanager ShutDown <device|group> <id>");

                    return;
            }

        }
    }

}