using UnityEngine;

namespace Combat
{
    public class Projectile : MonoBehaviour
    {
        public Vector3 initialSpeedVector = Vector3.zero;
        public Vector3 firingDirectionNormalized = Vector3.zero;
        public float speedInSeconds = 10f;
        private void Update()
        {
            if (firingDirectionNormalized == Vector3.zero) gameObject.SetActive(false);
            transform.Translate(initialSpeedVector * Time.deltaTime +
                                firingDirectionNormalized * (speedInSeconds * Time.deltaTime));
        }
    }
}
