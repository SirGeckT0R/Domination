using UnityEngine;
using UnityEngine.UI;

public class SliderHandler : MonoBehaviour
{
    [SerializeField] private SoundType _soundType;
    private Slider _slider;
    private SoundManager _manager;

    private void Awake()
    {
        _slider = GetComponent<Slider>();
        _manager = SoundManager.Instance;

        _slider.value = _manager.GetSoundVolume(_soundType);
        _slider.onValueChanged.AddListener(ValueChangeCheck);
    }

    public void ValueChangeCheck(float value)
    {
        _manager.ChangeSoundVolume(_soundType, value);
    }
}
