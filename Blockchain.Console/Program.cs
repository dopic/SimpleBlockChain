using System;
using Newtonsoft.Json;

namespace Blockchain.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var blockchain = new Core.Blockchain();
            blockchain.AddBlock(new {Souce = "Douglas", Target = "Magda", Amount = 10.00});
            blockchain.AddBlock(new {Souce = "Magda", Target = "Ilir", Amount = 5.00});

            System.Console.WriteLine(JsonConvert.SerializeObject(blockchain, Formatting.Indented));

            System.Console.WriteLine($"Is blockchain valid? {blockchain.Isvalid()}");

            System.Console.ReadLine();
        }
    }
}