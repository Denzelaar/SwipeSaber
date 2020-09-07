using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{

    static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {

                // Find existing instances, allowing Singletons to be placed in the scene
                _instance = (T)FindObjectOfType(typeof(T));

                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject();
                    singletonObject.name = typeof(T).ToString();
                    _instance = singletonObject.AddComponent<T>();

                    DontDestroyOnLoad(singletonObject);
                }
            }

            return _instance;
        }
    }

    virtual public void Init()
    {

    }

    /* Can be used in OnDestroy of GameObjects to make sure an instance of the Singleton still exists */
    public static bool Exists()
    {
        return _instance != null;
    }

}

