using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Blockchain.Core
{
    public class Blockchain
    {
        private const int MiningDifficulty = 4;
        private Block _headBlock;
        
        public Blockchain()
        {
            _headBlock = CreateGenesisBlock();
        }

        private Block CreateGenesisBlock()
        {
            var block = new Block(0, DateTime.MinValue, "First");
            block.Mine(MiningDifficulty);
            return block;
        }

        public void AddBlock(object data)
        {
            var newBlock = new Block
            (
                _headBlock.Index + 1,
                DateTime.UtcNow,
                data,
                _headBlock
            );
            
            newBlock.Mine(MiningDifficulty);
            _headBlock = newBlock;
        }

        public bool Isvalid()
        {
            Block block = _headBlock;
            do
            {
                if (!block.IsValid())
                    return false;
                block = block.PreviousBlock;
            } while (block != null);

            return true;
        }

        public override string ToString()
        {
           var builder = new StringBuilder();
           _headBlock.WriteDescendants(builder);           
            
            return builder.ToString();

            //return JsonConvert.SerializeObject(_headBlock, Formatting.Indented);
        }
    }
}