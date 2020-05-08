using Microsoft.ML;
using ML.Infrastructure;
using ML.Infrastructure.VOs;
using ML.Modes.HighwayBrazilAccidents;
using ML.Trainings.VOs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ML.Trainings.HighwayBrazilAccidents
{
    public class HighwayBrazilAccidentsTrainning
    {
        private const string _basePath = @"HighwayBrazilAccidents\DataSource";
        private readonly string _dataSourcePath = Path.Combine(Environment.CurrentDirectory, _basePath, "HighwayBrazilAccidents.csv");
        private readonly string _trainedModelPath = Path.Combine(Environment.CurrentDirectory, _basePath, "RodoviaModelo.zip");
        private readonly MLContext _mlContext = new MLContext(seed: EnviromentVO.MachineLearningSeedDefault);

        public bool DoTrain()
        {
            Console.WriteLine("=============== Init HighwayBrazilAccidents training =========");

            if (!File.Exists(this._dataSourcePath))
            {
                Console.WriteLine("     Dataset not found");
                Console.WriteLine("=============== Not done training process ================");
                return false;
            }

            /// 1- Carga de dados
            var dataView = this.LoadDataSource();

            /// 1.1 - Separacao de treino e teste
            var splitData = _mlContext.Data.TrainTestSplit(dataView, testFraction: 0.3);

            /// 2 - Transformação dos dados
            var trainingPipeline = this.BuildTrainingPipeline();

            /// 3 Treino
            var trainedModel = MachineLearningHelper.TrainModel(splitData, trainingPipeline);

            /// 4 - Avaliação do modelo
            MachineLearningHelper.PrintClassificationMetrics(this._mlContext, splitData, trainedModel, "tipo_acidente", DefaultColumnNames.Score);

            /// 5 - Serializando o modelo (salvando o modelo em um arquivo)
            MachineLearningHelper.SaveModel(this._mlContext, trainedModel, _trainedModelPath, dataView.Schema);

            return true;
        }

        private IDataView LoadDataSource()
        {
            return _mlContext.Data.LoadFromTextFile<HighwayBrazilAccidentsData>(
                                            path: _dataSourcePath,
                                            hasHeader: true,
                                            separatorChar: ';',
                                            allowQuoting: true,
                                            allowSparse: false);
        }

        public IEstimator<ITransformer> BuildTrainingPipeline()
        {

            var dataProcessPipeline = this._mlContext.Transforms.Conversion.MapValueToKey("tipo_acidente", "tipo_acidente")
                                      .Append(this._mlContext.Transforms.Categorical.OneHotEncoding(new[] { new InputOutputColumnPair("uf", "uf"), new InputOutputColumnPair("causa_acidente", "causa_acidente"), new InputOutputColumnPair("tipo_veiculo", "tipo_veiculo"), new InputOutputColumnPair("tipo_envolvido", "tipo_envolvido") }))
                                      .Append(this._mlContext.Transforms.Categorical.OneHotHashEncoding(new[] { new InputOutputColumnPair("municipio", "municipio"), new InputOutputColumnPair("marca", "marca") }))
                                      .Append(this._mlContext.Transforms.Concatenate("Features", new[] { "uf", "causa_acidente", "tipo_veiculo", "tipo_envolvido", "municipio", "marca", "br", "km" }));

            var trainer = this._mlContext.MulticlassClassification.Trainers.LightGbm(labelColumnName: "tipo_acidente", featureColumnName: "Features")
                                      .Append(this._mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel", "PredictedLabel"));


            var trainingPipeline = dataProcessPipeline.Append(trainer);

            return trainingPipeline;
        }

        public PredictionEngine<HighwayBrazilAccidentsData, HighwayBrazilAccidentsPrediction> DoLoadModel()
        {
            ITransformer mlModel = this._mlContext.Model.Load(_trainedModelPath, out var modelInputSchema);
            var predEngine = this._mlContext.Model.CreatePredictionEngine<HighwayBrazilAccidentsData, HighwayBrazilAccidentsPrediction>(mlModel);

            return predEngine;
        }

        public HighwayBrazilAccidentsPrediction DoPredict(HighwayBrazilAccidentsData data)
        {
            PredictionEngine<HighwayBrazilAccidentsData, HighwayBrazilAccidentsPrediction> predEngine = this.DoLoadModel();

            HighwayBrazilAccidentsPrediction result = predEngine.Predict(data);

            Console.WriteLine("============ Rodovia Predict ==============");
            Console.WriteLine($"    Class: {result.Prediction}");
            Console.WriteLine("============================================");

            return result;
        }

        public HighwayBrazilAccidentsData CreateSingleDataSample()
        {
            var dataView = this.LoadDataSource();
            HighwayBrazilAccidentsData sampleForPrediction = this._mlContext.Data.CreateEnumerable<HighwayBrazilAccidentsData>(dataView, false).FirstOrDefault();

            return sampleForPrediction;
        }

        public List<HighwayBrazilAccidentsData> CreateDataSample(int size = 6)
        {
            var dataView = this.LoadDataSource();
            var list = this._mlContext.Data.CreateEnumerable<HighwayBrazilAccidentsData>(dataView, false).ToList();

            var result = new List<HighwayBrazilAccidentsData>();

            Random random = new Random();

            for (int count = size; count > 0; count--)
            {
                int index = random.Next(list.Count - 1);
                var temp = list[index];
                result.Add(temp);
            }

            return result;
        }

    }
}
