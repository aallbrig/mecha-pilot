using System;
using UnityEngine;

namespace Gameplay.OutOfPlaySphereBehaviors
{
    public class DeactivateWhenOutsidePlaySphere : MonoBehaviour, IOutOfPlaySphere
    {
        public void OnPlaySphereOutOfBounds()
        {
            Deactivated?.Invoke();
            gameObject.SetActive(false);
        }

        public event Action Deactivated;
    }
}
