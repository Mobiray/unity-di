using System;
using System.Collections.Generic;
using System.Reflection;

using UnityEngine;

namespace Mobiray.DI
{
    public class Injector : MonoBehaviour
    {
        private void Awake()
        {
            InjectGameObject();
        }

        private void InjectGameObject()
        {
            var behaviours = GetComponentsInChildren<MonoBehaviour>(true);
            
            foreach (var behaviour in behaviours)
            {
                if (behaviour != null)
                    DIContainer.Instance.Inject(behaviour);
            }
        }
    }
}
