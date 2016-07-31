using System.Diagnostics;
using System.IO;
using System.Text;

namespace DesktopSearch.Core.Extractors.Roslyn
{
    public static class CommentCleaner
    {
        public static string PrepareSinglelineComment(string comment)
        {
            var result = new StringBuilder(comment.Length);

            using (var reader = new StringReader(comment))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var trim = line.Trim(new[] { ' ', });
                    //Debug.Assert(trim.StartsWith("/// "));

                    if (trim.StartsWith("/// "))
                    {
                        result.AppendLine(trim.Substring(4));    
                    }
                    else
                    {
                        result.AppendLine(trim);
                    }
                }
            }

            // remove CR LF at the end
            return result.ToString().Substring(0, result.Length - 2);
        }

        public static string PrepareMultilineComment(string comment)
        {
            var result = new StringBuilder(comment.Length);

            using (var reader = new StringReader(comment))
            {
                string line = reader.ReadLine();
                var startPos = line.IndexOf("/*") + 3;
                result.AppendLine(line.Substring(startPos).TrimEnd(' '));

                while ((line = reader.ReadLine()) != null)
                {
                    var trim = line.Trim(new[] { ' ', });
                    result.AppendLine(trim);
                }
            }

            // remove CR LF at the end
            var s = result.ToString();
            return s.Substring(0, s.Length - 5);
        }
    }
}