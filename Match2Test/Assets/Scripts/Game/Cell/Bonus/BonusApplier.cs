using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusApplier : MonoBehaviour
{
    [SerializeField] protected BonusType _bonusType = BonusType.Bomb;
    public BonusType Type => _bonusType;
    public virtual void Apply() {

    }
}
