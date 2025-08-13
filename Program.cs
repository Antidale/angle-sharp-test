using System.Net;
using AngleSharp;
using AngleSharp.Dom;

var directory = "./TestFlagsets";

var tFile = new DirectoryInfo(directory).GetFiles().First();
using var file = new StreamReader(tFile.FullName);
var html = await file.ReadToEndAsync();
var config = Configuration.Default.WithDefaultLoader();
var context = new BrowsingContext(config);
var document = await context.OpenAsync(req => req.Content(html));

var title = document.Title;
var seed = document.All.First(x => x.Id == "seed").TextContent;
var version = document.QuerySelector("#version")?.TextContent;
var verificationNotes = document.All.Where(x => x.ClassList.Contains("checksum-tile")).ToList();
var verificationTitles = document.QuerySelectorAll(".checksum-tile").Select(x => x.GetAttribute("title")).ToList();

var head = document.Head;
var style = head?.QuerySelector("style");
style?.Remove();
var linkElement = document.CreateElement("link");

linkElement.SetAttribute(null, "rel", "stylesheet");
linkElement.SetAttribute(null, "href", "https://info.tellah.life/seeds/seed.css");
head?.AppendChild(linkElement);
using var writer = new StreamWriter("test.html");
await writer.WriteAsync(document.ToHtml());


// Console.WriteLine(title);
// Console.WriteLine(seed);
// Console.WriteLine(version);
// Console.WriteLine(verificationNotes.Count);
// verificationTitles.ForEach(Console.WriteLine);
// verificationNotes.ForEach(async note =>
// {
//     title = note.GetAttribute("title");
//     if (!File.Exists($"{title}.png"))
//     {
//         var base64String = note.GetAttribute("src")?.Split(",").Last();
//         if (string.IsNullOrWhiteSpace(base64String))
//         {
//             return;
//         }
//         else
//         {
//             try
//             {
//                 await File.WriteAllBytesAsync($"{title}.png", Convert.FromBase64String(base64String));
//                 Console.WriteLine($"wrote {title}.png");
//             }
//             catch (Exception ex)
//             {
//                 Console.WriteLine(ex.Message);
//             }
//         }

//     }
// });