using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;

namespace MediaRenamer;

public class Renamer {
    private static readonly Regex Regex = new(".*(?<year>20[0-4]{1}[0-9]{1})(?<month>[0-1]{1}[0-9]{1})(?<day>[0-3]{1}[0-9]{1}).+", RegexOptions.Compiled);

    private readonly string _directory;
    private readonly ILogger _logger;

    public Renamer(string directory, ILogger logger) {
        _directory = directory ?? throw new ArgumentNullException(nameof(directory));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public void Rename() {
        _logger.LogInformation("Processing directory {0}...", _directory);
        
        var filePaths = Directory.GetFiles(_directory, "*.*", SearchOption.TopDirectoryOnly);
        var i = 1;
        var totalCount = filePaths.Length;
        
        foreach (var file in filePaths) {
            _logger.LogInformation("Processing file {0} of {1}: {2}", i, totalCount, Path.GetFileName(file));
            
            var match = Regex.Match(Path.GetFileName(file));
            if (match.Success) {
                var yearStr = match.Groups["year"].Value;
                var monthStr = match.Groups["month"].Value;
                var dayStr = match.Groups["day"].Value;

                var targetDirectory = Path.Combine(_directory, $"{yearStr}-{monthStr}-{dayStr}");
                Directory.CreateDirectory(targetDirectory);

                var destination = Path.Combine(targetDirectory, Path.GetFileName(file));
                if (!File.Exists(destination)) {
                    File.Move(file, destination, false);
                }
            }

            i++;
        }
    }
}