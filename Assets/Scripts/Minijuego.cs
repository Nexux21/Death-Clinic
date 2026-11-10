using UnityEngine;

public class Minijuego : MonoBehaviour
{
    [SerializeField] private GameObject senal;
    [SerializeField] private KeyCode tecla = KeyCode.E;
    [Tooltip("Déjalo vacío para aceptar cualquier herramienta")]
    [SerializeField] private string herramientaRequerida = "";

    private Collider2D zonaCol;

    void Awake()
    {
        zonaCol = GetComponent<Collider2D>();
        if (senal != null) senal.SetActive(false);
    }

    void Update()
    {
        bool herramientaCerca = HayHerramientaCorrecta();

        // Mostrar u ocultar la señal
        if (senal != null && senal.activeSelf != herramientaCerca)
            senal.SetActive(herramientaCerca);

        // Si está cerca y presiono la tecla
        if (herramientaCerca && Input.GetKeyDown(tecla))
            Operar();
    }
    bool HayHerramientaCorrecta()
    {
        Herramienta herramienta = Herramienta.HerramientaActual;
        if (herramienta == null) return false;

        if (herramientaRequerida != "" &&
            herramienta.nombreHerramienta != herramientaRequerida)
            return false;

        Collider2D herramientaCol = herramienta.GetComponent<Collider2D>();
        return herramientaCol != null && zonaCol.Distance(herramientaCol).isOverlapped;
    }
    void Operar()
    {
        Debug.Log("¡Operando en " + gameObject.name + "!");
        Destroy(gameObject);
    }
}