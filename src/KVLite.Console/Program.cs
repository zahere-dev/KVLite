// See https://aka.ms/new-console-template for more information
using KVLite.Core.Storage;

var kvStore = new KeyValueStore();

// Test the KeyValueStore methods
kvStore.Set("testKey", "testValue");

var value = kvStore.Get("testKey");
Console.WriteLine(value);   
Console.ReadLine();
                