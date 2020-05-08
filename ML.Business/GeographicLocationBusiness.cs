using CsvHelper;
using ML.Modes;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace ML.Business
{
    public class GeographicLocationBusiness
    {
        public static List<GeographicLocationBrazilModel> LoadDataSource()
        {
            using (var reader = new StreamReader("DataSource\\uf_cidade.csv"))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var result = csv.GetRecords<GeographicLocationBrazilModel>();

                return result.ToList();
            }
        }

        public static List<GeographicLocationBrazilKMModel> LoadDataSourceKM()
        {
            using (var reader = new StreamReader("DataSource\\km_cidade.csv"))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var result = csv.GetRecords<GeographicLocationBrazilKMModel>();

                return result.ToList();
            }
        }

        public static List<string> ConsultUF()
        {
            var result = GeographicLocationBusiness.LoadDataSource().Select(x => x.UF).OrderBy(o => o);

            return result.Distinct().ToList();
        }

        public static List<string> ConsultMunicipio(string uf)
        {
            var result = GeographicLocationBusiness.LoadDataSource().Where(x => x.UF.ToLower().Equals(uf.ToLower())).Select(s => s.Municipio).OrderBy(o => o);

            return result.Distinct().ToList();
        }

        public static List<string> ConsultKM(string municipio)
        {
            var result = GeographicLocationBusiness.LoadDataSourceKM().Where(x => x.Municipio.ToLower().Equals(municipio.ToLower())).Select(s => s.KM).OrderBy(o => o);

            return result.Distinct().ToList();
        }
    }
}