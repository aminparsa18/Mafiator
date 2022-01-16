using System;
using System.Linq;

namespace Mafiator.Common.Helpers
{
    public class RandomHelper
    {
        private const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
        private static readonly Random Random=new();
        public static string RandomStr(int len)
        {
            return new string(Enumerable.Repeat(chars,len).Select(s=>s[Random.Next(s.Length)]).ToArray());
        }
    }
}
