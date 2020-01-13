using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameElementHolder : MonoBehaviour
{
	[SerializeField] private List<BlockView> _blocks = null;
	[SerializeField] private List<BonusView> _bonuses = null;
	[SerializeField] private List<BonusApplier> _bonusApplier = null;

	public BlockView GetBlockView(BlockType blockType)
	{
		return _blocks.Find(block => block.Type == blockType);
	}

	public BonusView GetBonusView(BonusType bonusType)
	{
		return _bonuses.Find(bonus => bonus.Type == bonusType);
	}

	public BonusApplier GetBonusApplier(BonusType bonusType) {
		return _bonusApplier.Find(bonus => bonus.Type == bonusType);
	}
}
