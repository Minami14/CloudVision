using Newtonsoft.Json;
using System.Collections.Generic;

namespace Minami
{
    namespace CloudVision
    {
        public class LabelAnnotation
        {
            public string[] Descriptions { get; }
            public Dictionary<string, double> ScoreTable { get; }

            public LabelAnnotation(string[] descriptions, double[] scores)
            {
                Descriptions = descriptions;
                ScoreTable = new Dictionary<string, double>();

                for (var i = 0; i < descriptions.Length; i++)
                {
                    ScoreTable.Add(descriptions[i], scores[i]);
                }
            }

            public override string ToString()
            {
                return JsonConvert.SerializeObject(this, Formatting.Indented);
            }
        }
    }
}