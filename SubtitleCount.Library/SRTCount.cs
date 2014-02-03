using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SubtitleCount
{
    [SubtitleCounter(".srt")]
    public class SRTCount : ISubtitleCount
    {
        private const string LinePattern = @"^.*$";

        public CountResult Count(string content)
        {
            int words = 0, lines = 0;
            var lineWords = new List<int>();

            var linesMatch = Regex.Matches(content, LinePattern, RegexOptions.Multiline);

            bool textLine = false;
            foreach (Match match in linesMatch)
            {
                if (match.Value.Contains("-->"))
                {
                    textLine = true;
                    continue;
                }
                if (textLine)
                {
                    string text = match.Value.Trim().Replace(" ", "").Replace("　", "");
                    int line = text.Length;
                    words += line;
                    lines++;
                    lineWords.Add(line);
                    textLine = false;
                }
            }

            var result = new CountResult(words, lines);
            result.LineWords = lineWords;

            return result;
        }
    }
}
