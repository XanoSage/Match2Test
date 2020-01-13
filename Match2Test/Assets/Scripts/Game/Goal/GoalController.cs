using Game.Audio;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Game
{
    public class GoalController : IGoalController
    {
        public List<IBlockGoal> BlockGoalsList { get; }

        private readonly List<IBlockGoalController> _blockGoalControllers;

        public event Action GoalsCompleteEvent;
        public event Action<IBlockGoal> BlockGoalAddedEvent;
        public event Action<IBlockGoal> BlockGoalRemovedEvent;

        private GoalModel _goalModel;
        private readonly IBlockMatchingEvent _blockMatchingEvent;
        private GoalController(IBlockMatchingEvent blockMatchingEvent) {
            BlockGoalsList = new List<IBlockGoal>();
            _blockGoalControllers = new List<IBlockGoalController>();
            _blockMatchingEvent = blockMatchingEvent;
        }

        public static IGoalController Create(IBlockMatchingEvent blockMatchingEvent) {
            return new GoalController(blockMatchingEvent);
        }

        public void Init(GoalModel goalModel) {
            _goalModel = goalModel;
            foreach (var blockGoalModel in _goalModel.BlockGoalsList) {
                CreateBlockGoalController(blockGoalModel);
            }
        }

        public void Clear() {

            foreach (var item in _blockGoalControllers) {
                RaiseBlockGoalRemovedEvent(item);
            }
            _blockGoalControllers.Clear();
        }

        public List<BlockType> GetBlockGoals() {
            var result = new List<BlockType>();
            foreach (var blockGoalModel in _goalModel.BlockGoalsList) {
                result.Add(blockGoalModel.Type);
            }
            return result;
        }

        private void RaiseGoalsCompleteEvent() {
            GoalsCompleteEvent?.Invoke();
        }

        private void CheckOtherForCompletation() {
            foreach (var blockGoalController in _blockGoalControllers) {
                if (!blockGoalController.IsComplete)
                    return;
            }

            RaiseGoalsCompleteEvent();
        }

        private void CreateBlockGoalController(BlockGoalModel blockGoalModel) {
            var blockGoalController = Factory.GoalFactory.CreateBlockGoalController(blockGoalModel, _blockMatchingEvent);
            blockGoalController.BlockGoalCompleteEvent += OnBlockGoalCompleteHandler;
            _blockGoalControllers.Add(blockGoalController);
            BlockGoalsList.Add(blockGoalController);
            RaiseBlockGoalAddedEvent(blockGoalController);
        }

        private void OnBlockGoalCompleteHandler(IBlockGoalController blockGoalController) {
            blockGoalController.BlockGoalCompleteEvent -= OnBlockGoalCompleteHandler;
            CheckOtherForCompletation();

            AudioManager.PlaySoundFx(SoundFxType.GoalComplete);
        }

        private void RaiseBlockGoalAddedEvent(IBlockGoal blockGoal) {
            BlockGoalAddedEvent?.Invoke(blockGoal);
        }

        private void RaiseBlockGoalRemovedEvent(IBlockGoal blockGoal) {
            BlockGoalRemovedEvent?.Invoke(blockGoal);
        }

        
    }

       
}