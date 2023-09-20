using UnityEngine;

public class GeneradorPelotas : MonoBehaviour
{
    public GameObject pelotaPrefab;
    public float tiempoEntreCreaciones = 2.0f;
    public float minX = -5.0f;
    public float maxX = 5.0f;
    public float posY = 236.0f;

    private float tiempoUltimaCreacion;
    private bool juegoEnCurso = true;
    private GameObject[] pelotasGeneradas;

    private Jugador jugador;

    private void Start()
    {
        tiempoUltimaCreacion = Time.time;
        jugador = FindObjectOfType<Jugador>();
    }

    private void Update()
    {
        if (juegoEnCurso && Time.time - tiempoUltimaCreacion > tiempoEntreCreaciones)
        {
            CrearPelota();
            tiempoUltimaCreacion = Time.time;
        }
    }

    private void CrearPelota()
    {
        float posX = Random.Range(minX, maxX);

        GameObject nuevaPelota = Instantiate(pelotaPrefab, new Vector3(posX, posY, 0), Quaternion.identity);
        AgregarPelotaGenerada(nuevaPelota);
    }

    public void IniciarJuego()
    {
        juegoEnCurso = true;
        tiempoUltimaCreacion = Time.time;
        DestruirPelotasGeneradas();
    }

    public void DetenerJuego()
    {
        juegoEnCurso = false;
        tiempoUltimaCreacion = Time.time;
        DestruirPelotasGeneradas();
    }

    private void AgregarPelotaGenerada(GameObject pelota)
    {
        if (pelotasGeneradas == null)
        {
            pelotasGeneradas = new GameObject[] { pelota };
        }
        else
        {
            GameObject[] newArray = new GameObject[pelotasGeneradas.Length + 1];
            pelotasGeneradas.CopyTo(newArray, 0);
            newArray[pelotasGeneradas.Length] = pelota;
            pelotasGeneradas = newArray;
        }
    }

    public void DestruirPelotasGeneradas()
    {
        if (pelotasGeneradas != null)
        {
            foreach (GameObject pelota in pelotasGeneradas)
            {
                Destroy(pelota);
            }
            pelotasGeneradas = null;
        }
    }
}
