using TanksArmageddon.Core.CompositionRoot;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TanksArmageddon
{
    public class StartButtonPressed : MonoScript, IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log("Start button pressed!");
        }
    }
}
