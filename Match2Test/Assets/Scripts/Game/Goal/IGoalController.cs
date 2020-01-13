using System;
using System.Collections.Generic;

namespace Game
{
    public interface IGoalInfo
    {
        List<IBlockGoal> BlockGoalsList { get; }
    }

    public interface IGoalEvents
    {
        event Action GoalsCompleteEvent;
        event Action<IBlockGoal> BlockGoalAddedEvent;
        event Action<IBlockGoal> BlockGoalRemovedEvent;
    }

    public interface IGoal : IGoalEvents, IGoalInfo
    {
    }

    public interface IGoalController : IGoal
    {
        void Init(GoalModel goalModel);
        void Clear();
        List<BlockType> GetBlockGoals();
    }

   
}