using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

public class FileName
{
    public string BaseName { get; set; }
    public int Version { get; set; }

    private static readonly Regex VersionRegex = new Regex(
        @"^(.*?)(?:\(v(\d+)\))?$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

    public static FileName Parse(string name)
    {
        var match = VersionRegex.Match(name);
        if (!match.Success)
            throw new ArgumentException("Invalid filename format.");

        string baseName = match.Groups[1].Value.Trim();
        int version = match.Groups[2].Success ? int.Parse(match.Groups[2].Value) : 0;

        return new FileName { BaseName = baseName, Version = version };
    }

    public override string ToString()
    {
        return Version > 0 ? $"{BaseName}(v{Version})" : BaseName;
    }
}


namespace _23__File_Name_Versioning
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var existingNames = new List<string>
        {
            "Report", "Summary(v1)", "Summary(v2)", "Data(v1)"
        };

            var newNames = new List<string>
        {
            "report", "Summary", "Data", "NewFile", "Summary"
        };

            var finalList = EnsureUniqueNames(existingNames, newNames);

            Console.WriteLine("Final Unique Names:");
            foreach (var name in finalList)
            {
                Console.WriteLine(name);
            }
        }

        public static List<string> EnsureUniqueNames(List<string> existing, List<string> incoming)
        {
            var allFiles = existing
                .Select(FileName.Parse)
                .ToList();

            var used = new HashSet<string>(allFiles.Select(f => f.ToString()), StringComparer.OrdinalIgnoreCase);

            foreach (var newName in incoming)
            {
                var parsed = FileName.Parse(newName);
                string candidate = parsed.ToString();

                while (used.Contains(candidate))
                {
                    parsed.Version++;
                    candidate = parsed.ToString();
                }

                used.Add(candidate);
                allFiles.Add(parsed);
            }

            return allFiles.Select(f => f.ToString()).ToList();
        }
    }
}




