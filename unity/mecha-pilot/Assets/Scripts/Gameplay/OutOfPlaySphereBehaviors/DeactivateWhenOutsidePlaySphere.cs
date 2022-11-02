using UnityEngine;

namespace Gameplay.OutOfPlaySphereBehaviors
{
    public class DeactivateWhenOutsidePlaySphere : MonoBehaviour, IOutOfPlaySphere
    {

        public void OnPlaySphereOutOfBounds() => gameObject.SetActive(false);
    }
}
