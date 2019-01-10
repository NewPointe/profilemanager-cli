using System;
using System.Threading.Tasks;

using org.newpointe.util;
using org.newpointe.profilemanager.api;

namespace org.newpointe.profilemanager.cli.commands
{

    class LockCommand : Command
    {
        private CliClient parent;
        public LockCommand(CliClient parent)
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

                        Console.WriteLine("Starting a Lock task for Device " + deviceId.ToString());
                        await client.LockDevice(deviceId.Value);

                        if (args.Length > 0) await parent.Execute(args, false);

                    }
                    else
                    {
                        Console.WriteLine("Usage: profilemanager lock device <id>");
                    }

                    break;
                case "group":

                    var groupId = CliParseUtil.TryShiftId(args, out args);
                    if (groupId.HasValue)
                    {

                        var client = await parent.GetLoggedInClient();

                        Console.WriteLine("Starting a Lock task for Group " + groupId.ToString());
                        await client.LockDeviceGroup(groupId.Value);

                        if (args.Length > 0) await parent.Execute(args, false);

                    }
                    else
                    {
                        Console.WriteLine("Usage: profilemanager lock group <id>");
                    }

                    break;
                default:

                    Console.WriteLine("Usage: profilemanager lock <device|group> <id>");

                    return;
            }

        }
    }

}