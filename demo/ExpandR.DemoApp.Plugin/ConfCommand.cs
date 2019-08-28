using ExpandR.DemoAPI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ExpandR.DemoApp.Plugin
{
    public class ConfCommand : ICommand
    {
        private readonly IConfiguration _conf;
        private readonly ILogger<ConfCommand> _log;

        public ConfCommand (IConfiguration conf, ILogger<ConfCommand> log)
        {
            this._conf = conf;
            this._log = log;
        }

        public string Syntax => "[<key> [<value>]]";

        public string Description => "List, read or write configuration values.";

        public int Call(string[] args)
        {
            string key, value;
            switch (args.Length)
            {
                case 0: // List
                    foreach (var entry in _conf.AsEnumerable())
                        _log.LogInformation($"{entry.Key} = {entry.Value}");
                    break;
                case 1: // Read
                    key = args[0];
                    value = _conf.GetValue(key, (string) null);
                    if (value is null)
                    {
                        _log.LogWarning($"{key} is undefined.");
                        return 2;
                    }
                    _log.LogInformation($"{key} = {value}");
                    break;
                case 2:
                    key = args[0];
                    value = args[1];
                    _conf[key] = value;
                    _log.LogInformation($"{key} = {value}");
                    break;
                default:
                    _log.LogWarning($"Invalid command invocation. Expected syntax is: {Syntax}");
                    return 1;
            }
            return 0;
        }
    }
}
