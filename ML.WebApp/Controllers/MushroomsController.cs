using Microsoft.AspNetCore.Mvc;
using ML.Infrastructure.Enums;
using ML.Infrastructure.Web.Bases;
using ML.Modes.Mushrooms;
using ML.Trainings.Mushrooms;

namespace ML.WebApp.Controllers
{
    [Route("[controller]")]
    public class MushroomsController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("DoPredict")]
        public IActionResult DoPredict(MushroomsData mushroomsData)
        {
            mushroomsData.Class = string.Empty;

            var mushroomsTraining = new MushroomsTrainning();
            var result = mushroomsTraining.DoPredict(mushroomsData);
            var customTypeResultEnum = CustomTypeResultEnum.Success;

            if (result.Prediction.ToLower().Equals("p"))
            {
                customTypeResultEnum = CustomTypeResultEnum.Warning;
            }

            return this.ApiResponse(result.Description, customTypeResultEnum);
        }
    }
}