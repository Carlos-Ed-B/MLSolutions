using ML.Trainings.Mushrooms;
using Newtonsoft.Json;
using Xunit;

namespace ML.Test
{
    public class MushroomsTrainningTest : BaseTest
    {
        [Fact]
        public void DoTrain()
        {
            var mushroomsTraining = new MushroomsTrainning();
            var result = mushroomsTraining.DoTrain();

            Assert.True(result);
        }

        [Fact]
        public void DoPredict()
        {
            var mushroomsTraining = new MushroomsTrainning();
            mushroomsTraining.DoLoadModel();

            var dataList = mushroomsTraining.CreateDataSample();

            foreach (var data in dataList)
            {
                string json = JsonConvert.SerializeObject(data);
                var result = mushroomsTraining.DoPredict(data);
                Assert.True(result.Prediction.Equals(data.Class));
            }

        }
    }
}
