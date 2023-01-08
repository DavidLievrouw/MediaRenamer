using CommandLine;
using Microsoft.Extensions.Logging;

namespace MediaRenamer;

public class Program {
    private static ILogger _logger;

    public static void Main(string[] args) {
        using var loggerFactory = LoggerFactory.Create(builder => {
            builder
                .AddFilter("Microsoft", LogLevel.Warning)
                .AddFilter("System", LogLevel.Warning)
                .AddFilter("MediaRenamer.Program", LogLevel.Debug)
                .AddConsole();
        });
        _logger = loggerFactory.CreateLogger<Program>();

        Parser.Default.ParseArguments<Options>(args)
            .WithParsed(o => {
                var renamer = new Renamer(o.Directory, _logger);
                renamer.Rename();
            });
    }

    public class Options {
        [Option('d', "directory", Required = true, HelpText = "The directory with the source files.")]
        public string Directory { get; set; }
    }
}