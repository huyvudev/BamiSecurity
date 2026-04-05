namespace OpenXMLLibrary.Dtos
{
    public class OutputTextModelDto
    {
        public required string Text { get; set; }
        public List<string> Children { get; set; } = [];
    }
}
