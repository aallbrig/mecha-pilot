using System.Collections;
using UnityEngine;

namespace Combat
{
    public class Projectile : MonoBehaviour
    {
        public Vector3 normalizedVector = Vector3.zero;
        public float speedInSeconds = 10f;
        public float lifetimeInSeconds = 5f;
        private void Start() => StartCoroutine(ProjectileLifetime());
        private void Update()
        {
            if (normalizedVector == Vector3.zero) gameObject.SetActive(false);
            transform.Translate(normalizedVector * (speedInSeconds * Time.deltaTime));
        }
        private IEnumerator ProjectileLifetime()
        {
            yield return new WaitForSeconds(lifetimeInSeconds);
            gameObject.SetActive(false);
        }
    }
}
