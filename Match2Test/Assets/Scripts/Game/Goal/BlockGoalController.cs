using System;
using UnityEngine;

namespace Game
{
    public class BlockGoalController : IBlockGoalController
    {
        public bool IsComplete => _blockGoalModel.Amount == _counter;
        public BlockType Type => _blockGoalModel.Type;
        public int Amount => _blockGoalModel.Amount - _counter;

        public event Action<IBlockGoalController> BlockGoalCompleteEvent;
        public event BlockMatchingDelegate BlocGoalAmountChangedEvent;

        private readonly BlockGoalModel _blockGoalModel;
        private readonly IBlockMatchingEvent _blockMatchingEvent;

        private int _counter = 0;

        private BlockGoalController(BlockGoalModel blockGoalModel, IBlockMatchingEvent blockMatchingEvent) {
            _blockGoalModel = blockGoalModel;
            _blockMatchingEvent = blockMatchingEvent;

            _blockMatchingEvent.BlockMatchingEvent += BlockMatchingHandler;
        }        

        public static IBlockGoalController Create(BlockGoalModel blockGoalModel, IBlockMatchingEvent blockMatchingEvent) {
            return new BlockGoalController(blockGoalModel, blockMatchingEvent);
        }

        public void Init(BlockGoalModel blockGoalModel) {

        }

        private void BlockMatchingHandler(BlockMatchingEventArgs blockGoalChangedEventArgs) {
            //Debug.Log($"[{GetType().Name}][BlockMatchingHandler] blockType: {blockGoalChangedEventArgs.Type}, " +
            //    $"amount: {blockGoalChangedEventArgs.Amount}, " +
            //    $"type:{Type}, " +
            //    $"counter: {_counter}, " +
            //    $"Amount: {_blockGoalModel.Amount}");

            if (blockGoalChangedEventArgs.Type != Type)
                return;

            if (IsComplete)
                return;

            _counter += blockGoalChangedEventArgs.Amount;
            RaisedBlockGoalChangedEvent();

            if (_counter >= _blockGoalModel.Amount) {
                _counter = _blockGoalModel.Amount;
                RaiseBlockGoalCompleteEvent();
            }
        }

        private void RaisedBlockGoalChangedEvent() {
            BlocGoalAmountChangedEvent?.Invoke(new BlockMatchingEventArgs(Type, Amount));
        }

        private void RaiseBlockGoalCompleteEvent() {
            _blockMatchingEvent.BlockMatchingEvent -= BlockMatchingHandler;

            BlockGoalCompleteEvent?.Invoke(this);
        }
    }   
}