using UnityEngine;
public class Herramienta : MonoBehaviour
{
    public static Herramienta HerramientaActual { get; private set; }
    [SerializeField] public string nombreHerramienta = "Pinzas";
    private Camera cam;
    private Vector3 offset;
    public bool EstaAgarrada => HerramientaActual == this;
    void Awake()
    {
        cam = Camera.main;
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && HerramientaActual == null)
        {
            Vector2 mouse = GetMouseWorld();
            Collider2D col = GetComponent<Collider2D>();
            if (col != null && col.OverlapPoint(mouse))
            {
                HerramientaActual = this;
                offset = transform.position - (Vector3)mouse;
            }
        }
        if (Input.GetMouseButtonDown(1) && EstaAgarrada)
        {
            HerramientaActual = null;
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