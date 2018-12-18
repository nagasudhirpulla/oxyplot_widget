using OxyPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Helpers
{
    class FetchHelper
    {
        public static async Task<List<DataPoint>> FetchData(DateTime fromTime, DateTime toTime, TimeSpan MaxFetchSize, Func<DateTime, DateTime, Task<List<DataPoint>>> FetchDataNonBatch)
        {
            List<DataPoint> dataPoints = new List<DataPoint>();
            DateTime fetchStartTime = fromTime;
            DateTime fetchEndTime = fromTime;
            do
            {
                // derive fetch start and fetch end times
                fetchStartTime = fetchEndTime;
                fetchEndTime = fetchStartTime + MaxFetchSize;

                if (fetchStartTime.Equals(fetchEndTime))
                {
                    // When batch interval is zero, we will get data in a single fetch
                    fetchEndTime = toTime;
                }
                if (fetchEndTime > toTime)
                {
                    // Do not fetch above toTime
                    fetchEndTime = toTime;
                }

                // get the data batch
                List<DataPoint> tempDataPoints = await FetchDataNonBatch(fetchStartTime, fetchEndTime);

                // if this iteration is not the first iteration, remove the first sample from this data point list, since it was the last sample of the previous data point list
                if (fetchStartTime > fromTime)
                {
                    tempDataPoints.RemoveAt(0);
                }

                // add the batch result to the final result
                dataPoints.AddRange(tempDataPoints);
            } while (fetchEndTime < toTime);
            return dataPoints;
        }

        public static List<DataPoint> GetDataPointsWithGivenMaxSampleInterval(List<DataPoint> pnts, TimeSpan maxRes, SamplingStrategy samplingStrategy)
        {
            if (samplingStrategy == SamplingStrategy.Raw || maxRes.TotalDays == 0 || pnts.Count == 0)
            {
                return pnts;
            }

            List<DataPoint> dataPoints = new List<DataPoint>();

            // get max sample interval as numeric
            double maxResNumeric = maxRes.TotalDays;

            double sampleBoundaryStart = pnts[0].X;
            double sampleBoundaryEnd = sampleBoundaryStart + maxResNumeric;
            List<double> sampleBucket = new List<double>();
            for (int pntIter = 0; pntIter < pnts.Count; pntIter++)
            {
                if (pnts[pntIter].X < sampleBoundaryEnd)
                {
                    // Add points to the sample bucket till we encounter the sample boundary
                    sampleBucket.Add(pnts[pntIter].Y);
                }
                else
                {
                    // Add the value to the final list
                    dataPoints.Add(new DataPoint(sampleBoundaryStart, GetBucketAggregate(sampleBucket, samplingStrategy)));

                    // Update the sample Boundaries
                    sampleBoundaryStart = sampleBoundaryEnd;
                    sampleBoundaryEnd += maxResNumeric;

                    // Empty the bucket
                    sampleBucket.Clear();

                    // Add current sample to the new bucket
                    sampleBucket.Add(pnts[pntIter].Y);
                }
            }

            if (sampleBucket.Count > 0)
            {
                // handle the last bucket
                dataPoints.Add(new DataPoint(sampleBoundaryStart, GetBucketAggregate(sampleBucket, samplingStrategy)));
            }

            return dataPoints;
        }

        public static double GetBucketAggregate(List<double> sampleBucket, SamplingStrategy samplingStrategy)
        {
            // Aggregate the sample bucket as per sampling strategy
            double bucketResult = 0;
            if (sampleBucket.Count == 0)
            {
                return double.NaN;
            }
            try
            {
                if (samplingStrategy == SamplingStrategy.Snap)
                {
                    foreach (double sampleVal in sampleBucket)
                    {
                        if (!Double.IsNaN(sampleVal))
                        {
                            bucketResult = sampleVal;
                            return bucketResult;
                        }
                    }
                }
                else if (samplingStrategy == SamplingStrategy.Average || samplingStrategy == SamplingStrategy.Sum)
                {
                    double numValidSamples = 0;
                    foreach (double sampleVal in sampleBucket)
                    {
                        if (!Double.IsNaN(sampleVal))
                        {
                            bucketResult += sampleVal;
                            numValidSamples += 1;
                        }
                    }
                    if (numValidSamples == 0)
                    {
                        // since we did not find a valid sample in the bucket, return NaN
                        return double.NaN;
                    }
                    if (samplingStrategy == SamplingStrategy.Average)
                    {
                        bucketResult = bucketResult / numValidSamples;
                    }
                    return bucketResult;
                }
                else if (samplingStrategy == SamplingStrategy.Maximum)
                {
                    bucketResult = Double.NegativeInfinity;
                    foreach (double sampleVal in sampleBucket)
                    {
                        if (!Double.IsNaN(sampleVal) && sampleVal > bucketResult)
                        {
                            bucketResult = sampleVal;
                        }
                    }
                    return bucketResult;
                }
                else if (samplingStrategy == SamplingStrategy.Minimum)
                {
                    bucketResult = Double.PositiveInfinity;
                    foreach (double sampleVal in sampleBucket)
                    {
                        if (!Double.IsNaN(sampleVal) && sampleVal < bucketResult)
                        {
                            bucketResult = sampleVal;
                        }
                    }
                    return bucketResult;
                }
            }
            catch (Exception)
            {
                // do nothing
            }
            return bucketResult;
        }
    }

    public enum SamplingStrategy
    {
        Average,
        Snap,
        Maximum,
        Minimum,
        Sum,
        Raw
    }
}
