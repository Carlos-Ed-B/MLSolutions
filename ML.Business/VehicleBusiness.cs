using CsvHelper;
using ML.Modes;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace ML.Business
{
    public class VehicleBusiness
    {
        public static List<VehicleModel> LoadDataSource()
        {
            using (var reader = new StreamReader("DataSource\\tipoVeiculo_marca.csv"))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var result = csv.GetRecords<VehicleModel>();

                return result.ToList();
            }
        }

        public static List<string> ConsultBrand(string vehicleType)
        {
            var result = VehicleBusiness.LoadDataSource().Where(x => x.TipoVeiculo.ToLower().Equals(vehicleType.ToLower())).Select(s => s.Marca).OrderBy(o => o);

            return result.Distinct().ToList();
        }
    }
}
