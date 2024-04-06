using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static T instance;

    public static T Instance { get => GetInstance();}

    [SerializeField] protected bool dontDestroyOnLoad = false;

    protected virtual void Awake()
    {
        if(instance == null)
        {
            instance = (T)this;
            if(dontDestroyOnLoad)
            {
                DontDestroyOnLoad(gameObject);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    protected virtual void OnDestroy()
    {
        if(instance == this)
        {
            instance = null;
        }
    }

    public static T GetInstance()
    {
        if(instance == null)
        {
            var go = new GameObject(typeof(T).ToString());
            var gs = go.AddComponent<T>();
            go.hideFlags = HideFlags.DontSaveInBuild | HideFlags.DontSaveInEditor;
            instance = gs;
        }
        return instance;
    }
}
