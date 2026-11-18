using UnityEngine;

public class Minijuego : MonoBehaviour
{
    [SerializeField] private GameObject senal;
    [SerializeField] private KeyCode tecla = KeyCode.E;
    [Tooltip("Déjalo vacío para aceptar cualquier herramienta")]
    [SerializeField] private string herramientaRequerida = "";

    [Header("Cambio de escena")]
    [Tooltip("Nombre exacto de la escena. Vacío = la zona solo desaparece")]
    [SerializeField] private string escenaDestino = "";
    [Tooltip("Identificador único de esta zona (ej: Tumor1)")]
    [SerializeField] private string idZona = "";

    private Collider2D zonaCol;

    void Awake()
    {
        zonaCol = GetComponent<Collider2D>();
        if (senal != null) senal.SetActive(false);

        // Si ya la completé antes, no debe aparecer
        if (idZona != "" && Escenas.EstaCompletada(idZona))
            gameObject.SetActive(false);
    }

    void Update()
    {
        bool herramientaCerca = HayHerramientaCorrecta();

        if (senal != null && senal.activeSelf != herramientaCerca)
            senal.SetActive(herramientaCerca);

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
        if (escenaDestino != "")
            Escenas.IrAEscena(escenaDestino, idZona);
        else
            gameObject.SetActive(false);
    }
}