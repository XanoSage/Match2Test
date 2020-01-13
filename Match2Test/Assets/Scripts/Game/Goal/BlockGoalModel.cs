using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class BlockGoalModel
    {
        public BlockType Type;
        public int Amount;
    }

    [Serializable]
    public class GoalModel
    {
        public List<BlockGoalModel> BlockGoalsList;
    }

    public delegate void BlockMatchingDelegate(BlockMatchingEventArgs eventHandler);

    public class BlockMatchingEventArgs: EventArgs
    {
        public BlockType Type { get; }
        public int Amount { get; }

        public BlockMatchingEventArgs(BlockType type, int amount): base() {
            Type = type;
            Amount = amount;
        }
    }

       
}