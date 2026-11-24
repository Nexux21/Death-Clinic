using UnityEngine;

public class Circulos : MonoBehaviour
{
    [SerializeField] private float tiempoAparicion = 0.15f;
    [Tooltip("Tamaño final al acabarse su vida (1 = no se encoge)")]
    [SerializeField] private float escalaFinal = 0.4f;

    private float vida;
    private float tiempo;
    private Vector3 escalaBase;
    private bool golpeado;

    public void Iniciar(float duracion)
    {
        vida = duracion;
        escalaBase = transform.localScale;
        transform.localScale = Vector3.zero;
    }

    void Update()
    {
        tiempo += Time.deltaTime;

        if (tiempo < tiempoAparicion)
        {
            // Aparece creciendo
            transform.localScale = escalaBase * (tiempo / tiempoAparicion);
        }
        else
        {
            // Se va encogiendo hasta desaparecer
            float p = Mathf.Clamp01((tiempo - tiempoAparicion) / (vida - tiempoAparicion));
            transform.localScale = escalaBase * Mathf.Lerp(1f, escalaFinal, p);

            if (p >= 1f)
                Destroy(gameObject);
        }
    }

    // Devuelve true si el golpe cuenta (evita contar dos veces el mismo círculo)
    public bool Golpear()
    {
        if (golpeado) return false;
        golpeado = true;
        Destroy(gameObject);
        return true;
    }
}