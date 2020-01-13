using Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LevelModel : ScriptableObject
{
	private const string SETTINGS_PATH = "Levels";
	private const string LEVEL_PREFIX_FORMAT = "Level_{0}";
	private static int _levelCount = 1;

	public int LevelNumber = 0;
	public BlockType[] AvailableBlocks;
	public BonusType[] AvailableBonuses;
	public BonusesModel BonusModel;
	public int[] StarScoreData = new int[3];
	//goals
	//limitation
	public int MovesAmount;

    public GoalModel GoalData;

	public GameFieldRaw GameFieldRawData;

#if UNITY_EDITOR
	[MenuItem("Tools/Levels/Create Level Data")]
	public static void CreateLevelModel()
	{
		AssetUtils.CreateAsset<LevelModel>("Assets/Resources/Levels", string.Format(LEVEL_PREFIX_FORMAT, _levelCount++));
	}
#endif

	public static LevelModel Current { get; private set; }

	public static LevelModel LoadLevel(int levelNumber)
	{
		var levelName = string.Format(LEVEL_PREFIX_FORMAT, levelNumber);
		var path = string.Format("{0}/{1}", SETTINGS_PATH, levelName);
		var levelModel = Resources.Load<LevelModel>(path);
		if (levelModel == null)
		{
			Debug.LogError($"Can not load level at path: {path}");
            return LoadLevel(levelNumber - 1);
		}
		else
		{
			Current = levelModel;
		}

		return levelModel;
	}
}