using System.Text;
using System.Text.RegularExpressions;

namespace DSA
{
    public static class TextAnalyzer
    {
        private static byte[] GetBytes(string text) => Encoding.UTF8.GetBytes(text);
        private static string GetString(byte[] data) => Encoding.UTF8.GetString(data);
        

        public static byte[] Concatenate(byte[] data, int r, int s)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(GetString(data));
            sb.Append($"\nr : {r}, s : {s};\n");
            return GetBytes(sb.ToString());
        }

        public static (byte[] message, int r, int s) Analyze(byte[] data)
        {
            const string pattern = "\\nr : [0-9]+, s : [0-9]+;\\n";

            var messageText = GetString(data);

            var rsString = Regex.Matches(messageText, pattern).Last().ToString();
            
            var matches = Regex.Matches(rsString, "[0-9]+");
            
            int r = int.Parse(matches[0].ToString());
            int s = int.Parse(matches[1].ToString());

            var sb = new StringBuilder();

            var newMessage = Regex.Replace(messageText, pattern, string.Empty);

            return (GetBytes(newMessage), r, s);
        }
    }
}
