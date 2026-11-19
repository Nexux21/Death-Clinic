using UnityEngine;

public class ZonaOperacion : MonoBehaviour
{
    [SerializeField] private GameObject senal;
    [SerializeField] private KeyCode tecla = KeyCode.E;
    [Tooltip("Déjalo vacío para aceptar cualquier herramienta")]
    [SerializeField] private string herramientaRequerida = "";

    [Header("Cambio de escena")]
    [Tooltip("Nombre exacto de la escena a la que te lleva")]
    [SerializeField] private string escenaDestino = "";
    [Tooltip("Identificador único (si lo dejas vacío usa el nombre del objeto)")]
    [SerializeField] private string idZona = "";

    private Collider2D zonaCol;
    private string IdFinal => idZona != "" ? idZona : gameObject.name;

    void Awake()
    {
        zonaCol = GetComponent<Collider2D>();
        if (senal != null) senal.SetActive(false);

        // Si ya la completé antes, no debe aparecer al volver
        if (Escenas.EstaCompletada(IdFinal))
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
        if (escenaDestino == "")
        {
            Debug.LogError("A " + gameObject.name +
                           " le falta el nombre de la Escena Destino en el Inspector.");
            return;
        }

        Escenas.IrAEscena(escenaDestino, IdFinal);
    }
}