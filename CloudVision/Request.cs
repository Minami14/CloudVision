using Newtonsoft.Json;
using System;
using System.IO;

namespace Minami
{
    namespace CloudVision
    {
        public class Request
        {
            public Requests[] requests { get; }

            public Request(string[] path, string[][] type)
            {
                requests = new Requests[path.Length];
                for (var i = 0; i < path.Length; i++)
                {
                    requests[i] = new Requests(path[i], type[i]);
                }
            }

            public class Requests
            {
                public Image image { get; }
                public Features[] features { get; }

                public Requests(string path, string[] type)
                {
                    image = new Image(path);
                    features = new Features[type.Length];
                    for (var i = 0; i < type.Length; i++)
                    {
                        features[i] = new Features(type[i]);
                    }
                }
            }

            public class Image
            {
                public string content { get; }

                public Image(string path)
                {
                    content = ImageToBase64(path);
                }
            }

            public class Features
            {
                public string type { get; }

                public Features(string type)
                {
                    this.type = type;
                }
            }

            private static string ImageToBase64(string path)
            {
                var result = "";
                var countLimit = 10;
                for (var i = 0; i < countLimit; i++)
                {
                    try
                    {
                        var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                        var bs = new byte[fs.Length];
                        var readBytes = fs.Read(bs, 0, (int)fs.Length);
                        fs.Close();
                        result = Convert.ToBase64String(bs);
                        break;
                    }
                    catch
                    {
                        System.Threading.Thread.Sleep(500);
                    }
                }
                return result;
            }

            public override string ToString()
            {
                return JsonConvert.SerializeObject(this, Formatting.Indented);
            }
        }
    }
}