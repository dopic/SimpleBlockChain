using System;
using System.Collections.Generic;
using System.Linq;

namespace Blockchain.Core
{
    public class Blockchain
    {
        private const int MINING_DIFFICULTY = 4;
        
        public List<Block> Chain { get; }

        public Blockchain()
        {
            Chain = new List<Block>
            {
                CreateGenesisBlock()
            };
        }

        private Block CreateGenesisBlock()
        {
            var block = new Block(0, DateTime.MinValue, "First");
            block.Mine(MINING_DIFFICULTY);
            return block;
        }

        public void AddBlock(object data)
        {
            var previousHash = Chain.Last().Hash;
            var newBlock = new Block
            (
                Chain.Count,
                DateTime.UtcNow,
                data,
                previousHash
            );
            
            newBlock.Mine(MINING_DIFFICULTY);
            Chain.Add(newBlock);
        }

        public bool Isvalid()
        {
            Block previousBlock = null;
            foreach (var block in Chain)
            {
                if (!block.IsValid())
                    return false;

                if (previousBlock != null && previousBlock.Hash != block.PreviousHash)
                    return false;

                previousBlock = block;
            }

            return true;
        }
    }
}