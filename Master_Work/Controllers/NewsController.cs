using Master_Work.Entities;
using Master_Work.Models;
using Microsoft.AspNetCore.Mvc;

namespace Master_Work.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NewsController : ControllerBase
    {
        private readonly FakeNewsAnalyzer _newsAnalyzer;

        public NewsController()
        {
            _newsAnalyzer = new FakeNewsAnalyzer();
        }

        [HttpPost("train")]
        public IActionResult TrainModel([FromForm] IFormFile trueNewsFile, [FromForm] IFormFile fakeNewsFile)
        {
            var trueNewsPath = Path.GetTempFileName();
            var fakeNewsPath = Path.GetTempFileName();

            using (var stream = new FileStream(trueNewsPath, FileMode.Create))
            {
                trueNewsFile.CopyTo(stream);
            }

            using (var stream = new FileStream(fakeNewsPath, FileMode.Create))
            {
                fakeNewsFile.CopyTo(stream);
            }

            _newsAnalyzer.TrainModel(trueNewsPath, fakeNewsPath);

            return Ok("Model trained successfully.");
        }

        [HttpPost("predict")]
        public IActionResult Predict([FromBody] NewsArticle article)
        {
            var result = _newsAnalyzer.Predict(article);
            return Ok(new { IsFake = result });
        }
    }


}
