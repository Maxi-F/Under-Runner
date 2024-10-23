using TMPro;
using UnityEngine;

namespace DEBUG.UI
{
    public class ShowID : MonoBehaviour
    {
        [SerializeField] private GameObject aGameObject;
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private bool showIds;
        
        #if UNITY_EDITOR
        private void Update()
        {
            if(showIds)
                text.text = $"{aGameObject.gameObject.GetInstanceID()}";
        }
        #endif
    }
}
