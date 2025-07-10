using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Source.Scripts.Release.Player
{
    public class MobileInputHandler : MonoBehaviour
    {
        [SerializeField] private Button _leftButton;
        [SerializeField] private Button _rightButton;

        public bool LeftPressed { get; private set; }
        public bool RightPressed { get; private set; }

        private void Awake()
        {
            if (!YG.YG2.envir.isMobile) return;

            AddEvent(_leftButton.gameObject, EventTriggerType.PointerDown, () => LeftPressed = true);
            AddEvent(_leftButton.gameObject, EventTriggerType.PointerUp, () => LeftPressed = false);

            AddEvent(_rightButton.gameObject, EventTriggerType.PointerDown, () => RightPressed = true);
            AddEvent(_rightButton.gameObject, EventTriggerType.PointerUp, () => RightPressed = false);
        }

        private void AddEvent(GameObject target, EventTriggerType type, System.Action callback)
        {
            var trigger = target.GetComponent<EventTrigger>() ?? target.AddComponent<EventTrigger>();
            var entry = new EventTrigger.Entry { eventID = type };
            entry.callback.AddListener(_ => callback());
            trigger.triggers.Add(entry);
        }
    }
}