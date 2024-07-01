using Unity.VisualScripting;
using UnityEngine;

public class Singletone<T> : MonoBehaviour where T : MonoBehaviour
{
    private static readonly object _threadLock = new object();

    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindAnyObjectByType<T>();
                lock (_threadLock)
                {
                    if (_instance == null)
                    {
                        var singletonGo = new GameObject();
                        singletonGo.name = typeof(T).Name;

                        _instance = singletonGo.AddComponent<T>();
                    }

                    DontDestroyOnLoad(_instance);
                }
            }

            return _instance;
        }
    }
}