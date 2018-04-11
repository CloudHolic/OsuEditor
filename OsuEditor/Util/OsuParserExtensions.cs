using System.IO;

namespace OsuEditor.Util
{
    public static class OsuParserExtensions
    {
        public static string GetDiffName(string fileName)
        {
            var diff = string.Empty;
            using (var reader = new StreamReader(fileName))
            {
                string currentLine;

                while ((currentLine = reader.ReadLine()) != null)
                    if (currentLine.StartsWith("Version:"))
                        diff = currentLine.Substring(currentLine.IndexOf(':') + 1);
            }

            return diff;
        }
    }
}
