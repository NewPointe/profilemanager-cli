using System;
using System.Threading.Tasks;

using org.newpointe.util;
using org.newpointe.profilemanager.api;

namespace org.newpointe.profilemanager.cli.commands
{

    class RestartCommand : Command
    {
        private CliClient parent;
        public RestartCommand(CliClient parent)
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

                        Console.WriteLine("Starting a restart task for Device " + deviceId.ToString());
                        await client.RestartDevice(deviceId.Value);

                        if (args.Length > 0) await parent.Execute(args, false);

                    }
                    else
                    {
                        Console.WriteLine("Usage: profilemanager restart device <id>");
                    }

                    break;
                case "group":

                    var groupId = CliParseUtil.TryShiftId(args, out args);
                    if (groupId.HasValue)
                    {

                        var client = await parent.GetLoggedInClient();

                        Console.WriteLine("Starting a restart task for Group " + groupId.ToString());
                        await client.RestartDeviceGroup(groupId.Value);

                        if (args.Length > 0) await parent.Execute(args, false);

                    }
                    else
                    {
                        Console.WriteLine("Usage: profilemanager restart group <id>");
                    }

                    break;
                default:

                    Console.WriteLine("Usage: profilemanager restart <device|group> <id>");

                    return;
            }

        }
    }

}