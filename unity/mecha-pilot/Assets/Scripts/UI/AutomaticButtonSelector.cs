using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class AutomaticButtonSelector : MonoBehaviour
    {
        public Button targetButton;
        public bool onEnableFlag;
        private void OnEnable()
        {
            if (targetButton == null) return;
            if (onEnableFlag == false) return;

            targetButton.Select();
        }
    }
}
