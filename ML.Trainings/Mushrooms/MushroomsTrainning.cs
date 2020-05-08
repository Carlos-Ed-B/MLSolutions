using Microsoft.ML;
using ML.Infrastructure;
using ML.Infrastructure.VOs;
using ML.Modes.Mushrooms;
using ML.Trainings.VOs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ML.Trainings.Mushrooms
{
    public class MushroomsTrainning
    {
        private const string _basePath = @"Mushrooms\DataSource";
        private readonly string _dataSourcePath = Path.Combine(Environment.CurrentDirectory, _basePath, "mushrooms.csv");
        private readonly string _trainedModelPath = Path.Combine(Environment.CurrentDirectory, _basePath, "MushroomsModelo.zip");
        private readonly MLContext _mlContext = new MLContext(seed: EnviromentVO.MachineLearningSeedDefault);

        public bool DoTrain()
        {
            Console.WriteLine("=============== Init all Mushrooms training process =========");

            if (!File.Exists(this._dataSourcePath))
            {
                Console.WriteLine("     Dataset not found");
                Console.WriteLine("=============== Not done training process ===============");
                return false;
            }

            /// 1- Carga de dados
            var dataView = this.LoadDataSource();

            /// 1.1 - Separacao de treino e teste
            var splitData = _mlContext.Data.TrainTestSplit(dataView, testFraction: 0.5);

            /// 2 - Transformação dos dados
            var trainingPipeline = this.BuildTrainingPipeline();

            /// 3 Treino
            var trainedModel = MachineLearningHelper.TrainModel(splitData, trainingPipeline);

            /// 4 - Avaliação do modelo
            MachineLearningHelper.PrintClassificationMetrics(this._mlContext, splitData, trainedModel, "class", DefaultColumnNames.Score);

            /// 5 - Serializando o modelo (salvando o modelo em um arquivo)
            MachineLearningHelper.SaveModel(this._mlContext, trainedModel, _trainedModelPath, dataView.Schema);

            return true;
        }

        private IDataView LoadDataSource()
        {
            return this._mlContext.Data.LoadFromTextFile<MushroomsData>(
                                path: _dataSourcePath,
                                hasHeader: true,
                                separatorChar: ',',
                                allowQuoting: true,
                                allowSparse: false);
        }

        public IEstimator<ITransformer> BuildTrainingPipeline()
        {           
            var dataProcessPipeline = this._mlContext.Transforms.Conversion.MapValueToKey("class", "class")
                                      .Append(this._mlContext.Transforms.Categorical.OneHotEncoding(new[] { new InputOutputColumnPair("bruises", "bruises"), 
                                          new InputOutputColumnPair("cap-shape", "cap-shape"), 
                                          new InputOutputColumnPair("cap-surface", "cap-surface"), 
                                          new InputOutputColumnPair("cap-color", "cap-color"), 
                                          new InputOutputColumnPair("odor", "odor"), 
                                          new InputOutputColumnPair("gill-spacing", "gill-spacing"), 
                                          new InputOutputColumnPair("gill-size", "gill-size"), 
                                          new InputOutputColumnPair("gill-color", "gill-color"), 
                                          new InputOutputColumnPair("stalk-root", "stalk-root"), 
                                          new InputOutputColumnPair("stalk-surface-above-ring", "stalk-surface-above-ring"), 
                                          new InputOutputColumnPair("stalk-surface-below-ring", "stalk-surface-below-ring"), 
                                          new InputOutputColumnPair("stalk-color-above-ring", "stalk-color-above-ring"), 
                                          new InputOutputColumnPair("stalk-color-below-ring", "stalk-color-below-ring"), 
                                          new InputOutputColumnPair("veil-color", "veil-color"), 
                                          new InputOutputColumnPair("ring-number", "ring-number"), 
                                          new InputOutputColumnPair("ring-type", "ring-type"), 
                                          new InputOutputColumnPair("spore-print-color", "spore-print-color"), 
                                          new InputOutputColumnPair("population", "population"), 
                                          new InputOutputColumnPair("habitat", "habitat") }))
                                      .Append(this._mlContext.Transforms.Concatenate("Features", new[] { "bruises", "cap-shape", "cap-surface", "cap-color", "odor",                    "gill-spacing", "gill-size", "gill-color",                "stalk-root", "stalk-surface-above-ring", "stalk-surface-below-ring", "stalk-color-above-ring", "stalk-color-below-ring",              "veil-color", "ring-number", "ring-type", "spore-print-color", "population", "habitat" }))
                                      .Append(this._mlContext.Transforms.NormalizeMinMax("Features", "Features"))
                                      .AppendCacheCheckpoint(this._mlContext);

            var trainer = this._mlContext.MulticlassClassification.Trainers.OneVersusAll(this._mlContext.BinaryClassification.Trainers.AveragedPerceptron(labelColumnName: "class", numberOfIterations: 10, featureColumnName: "Features"), labelColumnName: "class")
                                      .Append(this._mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel", "PredictedLabel"));

            var trainingPipeline = dataProcessPipeline.Append(trainer);

            return trainingPipeline;
        }

        public PredictionEngine<MushroomsData, MushroomsPrediction> DoLoadModel()
        {
            ITransformer mlModel = this._mlContext.Model.Load(_trainedModelPath, out var modelInputSchema);
            var predEngine = this._mlContext.Model.CreatePredictionEngine<MushroomsData, MushroomsPrediction>(mlModel);

            return predEngine;
        }

        public MushroomsPrediction DoPredict(MushroomsData data)
        {
            PredictionEngine<MushroomsData, MushroomsPrediction> predEngine = this.DoLoadModel();

            MushroomsPrediction result = predEngine.Predict(data);

            Console.WriteLine("============ Mushrooms Predict =============");
            Console.WriteLine($"    Class: {result.Prediction}");
            Console.WriteLine($"    Prob. Classe 0: {result.Score[0]:0.###}");
            Console.WriteLine($"    Prob. Classe 1: {result.Score[1]:0.###}");
            Console.WriteLine("============================================");

            result.Description = "Cogumelo não identificado.";

            if (result.Prediction.Equals("e"))
            {
                result.Description = "Cogumelo comestível.";
            }

            if (result.Prediction.Equals("p"))
            {
                result.Description = "Cogumelo venenoso.";
            }

            return result;
        }

        public MushroomsData CreateSingleDataSample()
        {
            var dataView = this.LoadDataSource();
            MushroomsData sampleForPrediction = this._mlContext.Data.CreateEnumerable<MushroomsData>(dataView, false).FirstOrDefault();

            return sampleForPrediction;
        }

        public List<MushroomsData> CreateDataSample(int size = 6)
        {
            var dataView = this.LoadDataSource();
            var list = this._mlContext.Data.CreateEnumerable<MushroomsData>(dataView, false).ToList();

            var result = new List<MushroomsData>();

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
