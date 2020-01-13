using Game.Audio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonClickHelper : MonoBehaviour
{
    private Button _button;

    private void Awake() {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnButtonClickHandler);
    }

    private void OnButtonClickHandler() {
        AudioManager.PlaySoundFx(SoundFxType.ButtonClick);
    }
}
