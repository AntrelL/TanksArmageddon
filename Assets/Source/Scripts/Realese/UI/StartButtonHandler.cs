using IJunior.TypedScenes;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TanksArmageddon
{
    public class StartButtonHandler : MonoBehaviour, IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            TrainingScene.Load();
        }
    }
}