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
                ableToDie.Died += _ => gameObject.SetActive(false);
        }
        private void Update()
        {
            if (player == null) return;
            if (!player.isActiveAndEnabled) gameObject.SetActive(false);
        }
    }
}
