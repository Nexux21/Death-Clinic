using UnityEngine;
public class Herramienta : MonoBehaviour
{
    // La herramienta que tengo en la mano (otros scripts pueden leerla)
    public static Herramienta HerramientaActual { get; private set; }

    // 👇 ESTE ES EL BLOQUE NUEVO
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void Reiniciar()
    {
        HerramientaActual = null;
    }

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
        // CLIC IZQUIERDO: agarrar
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

        // CLIC DERECHO: soltar
        if (Input.GetMouseButtonDown(1) && EstaAgarrada)
        {
            HerramientaActual = null;
        }

        // Mientras esté agarrada, sigue al mouse
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