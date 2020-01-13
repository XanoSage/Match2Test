using Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameElementSpriteHolder : MonoBehaviour
{
    [SerializeField] private List<BlockSpriteData> _blockSpriteDataArray = null;

    private static GameElementSpriteHolder _instance = null;
    private Sprite GetBlockSpriteInner(BlockType blockType) {
        var blockSpriteData= _blockSpriteDataArray.Find(blockData => blockData.Type == blockType);
        if (blockSpriteData != null)
            return blockSpriteData.Sprite;
        return null;
    }

    private void Awake() {
        _instance = this;
    }

    public static Sprite GetBlockSprite(BlockType blockType) {
        return _instance?.GetBlockSpriteInner(blockType);
    }
}

[Serializable]
public class BlockSpriteData
{
    public BlockType Type;
    public Sprite Sprite;
}