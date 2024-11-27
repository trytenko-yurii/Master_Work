using System.Text.RegularExpressions;
using Master_Work.Entities;
using Master_Work.Repository;
using Microsoft.ML;

namespace Master_Work.Models
{
    public class FakeNewsAnalyzer
    {
        private readonly MLContext _mlContext;
        private ITransformer _model;

        public FakeNewsAnalyzer()
        {
            _mlContext = new MLContext();
        }

        public void TrainModel(string trueNewsFilePath, string fakeNewsFilePath)
        {
            // Load true and fake news
            var trueNews = NewsFileProcessor.LoadNewsFromFile(trueNewsFilePath)
                .Select(news => new TrainingData
                {
                    Title = news.title,
                    Content = RemoveLinks(news.text), // Remove hyperlinks from content
                    Author = news.subject,
                    PublicationDate = ParseDate(news.date),
                    Label = false // True news
                });

            var fakeNews = NewsFileProcessor.LoadNewsFromFile(fakeNewsFilePath)
                .Select(news => new TrainingData
                {
                    Title = news.title,
                    Content = RemoveLinks(news.text), // Remove hyperlinks from content
                    Author = news.subject,
                    PublicationDate = ParseDate(news.date),
                    Label = true // Fake news
                });

            // Merge datasets
            var trainingData = trueNews.Concat(fakeNews).ToList();

            // Convert data to DataView
            var dataView = _mlContext.Data.LoadFromEnumerable(trainingData);

            // Build the pipeline
            var pipeline = _mlContext.Transforms.Text.FeaturizeText("Features", nameof(TrainingData.Content))
                .Append(_mlContext.BinaryClassification.Trainers.SdcaLogisticRegression(labelColumnName: nameof(TrainingData.Label), featureColumnName: "Features"));

            // Train the model
            _model = pipeline.Fit(dataView);
        }

        // Remove links using a regular expression
        private string RemoveLinks(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return text;

            // Regular expression to match URLs
            var linkPattern = @"https?:\/\/[^\s]+";
            return Regex.Replace(text, linkPattern, string.Empty);
        }

        // Check and parse date
        private string ParseDate(object date)
        {
            if (date is string dateString)
            {
                if (DateTime.TryParse(dateString, out DateTime parsedDate))
                {
                    return parsedDate.ToString("yyyy-MM-dd");
                }
                else
                {
                    return string.Empty;
                }
            }
            else
            {
                return string.Empty;
            }
        }

        public bool Predict(NewsArticle article)
        {
            var predictionEngine = _mlContext.Model.CreatePredictionEngine<TrainingData, NewsPrediction>(_model);

            var data = new TrainingData
            {
                Title = article.title,
                Content = RemoveLinks(article.text), // Remove hyperlinks from content
                Author = article.subject,
                PublicationDate = article.date.ToString()
            };

            var prediction = predictionEngine.Predict(data);
            return prediction.IsFake;
        }
    }
}