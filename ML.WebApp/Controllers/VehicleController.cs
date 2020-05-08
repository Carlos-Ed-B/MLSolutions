using Microsoft.AspNetCore.Mvc;
using ML.Business;
using ML.Infrastructure.Web.Bases;

namespace ML.WebApp.Controllers
{
    public class VehicleController : BaseController
    {
        public IActionResult ConsultBrand(string vehicleType)
        {
            var result = VehicleBusiness.ConsultBrand(vehicleType);
            return this.ApiResponseSuccess(result);
        }
    }
}