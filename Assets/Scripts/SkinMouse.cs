using UnityEngine;

public class Mouse : MonoBehaviour
{
    [SerializeField] private Texture2D imagenCursor;
    [SerializeField] private Vector2 puntoActivo = Vector2.zero;

    void Start()
    {
        Cursor.SetCursor(imagenCursor, puntoActivo, CursorMode.Auto);
    }
}