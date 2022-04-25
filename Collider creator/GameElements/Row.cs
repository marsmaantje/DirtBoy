using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;

namespace GameElements
{
    class Row : Pivot
    {
        public List<Block> Blocks;
        public readonly float width;




        /// <summary>
        /// Generates a new row with the given blockCount and width.
        /// </summary>
        /// <param name="blockCount">amount of blocks to generate</param>
        /// <param name="width"> width of the total row</param>
        /// <param name="gap">gap between each block</param>
        public Row(int blockCount, int width, int gap)
        {
            Blocks = new List<Block>();
            this.width = width;
            int blockWidth = (width - (blockCount * (gap - 1))) / blockCount;

            for (int i = 0; i < blockCount; i++)
            {
                Block newBlock = new Block(blockWidth, blockWidth, Utils.Random(0, Block.VariationCount), Utils.Random(10, 50));
                AddChild(newBlock);
                Blocks.Add(newBlock);
                newBlock.position = new Vec2(Mathf.Map(i, 0, blockCount-1, -width/2f, width/2f), 0);
            }
        }
    }
}
