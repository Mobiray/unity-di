using System;
using System.Collections.Generic;
using System.Reflection;

using UnityEngine;

namespace Mobiray.DI
{
    public class Injector : MonoBehaviour
    {
        [SerializeField] private bool _injectOnAwake = true;
        
        private void Awake()
        {
            if (_injectOnAwake)
                InjectGameObject();
        }

        public void InjectGameObject()
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
