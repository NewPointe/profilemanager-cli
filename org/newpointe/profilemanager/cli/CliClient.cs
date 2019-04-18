using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using org.newpointe.util;
using org.newpointe.profilemanager.api;
using org.newpointe.profilemanager.cli.commands;

namespace org.newpointe.profilemanager.cli
{

    class CliClient: Command
    {
        ///<summary>A dictionary of subcommands.</summary>
        private Dictionary<string, Command> subcommands = new Dictionary<string, Command>();
        
        /// <summary>A dictionary of global options.</summary>
        private Dictionary<string, string> globalOptions = new Dictionary<string, string>();

        private string GetGlobal(string key, string defaultValue) {
            
            if(key == null) {
                return null;
            }
            
            string storedVal;
            if(globalOptions.TryGetValue(key, out storedVal) && !string.IsNullOrWhiteSpace(storedVal) ) {
                return storedVal;
            }
            else {
                return defaultValue;
            }

        }
        private void SetGlobal(string key, string value) {
            
            if(key == null) {
                return;
            }

            globalOptions[key] = value;

        }
        
        /// <summary>A dictionary of clients we've logged into.</summary>
        private Dictionary<string, ProfileManagerClient> clients = new Dictionary<string, ProfileManagerClient>();
        
        public CliClient() {
            this.RegisterSubcommand("lock", new LockCommand(this));
            this.RegisterSubcommand("restart", new RestartCommand(this));
            this.RegisterSubcommand("shutdown", new ShutDownCommand(this));
        }
        
        /// <summary>Registers a subcommand.</summary>
        /// <param name="commandKey">The name of the command.</param>
        /// <param name="commandEvaluator">The class that handles the command.</param>
        ///
        private void RegisterSubcommand(string commandKey, Command commandEvaluator) {
            this.subcommands.Add(commandKey, commandEvaluator);
        }
        
        public override async Task Execute(string[] args, bool isRoot)
        {
            // Handle environment variables
            SetGlobal("SERVER", Environment.GetEnvironmentVariable("PROFILEMANAGER_SERVER"));
            SetGlobal("USERNAME", Environment.GetEnvironmentVariable("PROFILEMANAGER_USERNAME"));
            SetGlobal("PASSWORD", Environment.GetEnvironmentVariable("PROFILEMANAGER_PASSWORD"));

            // Handle arguments
            string command = CliParseUtil.TryShiftString(args, out args);

            // Handle no arguments
            if(command == null) {
                if(isRoot) ShowHelp();
                return;
            }

            // Handle pre-command arguments
            while(command.StartsWith("-")) {

                switch (command)
                {
                    case "-h":
                    case "--help":
                        ShowHelp();
                        return;
                    case "-v":
                    case "--version":
                        Console.WriteLine("Version: {0}", typeof(CliClient).Assembly.GetName().Version);
                        return;

                    case "-u":
                    case "--username":
                        string username = CliParseUtil.TryShiftString(args, out args);
                        if(username == null) {
                            Console.WriteLine("Option '{0}' requires a value", command);
                            ShowHelp();
                            return;
                        }
                        else {
                            SetGlobal("USERNAME", username);
                        }
                        break;

                    case "-p":
                    case "--password":
                        string password = CliParseUtil.TryShiftString(args, out args);
                        if(password == null) {
                            Console.WriteLine("Option '{0}' requires a value", command);
                            ShowHelp();
                            return;
                        }
                        else {
                            SetGlobal("PASSWORD", password);
                        }
                        break;

                    case "-s":
                    case "--server":
                        string server = CliParseUtil.TryShiftString(args, out args);
                        if(server == null) {
                            Console.WriteLine("Option '{0}' requires a value", command);
                            ShowHelp();
                            return;
                        }
                        else {
                            SetGlobal("SERVER", server);
                        }
                        break;

                    default:
                        Console.WriteLine("Unknown Option '{0}'", command);
                        ShowHelp();
                        return;
                }

                command = CliParseUtil.TryShiftString(args, out args);
            }

            // Handle commands
            if(command == "help" || command == "version") {
                ShowHelp();
                return;
            }

            var commandEvaluator = subcommands.GetValueOrDefault(command, null);

            if(commandEvaluator == null) {
                Console.WriteLine("Unknown Command '{0}'", command);
                ShowHelp();
                return;
            }
            else {
                await commandEvaluator.Execute(args, false);
            }

        }

        public void ShowHelp() {
            Console.WriteLine("CLI interface for interacting with a Profile Manager server.");
            Console.WriteLine("");
            Console.WriteLine("Version: {0}", typeof(CliClient).Assembly.GetName().Version);
            Console.WriteLine("");
            Console.WriteLine("Usage:");
            Console.WriteLine("    profilemanager ([option] | <command>)...");
            Console.WriteLine("");
            Console.WriteLine("Options:");
            Console.WriteLine("    -h, --help                            Show this help and exit.");
            Console.WriteLine("    -v, --version                         Show the version and exit.");
            Console.WriteLine("    -s <url>, --server <url>              Set the server to connect to (Defaults to localhost).");
            Console.WriteLine("    -u <username>, --username <username>  Set the username for the server.");
            Console.WriteLine("    -p <password>, --password <password>  Set the password for the server.");
            Console.WriteLine("");
            Console.WriteLine("Environment Variables:");
            Console.WriteLine("    PROFILEMANAGER_SERVER    Set the server to connect to.");
            Console.WriteLine("    PROFILEMANAGER_USERNAME  Set the username for the server.");
            Console.WriteLine("    PROFILEMANAGER_PASSWORD  Set the password for the server.");
            Console.WriteLine("");
            Console.WriteLine("Commands: {0}", string.Join(", ", subcommands.Keys));
            Console.WriteLine("");
        }

        public async Task<ProfileManagerClient> GetLoggedInClient()
        {
            string serverUrl = GetGlobal("SERVER", "http://localhost");
            ProfileManagerClient client = clients.GetValueOrDefault(serverUrl, null);


            if(client == null) {
                
                Console.WriteLine("Using server '{0}'", serverUrl);
                client = new ProfileManagerClient(serverUrl);

                string username = GetGlobal("USERNAME", null);
                
                while(string.IsNullOrWhiteSpace(username)) {
                    Console.Write("Username: ");
                    username = Console.ReadLine();
                }

                SetGlobal("USERNAME", username);
                
                string password = GetGlobal("PASSWORD", null);
                
                while(string.IsNullOrWhiteSpace(password)) {
                    Console.Write("Password: ");
                    password = ConsoleUtil.ReadLine_Hidden();
                    Console.WriteLine("");
                }

                SetGlobal("USERNAME", password);

                await client.Login(username, password);

                clients.Add(serverUrl, client);

            }

            return client;

        }
    }

}