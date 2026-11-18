using UnityEngine;

public class Mouse : MonoBehaviour
{
    [SerializeField] private bool ocultarCursorDelSistema = true;

    [Header("GameObjects del cursor")]
    [Tooltip("Se muestra cuando NO llevas nada en la mano")]
    [SerializeField] private GameObject manoNormal;
    [Tooltip("Se muestra cuando agarras una herramienta")]
    [SerializeField] private GameObject manoAgarrando;

    private Camera cam;

    void Awake()
    {
        cam = Camera.main;
        Cursor.visible = !ocultarCursorDelSistema;
        ActualizarVisual(false);
    }

    void Update()
    {
        // Seguir al mouse
        Vector3 pos = cam.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0;
        transform.position = pos;

        // Mostrar un objeto u otro según si tengo una herramienta
        ActualizarVisual(Herramienta.HerramientaActual != null);
    }

    void ActualizarVisual(bool agarrando)
    {
        if (manoNormal != null && manoNormal.activeSelf == agarrando)
            manoNormal.SetActive(!agarrando);

        if (manoAgarrando != null && manoAgarrando.activeSelf != agarrando)
            manoAgarrando.SetActive(agarrando);
    }

    void OnDestroy()
    {
        Cursor.visible = true;
    }
}