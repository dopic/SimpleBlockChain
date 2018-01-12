using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace Blockchain.Core
{
    public class Block
    {
        public long Index { get; }
        public string Hash { get; private set; }
        public int Nonce { get; private set; }
        [JsonIgnore]
        public Block PreviousBlock { get; }
        public DateTime Timestamp { get; }
        public object Data { get; }

        public Block(long index, DateTime timestamp, object data, Block previousBlock = null)
        {
            Index = index;
            Timestamp = timestamp;
            Data = data;
            PreviousBlock = previousBlock;
            Hash = CalculateHash();
        }

        private string CalculateHash()
        {
            using (var stream = new MemoryStream())
            {
                Write(stream);

                using (var sha256 = SHA256.Create())
                    return ConvertToHex(sha256.ComputeHash(stream));
            }
        }

        private string ConvertToHex(byte[] bytes)
        {
            var builder = new StringBuilder();

            foreach (var b in bytes)
                builder.Append(b.ToString("x2"));

            return builder.ToString();
        }

        private void Write(MemoryStream stream)
        {
            var bytes = BitConverter.GetBytes(Index);
            stream.Write(bytes, 0, bytes.Length);

            bytes = BitConverter.GetBytes(Timestamp.Ticks);
            stream.Write(bytes, 0, bytes.Length);

            var data = JsonConvert.SerializeObject(Data);
            bytes = Encoding.UTF8.GetBytes(data);
            stream.Write(bytes, 0, bytes.Length);

            bytes = Encoding.UTF8.GetBytes(PreviousBlock?.Hash ?? string.Empty);
            stream.Write(bytes, 0, bytes.Length);

            bytes = BitConverter.GetBytes(Nonce);
            stream.Write(bytes, 0, bytes.Length);

            stream.Position = 0;
        }

        public bool IsValid()
        {
            var calculatedHash = CalculateHash();
            return Hash == calculatedHash;
        }

        public void Mine(int difficulty, int maxIterations = 1000000)
        {
            var iteration = 0;
            while (iteration++ < maxIterations && HashIsValid(difficulty))
            {
                Nonce++;
                Hash = CalculateHash();
            }
        }

        private bool HashIsValid(int difficulty)
        {
            return Hash.Substring(0, difficulty) != "0".PadLeft(difficulty, '0');
        }

        public void WriteDescendants(StringBuilder builder)
        {
            builder.AppendLine(JsonConvert.SerializeObject(this, Formatting.Indented));
            PreviousBlock?.WriteDescendants(builder);
        }
    }
}