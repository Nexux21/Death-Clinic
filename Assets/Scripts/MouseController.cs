using UnityEngine;
public class MouseController : MonoBehaviour
{
    private static MouseController herramientaActual;

    private Camera cam;
    private Vector3 offset;

    public bool EstaAgarrada => herramientaActual == this;

    void Awake()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && herramientaActual == null)
        {
            Vector2 mouse = GetMouseWorld();
            Collider2D col = GetComponent<Collider2D>();

            if (col != null && col.OverlapPoint(mouse))
            {
                herramientaActual = this;
                offset = transform.position - (Vector3)mouse;
            }
        }

        if (Input.GetMouseButtonDown(1) && EstaAgarrada)
        {
            herramientaActual = null;
        }

        if (EstaAgarrada)
        {
            Vector3 destino = (Vector3)GetMouseWorld() + offset;
            destino.z = transform.position.z;
            transform.position = destino;
        }
    }
    Vector2 GetMouseWorld()
    {
        return cam.ScreenToWorldPoint(Input.mousePosition);
    }
}