using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MenuSystem
{
    [RequireComponent(typeof(Button))]
    public class ButtonBehaviour : MonoBehaviour, ISelectHandler, IDeselectHandler
    {
        private Button button;
        private SelectAnimation selectAnimation;

        private void Awake()
        {
            button = GetComponent<Button>();

            selectAnimation = new();
        }

        public void OnSelect(BaseEventData eventData)
        {
            selectAnimation.ApplyEffect(button);
        }

        public void OnDeselect(BaseEventData eventData)
        {
            selectAnimation.RemoveEffect(button);
        }
    }
}