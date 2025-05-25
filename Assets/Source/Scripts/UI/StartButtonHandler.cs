using IJunior.TypedScenes;
using UnityEngine.EventSystems;
using UnityEngine;

namespace TanksArmageddon
{
    public class StartButtonHandler : MonoBehaviour, IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log("Start button pressed!");
            TrainingScene.Load();
        }
    }
}
