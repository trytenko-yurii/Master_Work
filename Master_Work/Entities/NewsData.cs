using Microsoft.ML.Data;

namespace Master_Work.Entities
{
    public class NewsData
    {
        [LoadColumn(0)]
        public string Title { get; set; }

        [LoadColumn(1)]
        public string Content { get; set; }

        [LoadColumn(2)]
        public string Author { get; set; }

        [LoadColumn(3)]
        public string PublicationDate { get; set; }
    }

    public class NewsPrediction
    {
        [ColumnName("PredictedLabel")]
        public bool IsFake { get; set; }
    }

}
