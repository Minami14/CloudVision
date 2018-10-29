using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Net;
using System.Text;

namespace Minami
{
    namespace CloudVision
    {
        public class CloudVisionClient
        {
            private static WebClient webClient;

            public string ApiKey { get; set; }

            public CloudVisionClient(string apiKey)
            {
                webClient = new WebClient();
                webClient.Headers[HttpRequestHeader.ContentType] = "application/json";
                webClient.Headers[HttpRequestHeader.Accept] = "application/json";
                webClient.Encoding = Encoding.UTF8;
                ApiKey = apiKey;
            }

            private JObject PostRequests(string[] path, string[][] type)
            {
                var uri = "https://vision.googleapis.com/v1/images:annotate?key=" + ApiKey;
                var request = new Request(path, type);
                var json = JsonConvert.SerializeObject(request, Formatting.Indented);
                return JObject.Parse(webClient.UploadString(new Uri(uri), json));
            }

            public TextAnnotation[] DetectText(string[] path)
            {
                var type = new string[][] { new string[] { "TEXT_DETECTION" } };
                var response = PostRequests(path, type);
                return response["responses"].Select(s => s["textAnnotations"]?[0])
                                            .Select(s => new TextAnnotation(s?["description"].ToString(), s?["locale"].ToString()))
                                            .ToArray();
            }

            public LabelAnnotation[] DetectLabels(string[] path)
            {
                var type = new string[][] { new string[] { "LABEL_DETECTION" } };
                var response = PostRequests(path, type);
                var responseLength = response["responses"].ToArray().Length;
                string[][] descriptions = new string[responseLength][];
                double[][] scores = new double[responseLength][];
                response["responses"].Select((value, index) => new { value, index })
                                     .ToList()
                                     .ForEach(x =>
                                     {
                                         descriptions[x.index] = x.value["labelAnnotations"].Select(y => y["description"].ToString()).ToArray();
                                         scores[x.index] = x.value["labelAnnotations"].Select(y => y["score"].ToObject<double>()).ToArray();
                                     });
                var labelAnnotations = new LabelAnnotation[responseLength];
                for (var i = 0; i < responseLength; i++)
                {
                    labelAnnotations[i] = new LabelAnnotation(descriptions[i], scores[i]);
                }
                return labelAnnotations;
            }
        }
    }
}