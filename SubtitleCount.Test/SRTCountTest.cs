using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SubtitleCount.Test
{
    [TestClass]
    public class SRTCountTest
    {
        [TestMethod]
        public void CountTest1()
        {
            string file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Subtitles", "test3.srt");

            var count = new SRTCount();

            var result = count.Count(File.ReadAllText(file));
            Console.WriteLine("Words: {0}", result.Words);
            Console.WriteLine("Lines: {0}", result.Lines);
        }

        [TestMethod]
        public void CountTest2()
        {
            string file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Subtitles", "test4.srt");

            var count = new SRTCount();

            var result = count.Count(File.ReadAllText(file));
            Console.WriteLine("Words: {0}", result.Words);
            Console.WriteLine("Lines: {0}", result.Lines);
        }
    }
}
