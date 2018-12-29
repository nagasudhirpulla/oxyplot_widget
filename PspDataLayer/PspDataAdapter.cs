using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Mono.Web;
using PspDataLayer.Config;
using Newtonsoft.Json;
using System.Collections.Specialized;

namespace PspDataLayer
{
    public class PspDataAdapter
    {
        public ConfigurationManagerJSON ConfigurationManager { get; set; } = new ConfigurationManagerJSON();

        public async Task<Dictionary<string, List<DataPoint>>> GetDataAsync(DateTime startTime, DateTime endTime, string measLabel)
        {
            // Initialize the results
            Dictionary<string, List<DataPoint>> results = new Dictionary<string, List<DataPoint>>();
            results[measLabel] = new List<DataPoint>();

            // Do a get request to get the data from the api
            var builder = new UriBuilder(ConfigurationManager.Host)
            {
                Port = ConfigurationManager.Port,
                Path = ConfigurationManager.Path
            };
            NameValueCollection query = HttpUtility.ParseQueryString(builder.Query);
            query["label"] = measLabel;
            query["from_time"] = startTime.ToString("yyyyMMdd");
            query["to_time"] = endTime.ToString("yyyyMMdd");
            builder.Query = query.ToString();
            string url = builder.ToString();
            HttpClient httpClient = new HttpClient();
            try
            {
                string content = await httpClient.GetStringAsync(url);

                // Parse the api result content to get the results
                TableRowsApiResultModel apiResult = await Task.Run(() => JsonConvert.DeserializeObject<TableRowsApiResultModel>(content));

                // Construct the desired result from the api result
                // Find the time label in the result columns
                int timeColIndex = apiResult.TableColNames.IndexOf("DATE_KEY");
                int measColIndex = 1;

                // check if both columns are present in the results
                if (timeColIndex > -1 && apiResult.TableColNames.Count > 1)
                {
                    for (int apiRowIter = 0; apiRowIter < apiResult.TableRows.Count; apiRowIter++)
                    {
                        DateTime resTime = DateTime.ParseExact(apiResult.TableRows[apiRowIter][timeColIndex].ToString(), "yyyyMMdd", null);
                        double resValue = (double)apiResult.TableRows[apiRowIter][measColIndex];
                        results[measLabel].Add(new DataPoint { Time = resTime, Value = resValue });
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error occured while fetching data from psp api\n{e.Message}");
            }
            return results;
        }

        public async Task<List<PspLabelApiItem>> GetMeasurementLabelsAsync()
        {
            // Initialize the results
            List<PspLabelApiItem> results = new List<PspLabelApiItem>();
            
            // Do a get request to get the data from the api
            var builder = new UriBuilder(ConfigurationManager.Host)
            {
                Port = ConfigurationManager.Port,
                Path = ConfigurationManager.LabelsPath
            };
            NameValueCollection query = HttpUtility.ParseQueryString(builder.Query);
            builder.Query = query.ToString();
            string url = builder.ToString();
            HttpClient httpClient = new HttpClient();
            try
            {
                string content = await httpClient.GetStringAsync(url);

                // Parse the api result content to get the results
                results = await Task.Run(() => JsonConvert.DeserializeObject<List<PspLabelApiItem>>(content));
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error occured while fetching labels data from psp api\n{e.Message}");
            }
            return results;
        }
    }

    public class DataPoint
    {
        public DateTime Time { get; set; }
        public double Value { get; set; }
    }

    public class TableRowsApiResultModel
    {
        public List<string> TableColNames { get; set; } = new List<string>();
        public List<string> TableColTypes { get; set; } = new List<string>();
        public List<List<object>> TableRows { get; set; } = new List<List<object>>();
    }

    public class PspLabelApiItem
    {
        public string Label { get; set; }
        public int Id { get; set; }
    }
}

/*
     Using Uri builder to build query strings  - https://stackoverflow.com/questions/17096201/build-query-string-for-system-net-httpclient-get
     Using HttpClient to do http get requests - https://blog.jayway.com/2012/03/13/httpclient-makes-get-and-post-very-simple/
     
     http://localhost:7001/api/psp?label=gujarat_thermal_mu&from_time=20181201&to_time=20181205
     {"tableColNames":["DATE_KEY","THERMAL"],"tableColTypes":["Decimal","Decimal"],"tableRows":[[20181201.0,180.30],[20181202.0,186.70],[20181203.0,192.6890],[20181204.0,191.0920],[20181205.0,191.30]]}
*/
