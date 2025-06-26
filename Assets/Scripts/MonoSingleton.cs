using UnityEngine;

public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    static T m_Instance;
    static bool hasBeenCreated;
    public static T Instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = FindAnyObjectByType<T>();
                if (m_Instance == null)
                {
                    if (!hasBeenCreated)
                        m_Instance = new GameObject("_" + typeof(T), typeof(T)).GetComponent<T>();
                }
                else
                {
                    hasBeenCreated = true;
                    DontDestroyOnLoad(m_Instance);
                    m_Instance.Init();
                }
                return m_Instance;
            }
            return m_Instance;
        }
    }

    private void Awake()
    {
        DontDestroyOnLoad(this);
        if (m_Instance == null)
        {
            m_Instance = this as T;
            hasBeenCreated = true;
            m_Instance.Init();
        }
    }
    protected virtual void Init() { }
}
