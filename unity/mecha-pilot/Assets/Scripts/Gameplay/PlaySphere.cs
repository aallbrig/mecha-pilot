using UnityEngine;

namespace Gameplay
{
    public interface IOutOfPlaySphere
    {
        public void OnPlaySphereOutOfBounds();
    }

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
