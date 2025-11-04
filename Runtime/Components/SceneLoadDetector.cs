using UnityEngine;

namespace Mobiray.DI
{
    [DefaultExecutionOrder(-1000)]
    public class SceneLoadDetector : MonoBehaviour
    {
        private void Awake()
        {
            FindObjectOfType<GameInstaller>().InjectAll();
        }
    }
}