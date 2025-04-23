using UnityEngine;

namespace Assets.Scripts.Map.PlayerInput
{
    public class PlayerAudioComponent : MonoBehaviour
    {
        [field: SerializeField] private AudioClip _turnSound;
        [field: SerializeField] private AudioClip _economicUpgradeSound;
        [field: SerializeField] private AudioClip _militaryUpgradeSound;

        private SoundManager _soundManager;

        private void Awake()
        {
            _soundManager = SoundManager.Instance;
        }

        public void PlayEconomicUpgradeSound()
        {
            _soundManager.PlaySound(_economicUpgradeSound);
        }

        public  void PlayMilitaryUpgradeSound()
        {
            _soundManager.PlaySound(_militaryUpgradeSound);
        }

        public void PlayEndTurnSound()
        {
            _soundManager.PlaySound(_turnSound);
        }
    }
}
