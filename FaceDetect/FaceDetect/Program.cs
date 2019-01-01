using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;

using SixLabors.Primitives;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.PixelFormats;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace FaceDetect
{
    class Program
    {
        private static string msg = "Please provide API key as first command line argument, followed by the filename of the image you want to provide.";

        public class PersonId
        {
            public string Id { get; set; }
            public string Age { get; set; }
            public string Smile { get; set; }
            public string Gender { get; set; }
            public string Emotion { get; set; }
            public string Hair { get; set; }
            public PointF[] Rectangle { get; set; }

            public PersonId(string id, string age, string smile, string gender, string emotions, string hair, PointF[] rectangle)
            {
                Id = id;
                Age = age;
                Smile = smile;
                Gender = gender;
                Emotion = emotions;
                Hair = hair;
                Rectangle = rectangle;
            }
        }

        static void Main(string[] args)
        {
            // Command Line Arguments
            var apiKey = !string.IsNullOrWhiteSpace(args[0]) ? args[0] : throw new ArgumentException(msg, args[0]);
            var filename = File.Exists(args[1]) ? args[1] : throw new FileNotFoundException(msg, args[1]);

            // HTTP Request
            var region = "westcentralus";
            var target = new Uri($"https://{region}.api.cognitive.microsoft.com/face/v1.0/detect/?subscription-key={apiKey}");
            var httpPost = CreateHttpRequest(target, "POST", "application/octet-stream");

            // Load Image
            using (var fs = File.OpenRead(filename))
            {
                fs.CopyTo(httpPost.GetRequestStream());
            }

            // Submit image to HTTP endpoint
            string data = GetResponse(httpPost);

            // Inspect JSON
            List<PersonId> persons = GetPersonId(data);

            // Draw rectangles on a copy of the image
            var img = Image.Load(filename);

            var count = 0;
            foreach (var person in persons)
            {
                img.Mutate(a => a.DrawPolygon(Rgba32.HotPink, 20, person.Rectangle));
                var sb = new StringBuilder("Would you take a look at this spud? ");
                sb.AppendLine($"I'd say that this person looks about {person.Age} ; ");
                sb.AppendLine($"He looks quite {person.Emotion}.....");
                Console.WriteLine(sb);
                count++;
            }
            Console.WriteLine($"Number of faces detected: {count}");

            var outFileName = $"{Environment.CurrentDirectory}\\{Path.GetFileNameWithoutExtension(filename)}-2{Path.GetExtension(filename)}";
            SaveImage(img, outFileName);

            OpenWithDefaultApp(outFileName);

            // Don't exit
            Console.ReadLine();
        }

        private static void OpenWithDefaultApp(string filename)
        {
            var si = new ProcessStartInfo()
            {
                FileName = "explorer.exe",
                Arguments = filename,
                WindowStyle = ProcessWindowStyle.Hidden
            };
            Process.Start(si);
        }

        private static void SaveImage(Image<Rgba32> img, string outFileName)
        {
            using (var fs = File.Create(outFileName))
            {
                img.SaveAsJpeg(fs);
            }
        }

        private static List<PersonId> GetPersonId(string data)
        {
            JArray faces = JArray.Parse(data);
            var persons = new List<PersonId>();

            foreach (JObject face in faces)
            {
                var id = (string)face["faceId"];
                var top = (int)face["faceRectangle"]["top"];
                var left = (int)face["faceRectangle"]["left"];
                var width = (int)face["faceRectangle"]["width"];
                var height = (int)face["faceRectangle"]["height"];
                var age = "12";
                var gender = "Non-binary";
                var smile = "Doesn't exist";
                var emotion = "Angery";
                var hair = "Receding lmao";

                //var emotionsToken = face.SelectToken("faceAttributes.emotion");
                //var emotions = emotionsToken.Ancestors();
                //var sigEmotion = "Angry";
                //var emotionConfidence = 0.0f;
                //foreach (var emotion in emotions)
                //{
                //    JProperty jProp = emotion.ToObject<JProperty>();
                //    string emotionType = jProp.Name;
                //    if ((float)emotion > emotionConfidence)
                //    {
                //        sigEmotion = emotionType;
                //        emotionConfidence = (float)emotion;
                //    }
                //}

                //var hairColours = face.SelectToken("faceAttributes.hair.hairColor");
                //var hairConfidence = 0.0f;
                //var hair = "Blonde";
                //foreach (var hairCol in hairColours)
                //{
                //    JProperty jProp = hairCol.ToObject<JProperty>();
                //    string hairColour = jProp.Name;
                //    if ((float)hairCol > hairConfidence)
                //    {
                //        hair = hairColour;
                //        hairConfidence = (float)hairCol;
                //    }
                //}

                var rectangle = new PointF[]
                {
                    new PointF(left, top),
                    new PointF(left + width, top),
                    new PointF(left + width, top + height),
                    new PointF(left, top + height)

                };

                var person = new PersonId(id, age, smile, gender, emotion, hair, rectangle);
                persons.Add(person);
            }
            return persons;
        }

        private static string GetResponse(HttpWebRequest httpPost)
        {
            using (var response = httpPost.GetResponse())
            {
                using (var sr = new StreamReader(response.GetResponseStream()))
                {
                    return sr.ReadToEnd();
                }
            }
        }

        private static HttpWebRequest CreateHttpRequest(Uri target, string method, string contentType)
        {
            var request = WebRequest.CreateHttp(target);
            request.Method = method;
            request.ContentType = contentType;
            return request;
        }
    }
}
