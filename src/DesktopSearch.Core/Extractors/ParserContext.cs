namespace CodeSearch.Extractors
{
    public class ParserContext
    {
        public ContentPersistence Persistence { get; set; }
    }

    public enum ContentPersistence
    {
        None,
        Full,
        Compressed,
    }
}