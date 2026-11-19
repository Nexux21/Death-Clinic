using UnityEngine;

public class Organo : MonoBehaviour
{
    [SerializeField] private string herramientaRequerida = "Pinzas";
    [SerializeField] private string tipoOrgano = "Corazon";
    [SerializeField] private bool volverAlInicioSiSeSuelta = true;

    // Solo un órgano en la mano a la vez
    private static Organo organoActual;

    private Collider2D col;
    private Herramienta herramientaQueLoLleva;
    private Vector3 offset;
    private Vector3 posicionInicial;

    public string TipoOrgano => tipoOrgano;
    public bool EstaAgarrado => herramientaQueLoLleva != null;
    public bool Depositado { get; private set; }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void Reiniciar()
    {
        organoActual = null;
    }

    void Awake()
    {
        col = GetComponent<Collider2D>();
        posicionInicial = transform.position;
    }

    void Update()
    {
        if (Depositado) return;

        if (!EstaAgarrado)
        {
            IntentarAgarrar();
            return;
        }

        // Si ya no tengo la herramienta en la mano, el órgano se suelta
        if (Herramienta.HerramientaActual != herramientaQueLoLleva)
        {
            Soltar();
            return;
        }

        // Clic izquierdo: intentar depositar
        if (Input.GetMouseButtonDown(0))
            IntentarDepositar();
    }

    void LateUpdate()
    {
        // Seguir a la herramienta (en LateUpdate para que no vaya un frame atrasado)
        if (EstaAgarrado && !Depositado)
            transform.position = herramientaQueLoLleva.transform.position + offset;
    }

    void IntentarAgarrar()
    {
        if (!Input.GetMouseButtonDown(0)) return;
        if (organoActual != null) return;

        Herramienta h = Herramienta.HerramientaActual;
        if (h == null) return;
        if (h.FrameAgarrada == Time.frameCount) return;

        if (herramientaRequerida != "" && h.nombreHerramienta != herramientaRequerida)
            return;

        Collider2D hCol = h.GetComponent<Collider2D>();
        if (hCol == null || !col.Distance(hCol).isOverlapped) return;

        herramientaQueLoLleva = h;
        organoActual = this;
        offset = transform.position - h.transform.position;
    }

    bool IntentarDepositar()
    {
        foreach (ZonaOrganos zona in ZonaOrganos.Todas)
        {
            if (zona.EstaSobre(col) && zona.Recibir(this))
                return true;
        }
        return false;
    }

    void Soltar()
    {
        // Antes de caerse, intenta depositarse si está sobre la bandeja
        if (IntentarDepositar()) return;

        herramientaQueLoLleva = null;
        organoActual = null;

        if (volverAlInicioSiSeSuelta)
            transform.position = posicionInicial;
    }

    // Lo llama la zona de depósito: el órgano se queda donde está
    public void Depositar()
    {
        Depositado = true;
        herramientaQueLoLleva = null;
        organoActual = null;
    }

    // Versión que además lo mueve a un punto concreto
    public void Depositar(Vector3 destino)
    {
        Depositar();
        transform.position = destino;
    }
}