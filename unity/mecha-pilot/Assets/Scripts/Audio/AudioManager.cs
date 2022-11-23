using UnityEngine;

namespace System
{
    public class AudioManager : MonoBehaviour
    {
        private void Start() => AudioSettings.Mobile.OnMuteStateChanged += OnAudioConfigurationChanged;
        private void OnAudioConfigurationChanged(bool currentVolumeIsZero)
        {
            if (currentVolumeIsZero) return;

            if (!AudioSettings.Mobile.audioOutputStarted)
                AudioSettings.Mobile.StartAudioOutput();
        }
    }
}
