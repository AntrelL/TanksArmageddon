using IJunior.TypedScenes;
using UnityEngine;
using UnityEngine.EventSystems;

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