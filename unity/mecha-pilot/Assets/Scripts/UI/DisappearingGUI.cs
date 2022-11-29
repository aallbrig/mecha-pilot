using System.Collections;
using UnityEngine;

namespace UI
{
    public class DisappearingGUI : MonoBehaviour
    {
        public float disappearAfterSeconds = 3f;
        private void Start() => StartCoroutine(Disappear());
        private IEnumerator Disappear()
        {
            yield return new WaitForSeconds(disappearAfterSeconds);
            gameObject.SetActive(false);
        }
    }
}
