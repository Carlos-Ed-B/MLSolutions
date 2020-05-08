﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace ML.Infrastructure
{
    public static class Statistics
    {
        /// <summary>
        /// Estimates the median value from the sorted data array (ascending).
        /// Approximately median-unbiased regardless of the sample distribution (R8).
        /// </summary>
        public static double Median(this double[] data)
        {
            if (data.Length == 0)
            {
                return double.NaN;
            }

            var k = data.Length / 2;

            return data.Length % 2 == 1
                ? data[k]
                : (data[k - 1] + data[k]) / 2.0;
        }

        /// <summary>
        /// Estimates the first quartile value from the sorted data array (ascending).
        /// Approximately median-unbiased regardless of the sample distribution (R8).
        /// </summary>
        public static double LowerQuartile(this double[] data)
        {
            return Quantile(data, 0.25d);
        }

        /// <summary>
        /// Estimates the third quartile value from the sorted data array (ascending).
        /// Approximately median-unbiased regardless of the sample distribution (R8).
        /// </summary>
        public static double UpperQuartile(this double[] data)
        {
            return Quantile(data, 0.75d);
        }

        /// <summary>
        /// Estimates the tau-th quantile from the sorted data array (ascending).
        /// The tau-th quantile is the data value where the cumulative distribution
        /// function crosses tau.
        /// Approximately median-unbiased regardless of the sample distribution (R8).
        /// </summary>
        /// <param name="data">Sample array, must be sorted ascendingly.</param>
        /// <param name="tau">Quantile selector, between 0.0 and 1.0 (inclusive).</param>
        /// <remarks>
        /// R-8, SciPy-(1/3,1/3):
        /// Linear interpolation of the approximate medians for order statistics.
        /// When tau &lt; (2/3) / (N + 1/3), use x1. When tau &gt;= (N - 1/3) / (N + 1/3), use xN.
        /// </remarks>
        public static double Quantile(double[] data, double tau)
        {
            if (tau < 0d || tau > 1d || data.Length == 0)
            {
                return double.NaN;
            }

            if (tau == 0d || data.Length == 1)
            {
                return data[0];
            }

            if (tau == 1d)
            {
                return data[data.Length - 1];
            }

            double h = (data.Length + 1 / 3d) * tau + 1 / 3d;
            var hf = (int)h;
            return hf < 1 ? data[0]
                : hf >= data.Length ? data[data.Length - 1]
                    : data[hf - 1] + (h - hf) * (data[hf] - data[hf - 1]);
        }

        /// <summary>
        /// Computes the Pearson Product-Moment Correlation coefficient.
        /// </summary>
        /// <param name="dataA">Sample data A.</param>
        /// <param name="dataB">Sample data B.</param>
        /// <returns>The Pearson product-moment correlation coefficient.</returns>
        public static double Pearson(IEnumerable<double> dataA, IEnumerable<double> dataB)
        {
            var n = 0;
            var r = 0.0;

            var meanA = 0d;
            var meanB = 0d;
            var varA = 0d;
            var varB = 0d;

            using (IEnumerator<double> ieA = dataA.GetEnumerator())
            using (IEnumerator<double> ieB = dataB.GetEnumerator())
            {
                while (ieA.MoveNext())
                {
                    if (!ieB.MoveNext())
                    {
                        throw new ArgumentOutOfRangeException(nameof(dataB), "Array too short.");
                    }

                    var currentA = ieA.Current;
                    var currentB = ieB.Current;

                    var deltaA = currentA - meanA;
                    var scaleDeltaA = deltaA / ++n;

                    var deltaB = currentB - meanB;
                    var scaleDeltaB = deltaB / n;

                    meanA += scaleDeltaA;
                    meanB += scaleDeltaB;

                    varA += scaleDeltaA * deltaA * (n - 1);
                    varB += scaleDeltaB * deltaB * (n - 1);
                    r += (deltaA * deltaB * (n - 1)) / n;
                }

                if (ieB.MoveNext())
                {
                    throw new ArgumentOutOfRangeException(nameof(dataA), "Array too short.");
                }
            }

            return r / Math.Sqrt(varA * varB);
        }

        /// <summary>
        /// Compute the correlation coefficient for two arrays.
        /// </summary>
        /// <remarks>
        /// Hat-tip: https://stackoverflow.com/a/17447920/489433
        /// </remarks>
        public static float ComputeCorrellationCoefficent(float[] values1, float[] values2)
        {
            if (values1.Length != values2.Length)
            {
                throw new ArgumentException("values must be the same length");
            }

            var avg1 = values1.Average();
            var avg2 = values2.Average();

            var sum1 = values1.Zip(values2, (x1, y1) => (x1 - avg1) * (y1 - avg2)).Sum();

            var sumSqr1 = values1.Sum(x => Math.Pow(x - avg1, 2.0));
            var sumSqr2 = values2.Sum(y => Math.Pow(y - avg2, 2.0));

            return sum1 / (float)Math.Sqrt(sumSqr1 * sumSqr2);
        }
    }
}
