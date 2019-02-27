using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Generics
{
    public abstract class Singleton<T> : MonoBehaviour where T : Component
    {
        public static T Instance;

        protected virtual void Awake()
        {
            if (Instance == null)
            {
                Instance = this as T;
                //DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                Debug.Log("Multiple objects: " + typeof(T).Name);
                Destroy(this.gameObject);
                return;
            }
        }
    }
}
