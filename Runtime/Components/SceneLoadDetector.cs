using UnityEngine;

namespace Mobiray.DI
{
    /// <summary>
    /// Add to a scene to inject it before any other component runs Awake/OnEnable.
    /// </summary>
    [DefaultExecutionOrder(-1000)]
    public class SceneLoadDetector : MonoBehaviour
    {
        private void Awake()
        {
            UnityDI.InjectScene(gameObject.scene);
        }
    }
}
