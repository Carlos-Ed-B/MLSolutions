using Microsoft.ML;
using ML.Infrastructure;
using ML.Infrastructure.VOs;
using ML.Modes.Iris;
using ML.Modes.Iris.Enums;
using ML.Trainings.VOs;
using System;
using System.IO;

namespace ML.Trainings.Iris
{
    public class IrisTraining
    {
        private const string _basePath = @"Iris\DataSource";
        private readonly string _dataSourcePath = Path.Combine(Environment.CurrentDirectory, _basePath, "iris-full.txt");
        private readonly string _trainedModelPath = Path.Combine(Environment.CurrentDirectory, _basePath, "Modelo.zip");
        private readonly MLContext _mlContext = new MLContext(seed: EnviromentVO.MachineLearningSeedDefault);

        public bool DoTrain()
        {
            Console.WriteLine("=============== Init all Iris training process ===============");

            if (!File.Exists(this._dataSourcePath))
            {
                Console.WriteLine("     Dataset not found");
                Console.WriteLine("=============== Not done training process ===============");
                return false;
            }

            /// 1- Carga de dados
            var dataView = this.LoadDataSource();

            /// 1.1 - Separacao de treino e teste
            var splitData = _mlContext.Data.TrainTestSplit(dataView, testFraction: 0.25);

            /// 2 - Transformação dos dados
            var trainingPipeline = this.BuildTrainingPipeline();

            /// 3 Treino
            var trainedModel = MachineLearningHelper.TrainModel(splitData, trainingPipeline);

            /// 4 - Avaliação do modelo
            MachineLearningHelper.PrintClassificationMetrics(this._mlContext, splitData, trainedModel, DefaultColumnNames.Label, DefaultColumnNames.Score);
            MachineLearningHelper.PrintCrossValidating(this._mlContext, dataView, trainingPipeline, DefaultColumnNames.Label);

            /// 5 - Serializando o modelo (salvando o modelo em um arquivo)
            MachineLearningHelper.SaveModel(this._mlContext, trainedModel, _trainedModelPath, dataView.Schema);

            Console.WriteLine("=============== End all training process ===============");

            return true;
        }

        private IDataView LoadDataSource()
        {
            return this._mlContext.Data.LoadFromTextFile<IrisData>(this._dataSourcePath, hasHeader: true);
        }

        private IEstimator<ITransformer> BuildTrainingPipeline()
        {
            var dataProcessPipeline = this._mlContext.Transforms.Conversion.MapValueToKey("Label", "Label")
                                      .Append(_mlContext.Transforms.Concatenate("Features", new[] { "Sepal length", "Sepal width", "Petal length", "Petal width" }))
                                      .Append(_mlContext.Transforms.NormalizeMinMax("Features", "Features"))
                                      .AppendCacheCheckpoint(_mlContext);

            var trainer = this._mlContext.MulticlassClassification.Trainers.SdcaMaximumEntropy(labelColumnName: "Label", featureColumnName: "Features")
                                .Append(this._mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel", "PredictedLabel"));

            var trainingPipeline = dataProcessPipeline.Append(trainer);

            return trainingPipeline;
        }

        public IrisPrediction DoPredict(IrisData irisData)
        {
            PredictionEngine<IrisData, IrisPrediction> predEngine = this.DoLoadModel();

            IrisPrediction result = predEngine.Predict(irisData);

            //MachineLearningHelper.GetScoresWithLabelsSorted(predEngine.OutputSchema, DefaultColumnNames.Score, result.Score, "Petala");

            result.Type = (IrisTypeEnum)result.Prediction;

            Console.WriteLine("=============== Iris Predict ===============");
            Console.WriteLine($"    Iris: {result.Prediction} => {result.Type}");
            Console.WriteLine("    Prob. Classe 0: {0:0.###}" + result.Score[0]);
            Console.WriteLine("    Prob. Classe 1: {0:0.###}" + result.Score[1]);
            Console.WriteLine("    Prob. Classe 2: {0:0.###}" + result.Score[2]);
            Console.WriteLine("============================================");

            return result;
        }

        public PredictionEngine<IrisData, IrisPrediction> DoLoadModel()
        {
            ITransformer mlModel = this._mlContext.Model.Load(_trainedModelPath, out var modelInputSchema);
            var predEngine = this._mlContext.Model.CreatePredictionEngine<IrisData, IrisPrediction>(mlModel);

            return predEngine;
        }
    }
}
