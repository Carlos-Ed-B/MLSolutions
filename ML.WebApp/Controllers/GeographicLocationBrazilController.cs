using Microsoft.AspNetCore.Mvc;
using ML.Business;
using ML.Infrastructure.Web.Bases;

namespace ML.WebApp.Controllers
{
    public class GeographicLocationBrazilController : BaseController
    {
        public IActionResult Consult()
        {
            var result = GeographicLocationBusiness.LoadDataSource();
            return this.ApiResponseSuccess(result);
        }

        public IActionResult ConsultUF()
        {
            var result = GeographicLocationBusiness.ConsultUF();
            return this.ApiResponseSuccess(result);
        }

        public IActionResult ConsultMunicipio(string uf)
        {
            var result = GeographicLocationBusiness.ConsultMunicipio(uf);
            return this.ApiResponseSuccess(result);
        }

        public IActionResult ConsultKM(string municipio)
        {
            var result = GeographicLocationBusiness.ConsultKM(municipio);
            return this.ApiResponseSuccess(result);
        }
    }
}