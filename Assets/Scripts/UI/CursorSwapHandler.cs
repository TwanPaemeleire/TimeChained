using Assets.Scripts.World;
using Unity.VisualScripting;
using UnityEngine;

public class CursorSwapHandler : MonoBehaviour
{
    [SerializeField] private Texture2D _pastCursor;
    [SerializeField] private Texture2D _futureCursor;
    Vector2 _cursorHandle;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Vector2 cursorSize = _pastCursor.Size();
        _cursorHandle = new Vector2(cursorSize.x / 2.0f, cursorSize.y / 2.0f);
        OnWorldSwap();
        WorldSwapHandler.Instance.OnWorldSwap.AddListener(OnWorldSwap);
    }

    void OnWorldSwap()
    {
        if(WorldSwapHandler.Instance.IsInCyberpunkWorld)
        {
            Cursor.SetCursor(_futureCursor, _cursorHandle, CursorMode.ForceSoftware);
        }
        else
        {
            Cursor.SetCursor(_pastCursor, _cursorHandle, CursorMode.ForceSoftware);
        }
    }
}
