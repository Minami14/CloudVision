using Newtonsoft.Json;

namespace Minami
{
    namespace CloudVision
    {
        public class TextAnnotation
        {
            public string Text { get; }
            public string Language { get; }

            public TextAnnotation(string text, string language)
            {
                Text = text;
                Language = language;
            }

            public override string ToString()
            {
                return JsonConvert.SerializeObject(this, Formatting.Indented);
            }
        }
    }
}