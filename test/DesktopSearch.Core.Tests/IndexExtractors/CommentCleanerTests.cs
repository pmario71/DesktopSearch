using CodeSearch.Extractors.Roslyn;
using Xunit;

namespace CodeSearchTests.Indexing
{
    public class CommentCleanerTests
    {
        [Fact]
        public void PrepareSinglelineComment_on_single_comment_line()
        {
            const string input = @"/// <summary>This is an xml doc comment</summary>";
            var result = CommentCleaner.PrepareSinglelineComment(input);

            Assert.Equal("<summary>This is an xml doc comment</summary>", result);
        }

        [Fact]
        public void PrepareSinglelineComment_on_multiple_comment_lines()
        {
            const string input = @"   /// <summary>First line.
   /// second line</summary>  ";
            var result = CommentCleaner.PrepareSinglelineComment(input);

            Assert.Equal("<summary>First line.\r\nsecond line</summary>", result);
        }

        [Fact]
        public void PrepareMultilineComment_on_single_comment_line()
        {
            const string input = @"/* <summary>This is an xml doc comment</summary> */";
            var result = CommentCleaner.PrepareMultilineComment(input);

            Assert.Equal("<summary>This is an xml doc comment</summary>", result);
        }

        [Fact]
        public void PrepareMultilineComment_on_multiple_comment_lines()
        {
            const string input = @"/* <summary>First line. 
 some other line.</summary> */";
            var result = CommentCleaner.PrepareMultilineComment(input);

            Assert.Equal("<summary>First line.\r\nsome other line.</summary>", result);
        }
    }
}