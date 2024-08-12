using TMPro;
using UnityEngine;

namespace Code.Runtime.UI.Screens
{
    public class UICounterView : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _count;

        public void Initialize(int count)
        {
            _count.text = count.ToString();
        }

        public void SetCount(int count) => 
            _count.text = count.ToString();
    }
}