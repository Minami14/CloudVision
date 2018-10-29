# CloudVision

## How to use

```csharp
using Minami.CloudVision;
using System;

namespace Sample
{
	class Program
	{
		static void Main(string[] args)
		{
			var apiKey = "YOUR API KEY";
			var cloudVisionClient = new CloudVisionClient(apiKey);
			
			string[] filePaths = new string[] { "sample.png" };
			
			var textAnnotations = cloudVisionClient.DetectText(filePaths);
			var text = textAnnotations[0].Text;
			var lang = textAnnotations[0].Language;
			Console.WriteLine("language = {0}", lang);
			Console.WriteLine(text);
      
			var labelAnnotations = cloudVisionClient.DetectLabels(filePaths);
			foreach (var label in labelAnnotations[0].ScoreTable)
			{
				var description = label.Key;
				var score = label.Value;
				Console.WriteLine("{0} : {1}", description, score);
			}
			
			Console.ReadLine();
		}
	}
}
```
