using AzureStorageLibrary.Services;
using System.Text;

AzureStorageLibrary.ConnectionStrings.AzureStorageConnectionString = "AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;DefaultEndpointsProtocol=http;BlobEndpoint=http://127.0.0.1:10000/devstoreaccount1;QueueEndpoint=http://127.0.0.1:10001/devstoreaccount1;TableEndpoint=http://127.0.0.1:10002/devstoreaccount1;";

AzQueue queue = new AzQueue("ornekkuyruk");

string base64message = Convert.ToBase64String(Encoding.UTF8.GetBytes("Mesaj"));

queue.SendMessageAsync(base64message).Wait();

Console.WriteLine("Mesaj Kaydedildi.");

var message = queue.RetrieveNextMessageAsync().Result;

string text = Encoding.UTF8.GetString(Convert.FromBase64String(message.MessageText));

Console.WriteLine("Mesaj: " + text);

queue.DeleteMessage(message.MessageId, message.PopReceipt).Wait();

Console.WriteLine("Mesaj Silindi.");