using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SubtitleCount.Test
{
    [TestClass]
    public class ASSCountTest
    {
        [TestMethod]
        public void CountTest1()
        {
            string file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Subtitles", "test1.ass");

            var count = new ASSCount();

            var result = count.Count(File.ReadAllText(file));
            Console.WriteLine("Words: {0}", result.Words);
            Console.WriteLine("Lines: {0}", result.Lines);
        }

        [TestMethod]
        public void CountTest2()
        {
            string file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Subtitles", "test2.ass");

            var count = new ASSCount();

            var result = count.Count(File.ReadAllText(file));
            Console.WriteLine("Words: {0}", result.Words);
            Console.WriteLine("Lines: {0}", result.Lines);
        }

        [TestMethod]
        public void CountCompare()
        {
            string file1 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Subtitles", "test1.ass");
            string file2 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Subtitles", "test2.ass");

            var count = new ASSCount();

            var result1 = count.Count(File.ReadAllText(file1));
            var result2 = count.Count(File.ReadAllText(file2));

            for (int i = 0; i < result1.Lines; i++)
            {
                if (i > result2.Lines)
                {
                    Console.WriteLine("Not Found Line: #{0}", i);
                }
                if (result1.LineWords[i] != result2.LineWords[i])
                {
                    Console.WriteLine("Line #{0}, ASS1: {1}, ASS2:{2}", i, result1.LineWords[i], result2.LineWords[i]);
                }
            }
        }
    }
}
