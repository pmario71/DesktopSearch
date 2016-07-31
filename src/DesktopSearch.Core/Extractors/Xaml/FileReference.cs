namespace DesktopSearch.Core.Extractors.Xaml
{
    public class FileReference
    {
        public FileReference()
        {
        }

        public FileReference(string file, int lineNR)
        {
            this.File = file;
            this.LineNR = lineNR;
        }

        public string File { get; private set; }
        public int LineNR { get; private set; }
    }
}