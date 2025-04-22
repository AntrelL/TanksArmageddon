using Essentia;

using UnityCanvas = UnityEngine.Canvas;

namespace TanksArmageddon
{
    public partial class Canvas
    {
        private UnityCanvas _unityCanvas;

        public Canvas(UnityCanvas unityCanvas)
        {
            _unityCanvas = unityCanvas;
        }

        public Camera WorldCamera
        {
            get => _unityCanvas.worldCamera;
            set => _unityCanvas.worldCamera = value;
        }

        public static implicit operator Canvas(UnityCanvas unityCanvas) => new(unityCanvas);

        public static implicit operator UnityCanvas(Canvas canvas) => canvas._unityCanvas;
    }
}
