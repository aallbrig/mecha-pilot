using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

namespace UI
{
    public class InitialLetterInputForm : MonoBehaviour
    {
        public List<TMP_InputField> letterInputs = new();
        public string GetInitials()
        {
            var stringBuilder = new StringBuilder();
            letterInputs.ForEach(inputField => stringBuilder.Append(inputField.text));
            return stringBuilder.ToString();
        }
    }
}
