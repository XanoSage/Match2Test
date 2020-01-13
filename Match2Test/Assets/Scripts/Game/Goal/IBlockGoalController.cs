using System;

namespace Game
{
    public interface IBlockGoal : IBlockGoalInfo, IBlockGoalEvents
    {
    }

    public interface IBlockGoalEvents
    {
        event Action<IBlockGoalController> BlockGoalCompleteEvent;
        event BlockMatchingDelegate BlocGoalAmountChangedEvent;
    }

    public interface IBlockGoalInfo
    {
        bool IsComplete { get; }
        BlockType Type { get; }
        int Amount { get; }
    }

    public interface IBlockGoalController: IBlockGoal
    {
        void Init(BlockGoalModel blockGoalModel);
    }
}