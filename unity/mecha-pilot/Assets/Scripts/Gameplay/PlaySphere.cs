using UnityEngine;

namespace Gameplay
{

    [RequireComponent(typeof(SphereCollider))]
    public class PlaySphere : MonoBehaviour
    {
        // responsible for deactivating things that go out of bounds
        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent<IOutOfPlaySphere>(out var playSphereOutsider))
                playSphereOutsider.OnPlaySphereOutOfBounds();
        }
    }
}
