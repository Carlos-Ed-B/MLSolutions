using ML.Business;
using System.Linq;
using Xunit;

namespace ML.Test
{
    public class GeographicLocationBusinessTest
    {
        [Fact]
        public void Consult()
        {
            var result = GeographicLocationBusiness.LoadDataSource();
            Assert.True(result.Any());

            var ufList = GeographicLocationBusiness.ConsultUF();
            Assert.True(ufList.Any());

            var municipioList = GeographicLocationBusiness.ConsultMunicipio("SP");
            Assert.True(municipioList.Any());

        }
    }
}
