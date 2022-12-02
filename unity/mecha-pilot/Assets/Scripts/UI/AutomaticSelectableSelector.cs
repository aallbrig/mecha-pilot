using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class AutomaticSelectableSelector : MonoBehaviour
    {
        public Selectable targetSelectable;
        public bool onEnableFlag;
        private void OnEnable()
        {
            if (targetSelectable == null) return;
            if (onEnableFlag == false) return;

            targetSelectable.Select();
        }
    }
}
