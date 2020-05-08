using ML.Trainings.HighwayBrazilAccidents;
using Newtonsoft.Json;
using System;
using Xunit;

namespace ML.Test
{
    public class RodoviaTrainningTest : BaseTest
    {
        [Fact]
        public void DoTrain()
        {
            var training = new HighwayBrazilAccidentsTrainning();
            var result = training.DoTrain();

            Assert.True(result);
        }

        [Fact]
        public void DoPredict()
        {
            var training = new HighwayBrazilAccidentsTrainning();
            training.DoLoadModel();

            var dataList = training.CreateDataSample();

            foreach (var data in dataList)
            {
                string json = JsonConvert.SerializeObject(data);

                var result = training.DoPredict(data);
                Assert.True(result.Prediction.Equals(data.Tipo_acidente));
            }

        }
    }
}
