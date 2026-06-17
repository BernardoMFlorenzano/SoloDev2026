using UnityEngine;

namespace CameraSystem
{
    [CreateAssetMenu(fileName = "CameraData", menuName = "Scriptable Objects/CameraData")]
    public class CameraData : ScriptableObject
    {
        public Vector2 defaultAspectRatio = new(16f, 9f);
        public Color barColor;
        public float transitionDuration;
    }
}