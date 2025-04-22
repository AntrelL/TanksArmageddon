using UnityCamera = UnityEngine.Camera;

namespace Essentia
{
    public partial class Camera
    {
        private UnityCamera _unityCamera;

        public Camera(UnityCamera unityCamera)
        {
            _unityCamera = unityCamera;
        }

        public static implicit operator Camera(UnityCamera unityCamera) => new(unityCamera);

        public static implicit operator UnityCamera(Camera camera) => camera._unityCamera;
    }
}
