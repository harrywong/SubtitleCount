using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SubtitleCount
{
    [SubtitleCounter(".ass")]
    public class ASSCount : ISubtitleCount
    {
        private const string TextPattern = @"^Dialogue:(?:.*?,){9}(?<text>.*)$";
        private const string TagPattern = @"{.*?}";

        public CountResult Count(string content)
        {
            int words = 0, lines = 0;
            var lineWords = new List<int>();
            var regexResult = Regex.Matches(content, TextPattern, RegexOptions.Multiline);

            lines = regexResult.Count;
            foreach (Match match in regexResult)
            {
                if (match.Groups["text"].Success)
                {
                    string text = match.Groups["text"].Value.Trim().Replace(" ", "").Replace("　", "");
                    var tagRegexResult = Regex.Matches(text, TagPattern);
                    int line = text.Length - tagRegexResult.Cast<Match>().Sum(c => c.Value.Length);
                    words += line;
                    lineWords.Add(line);
                }
            }

            var result = new CountResult(words, lines) { LineWords = lineWords };
            return result;
        }
    }
}
