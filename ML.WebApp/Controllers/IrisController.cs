using Microsoft.AspNetCore.Mvc;
using ML.Infrastructure.Enums;
using ML.Infrastructure.Web.Bases;
using ML.Modes.Iris;
using ML.Trainings.Iris;

namespace ML.WebApp.Controllers
{
    [Route("[controller]")]
    public class IrisController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("DoPredict")]
        public IActionResult DoPredict([FromBody]IrisData data)
        {
            var mushroomsTraining = new IrisTraining();
            var result = mushroomsTraining.DoPredict(data);

            return this.ApiResponse(result, CustomTypeResultEnum.Success);
        }
    }
}