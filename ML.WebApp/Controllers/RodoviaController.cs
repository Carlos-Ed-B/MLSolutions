using Microsoft.AspNetCore.Mvc;
using ML.Infrastructure.Web.Bases;
using ML.Modes.HighwayBrazilAccidents;
using ML.Trainings.HighwayBrazilAccidents;

namespace ML.WebApp.Controllers
{
    [Route("[controller]")]
    public class RodoviaController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("DoPredict")]
        public IActionResult DoPredict(HighwayBrazilAccidentsData rodoviaData)
        {
            rodoviaData.Tipo_acidente = string.Empty;

            var rodoviaTraining = new HighwayBrazilAccidentsTrainning();
            var result = rodoviaTraining.DoPredict(rodoviaData);

            return this.ApiResponseSuccess(result.Prediction);
        }
    }
}