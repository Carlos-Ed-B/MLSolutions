using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.ML;
using Microsoft.ML.Data;
using static Microsoft.ML.DataOperationsCatalog;

namespace ML.Infrastructure
{
    public static class MachineLearningHelper
    {
        public static void SaveModel(MLContext mlContext, ITransformer mlModel, string modelRelativePath, DataViewSchema modelInputSchema)
        {
            Console.WriteLine($"=============== Saving the model  =========================");
            mlContext.Model.Save(mlModel, modelInputSchema, FileHelper.GetAbsolutePath(modelRelativePath));
            Console.WriteLine("     The model saved on {0}", FileHelper.GetAbsolutePath(modelRelativePath));
            Console.WriteLine($"===========================================================");
        }

        public static void PrintCrossValidating(MLContext mlContext, IDataView trainingDataView, IEstimator<ITransformer> trainingPipeline, string labelColumnName)
        {
            Console.WriteLine("=============== Cross-validating to get accuracy metrics ===============");
            var crossValidationResults = mlContext.MulticlassClassification.CrossValidate(trainingDataView, trainingPipeline, numberOfFolds: 5, labelColumnName: labelColumnName);
            MachineLearningHelper.PrintMulticlassClassificationFoldsAverageMetrics(crossValidationResults);
            Console.WriteLine("=========================================================================");
        }

        public static void PrintClassificationMetrics(MLContext mlContext, TrainTestData splitData, ITransformer trainedModel, string labelColumnName, string scoreColumnName)
        {
            var predictions = trainedModel.Transform(splitData.TestSet);
            var metrics = mlContext.MulticlassClassification.Evaluate(predictions, labelColumnName, scoreColumnName);
            MachineLearningHelper.PrintMulticlassClassificationMetrics(metrics);
        }

        public static void PrintMulticlassClassificationMetrics(MulticlassClassificationMetrics metrics)
        {
            Console.WriteLine($"************************************************************");
            Console.WriteLine($"*   Metrics for multi-class classification model   ");
            Console.WriteLine($"*-----------------------------------------------------------");
            Console.WriteLine($"*    MacroAccuracy = {metrics.MacroAccuracy:0.####}, a value between 0 ~ 1, closer to 1 is better");
            Console.WriteLine($"*    MicroAccuracy = {metrics.MicroAccuracy:0.####}, a value between 0 ~ 1, closer to 1 is better");
            Console.WriteLine($"*    LogLoss = {metrics.LogLoss:0.####}, the closer to 0 is better");
            Console.WriteLine($"*-----------------------------------------------------------");

            for (int i = 0; i < metrics.PerClassLogLoss.Count(); i++)
            {
                Console.WriteLine($"    LogLoss for class {i + 1} = {metrics.PerClassLogLoss[i]:0.####}, closer to 0 is better");
            }

            Console.WriteLine($"************************************************************");
        }

        public static List<string> GetSlotNames(DataViewSchema schema, string name)
        {
            var column = schema.GetColumnOrNull(name);

            var slotNames = new VBuffer<ReadOnlyMemory<char>>();
            column.Value.GetSlotNames(ref slotNames);
            var names = new string[slotNames.Length];
            var num = 0;
            foreach (var denseValue in slotNames.DenseValues())
            {
                names[num++] = denseValue.ToString();
            }

            return names.ToList();
        }

        public static Dictionary<string, float> GetScoresWithLabelsSorted(DataViewSchema schema, string scoreName, float[] scores, string labelName = "", bool writeLog = true)
        {
            Dictionary<string, float> scoreEntries = new Dictionary<string, float>();

            var column = schema.GetColumnOrNull(scoreName);

            var slotNames = new VBuffer<ReadOnlyMemory<char>>();
            column.Value.GetSlotNames(ref slotNames);
            var names = new string[slotNames.Length];
            var num = 0;

            foreach (var denseValue in slotNames.DenseValues())
            {
                scoreEntries.Add(denseValue.ToString(), scores[num++]);
            }

            scoreEntries = scoreEntries.OrderByDescending(c => c.Value).ToDictionary(i => i.Key, i => i.Value);

            if (writeLog)
            {
                Console.WriteLine($"=============== Scores Labels Init  ===============");
                foreach (var scoreEntry in scoreEntries)
                {
                    Console.WriteLine($"    {labelName}: {scoreEntry.Key} Score: {scoreEntry.Value * 100}%");
                }
                Console.WriteLine($"=============== Scores Labels End  ===============");
            }

            return scoreEntries;
        }

        public static void PrintMulticlassClassificationFoldsAverageMetrics(IEnumerable<TrainCatalogBase.CrossValidationResult<MulticlassClassificationMetrics>> crossValResults)
        {
            var metricsInMultipleFolds = crossValResults.Select(r => r.Metrics);

            var microAccuracyValues = metricsInMultipleFolds.Select(m => m.MicroAccuracy);
            var microAccuracyAverage = microAccuracyValues.Average();
            var microAccuraciesStdDeviation = CalculateStandardDeviation(microAccuracyValues);
            var microAccuraciesConfidenceInterval95 = CalculateConfidenceInterval95(microAccuracyValues);

            var macroAccuracyValues = metricsInMultipleFolds.Select(m => m.MacroAccuracy);
            var macroAccuracyAverage = macroAccuracyValues.Average();
            var macroAccuraciesStdDeviation = CalculateStandardDeviation(macroAccuracyValues);
            var macroAccuraciesConfidenceInterval95 = CalculateConfidenceInterval95(macroAccuracyValues);

            var logLossValues = metricsInMultipleFolds.Select(m => m.LogLoss);
            var logLossAverage = logLossValues.Average();
            var logLossStdDeviation = CalculateStandardDeviation(logLossValues);
            var logLossConfidenceInterval95 = CalculateConfidenceInterval95(logLossValues);

            var logLossReductionValues = metricsInMultipleFolds.Select(m => m.LogLossReduction);
            var logLossReductionAverage = logLossReductionValues.Average();
            var logLossReductionStdDeviation = CalculateStandardDeviation(logLossReductionValues);
            var logLossReductionConfidenceInterval95 = CalculateConfidenceInterval95(logLossReductionValues);

            Console.WriteLine($"*************************************************************************************************************");
            Console.WriteLine($"*       Metrics for Multi-class Classification model      ");
            Console.WriteLine($"*------------------------------------------------------------------------------------------------------------");
            Console.WriteLine($"*       Average MicroAccuracy:    {microAccuracyAverage:0.###}  - Standard deviation: ({microAccuraciesStdDeviation:#.###})  - Confidence Interval 95%: ({microAccuraciesConfidenceInterval95:#.###})");
            Console.WriteLine($"*       Average MacroAccuracy:    {macroAccuracyAverage:0.###}  - Standard deviation: ({macroAccuraciesStdDeviation:#.###})  - Confidence Interval 95%: ({macroAccuraciesConfidenceInterval95:#.###})");
            Console.WriteLine($"*       Average LogLoss:          {logLossAverage:#.###}  - Standard deviation: ({logLossStdDeviation:#.###})  - Confidence Interval 95%: ({logLossConfidenceInterval95:#.###})");
            Console.WriteLine($"*       Average LogLossReduction: {logLossReductionAverage:#.###}  - Standard deviation: ({logLossReductionStdDeviation:#.###})  - Confidence Interval 95%: ({logLossReductionConfidenceInterval95:#.###})");
            Console.WriteLine($"*************************************************************************************************************");

        }

        public static double CalculateStandardDeviation(IEnumerable<double> values)
        {
            double average = values.Average();
            double sumOfSquaresOfDifferences = values.Select(val => (val - average) * (val - average)).Sum();
            double standardDeviation = Math.Sqrt(sumOfSquaresOfDifferences / (values.Count() - 1));
            return standardDeviation;
        }

        public static double CalculateConfidenceInterval95(IEnumerable<double> values)
        {
            double confidenceInterval95 = 1.96 * CalculateStandardDeviation(values) / Math.Sqrt((values.Count() - 1));
            return confidenceInterval95;
        }

        #region Train

        public static ITransformer TrainModel(IDataView trainingDataView, IEstimator<ITransformer> trainingPipeline)
        {
            var watch = Stopwatch.StartNew();
            Console.WriteLine("=============== Training  model ===============");
            ITransformer model = trainingPipeline.Fit(trainingDataView);
            watch.Stop();
            Console.WriteLine("     Time:" + watch.ElapsedMilliseconds / 1000 + " seconds");
            Console.WriteLine("===============================================");
            return model;
        }

        public static ITransformer TrainModel(TrainTestData trainingDataView, IEstimator<ITransformer> trainingPipeline)
        {
            var watch = Stopwatch.StartNew();
            Console.WriteLine("=============== Training model test ========================");
            ITransformer model = trainingPipeline.Fit(trainingDataView.TrainSet);
            watch.Stop();
            Console.WriteLine("*     Time:" + watch.ElapsedMilliseconds / 1000 + " seconds");
            Console.WriteLine("=============================================================");
            return model;
        }

        #endregion
    }
}
