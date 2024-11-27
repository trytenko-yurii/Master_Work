namespace Master_Work.Entities
{
    public class TrainingData
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string Author { get; set; }
        public string PublicationDate { get; set; }
        public bool Label { get; set; } // True для фейкових, False для правдивих
    }
}
