// See https://aka.ms/new-console-template for more information
using KVLite.Core;
using KVLite.Core.Storage;

Console.WriteLine("KVStore Lite Started");

var kvStore = new KeyValueStore();
var server = new Server(kvStore);
server.Start();

Console.ReadLine();
                