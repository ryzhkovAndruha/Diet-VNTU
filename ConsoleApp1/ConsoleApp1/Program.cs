// See https://aka.ms/new-console-template for more information
using System.Text;
using System.Text.Json.Nodes;
using System.Text.Json;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using Newtonsoft.Json;
using System;

Console.WriteLine("Hello, World!");
StringBuilder stringBuilder = new StringBuilder();
stringBuilder.AppendLine("{Перша лінія");
stringBuilder.AppendLine("Друга лінія");
stringBuilder.AppendLine("Третя лінія}");

byte[] utf8Bytes = Encoding.UTF8.GetBytes(stringBuilder.ToString());
string utf8String = Encoding.UTF8.GetString(utf8Bytes);
string json = JsonConvert.SerializeObject(utf8String, Formatting.Indented);

Console.WriteLine(json);
