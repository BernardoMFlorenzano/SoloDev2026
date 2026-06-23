using Unity.Cinemachine;
using UnityEngine;

namespace CameraSystem
{
    public class CameraManager : MonoBehaviour
    {
        public static CameraManager Instance;

        [SerializeField] private CameraData cameraData;
        [SerializeField] private Camera mainCamera;
        private CinemachineBrain cinemachineBrain;

        private void Awake()
        {
            cinemachineBrain = mainCamera.GetComponent<CinemachineBrain>();
            cinemachineBrain.DefaultBlend.Time = cameraData.transitionDuration;
        }

        public void SetBlendTime(float time) { cinemachineBrain.DefaultBlend.Time = time; }
    }
}