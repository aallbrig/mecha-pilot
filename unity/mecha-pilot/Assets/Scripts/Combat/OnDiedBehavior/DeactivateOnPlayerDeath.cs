using System;
using Character;
using UnityEngine;

namespace Combat.OnDiedBehavior
{
    public class DeactivateOnPlayerDeath : MonoBehaviour
    {
        public PlayerController player;
        private void Start()
        {
            player ??= FindObjectOfType<PlayerController>(true);
            if (player && player.gameObject.TryGetComponent<ICanDie>(out var ableToDie))
                ableToDie.Died += _ => Deactivate();
        }
        private void Update()
        {
            if (player == null) return;
            if (!player.isActiveAndEnabled) Deactivate();
        }

        public event Action Deactivated;

        private void Deactivate()
        {
            gameObject.SetActive(false);
            Deactivated?.Invoke();
        }
    }
}
