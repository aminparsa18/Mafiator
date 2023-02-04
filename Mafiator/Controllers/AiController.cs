using Mafiator.Api.Controllers.Base;
using Mafiator.Common.Data.Dtos.Api;
//using MafiatorML.Model;
using Microsoft.AspNetCore.Mvc;
using HttpGetAttribute = Microsoft.AspNetCore.Mvc.HttpGetAttribute;

namespace Mafiator.Api.Controllers
{
    public class AiController : ApiBaseController
    {
      
        [HttpGet]
        public IActionResult GetSentiment(string fileName)
        {
           // var path = Path.Combine(webHostEnvironment.ContentRootPath,fileName);
            //AudioToImageConverter.CreateSpectrogram(fileName);
            //var sampleData = new ModelInput()
            //{
            //    ImageSource = fileName
            //};
            // Make a single prediction on the sample data and print results
           // var predictionResult = consumeModel.Predict(sampleData);
            return Ok(new ApiResult<string>()
            {
                IsSuccess = true,
             //   Data = predictionResult.Prediction+fileName
            });
        }
    }
}
