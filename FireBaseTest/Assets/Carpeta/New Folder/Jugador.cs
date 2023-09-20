using UnityEngine;
using TMPro;

public class Jugador : MonoBehaviour
{
    public TextMeshProUGUI puntajeText;
    public GameObject botondeReinicio;
    private int puntaje = 0;
    public float velocidadMovimiento = 5f;
    public float tiempoLimite = 60.0f;
    private float tiempoRestante;
    private bool juegoTerminado = false;

    private float tiempoInicial; // Almacena el tiempo inicial al iniciar el juego

    // Referencia al ScoreController
    private ScoreController scoreController;

    private Pelota pelota; // Referencia al objeto de la pelota

    // Referencia al GeneradorPelotas
    public GeneradorPelotas generadorPelotas;

    private void Start()
    {
        tiempoInicial = tiempoLimite;
        tiempoRestante = tiempoInicial;
        ActualizarTiempo();
        botondeReinicio.SetActive(false);

        // Encuentra y almacena la referencia al ScoreController en el inicio
        scoreController = FindObjectOfType<ScoreController>();

        // Encuentra y almacena la referencia a la pelota
        pelota = FindObjectOfType<Pelota>();
    }

    private void Update()
    {
        if (!juegoTerminado)
        {
            // Resta tiempo solo si el juego no ha terminado
            tiempoRestante -= Time.deltaTime;

            if (tiempoRestante <= 0)
            {
                tiempoRestante = 0;
                juegoTerminado = true;
                FinDeJuegoPorTiempo(); // Detiene la pelota cuando el tiempo se agota
            }

            ActualizarTiempo();

            float movimientoHorizontal = Input.GetAxis("Horizontal");
            Vector2 movimiento = new Vector2(movimientoHorizontal, 0);
            transform.Translate(movimiento * Time.deltaTime * velocidadMovimiento);
        }
        else
        {
            // Si el juego está terminado, puedes implementar lógica adicional aquí
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Pelota"))
        {
            Destroy(other.gameObject);
            puntaje += 10;
            ActualizarPuntaje();

            if (juegoTerminado)
            {
                // Si el juego ha terminado, destruye las pelotas cuando colisionan con el jugador
                Destroy(other.gameObject);
            }
        }
    }

    private void ActualizarPuntaje()
    {
        puntajeText.text = "Puntaje: " + puntaje;

        // Actualiza el puntaje en el ScoreController
        scoreController.ActualizarScore(puntaje);
    }

    private void ActualizarTiempo()
    {
        int minutos = Mathf.FloorToInt(tiempoRestante / 60);
        int segundos = Mathf.FloorToInt(tiempoRestante % 60);
        string tiempoFormateado = string.Format("{0:00}:{1:00}", minutos, segundos);

        puntajeText.text = "Puntaje: " + puntaje + " | Tiempo: " + tiempoFormateado;
    }

    private void FinDeJuegoPorTiempo()
    {
        botondeReinicio.SetActive(true);
        Debug.Log("¡Tiempo agotado! Has perdido.");

        // Detener el generador de pelotas
        generadorPelotas.DetenerJuego();
    }

    public void ReiniciarJuego()
    {
        // Restablece el tiempo al reiniciar
        tiempoRestante = tiempoInicial;

        // Reinicia el puntaje del jugador a 0
        puntaje = 0;
        ActualizarPuntaje(); // Actualiza el puntaje en la UI

        botondeReinicio.SetActive(false);

        // Iniciar el generador de pelotas al reiniciar
        generadorPelotas.IniciarJuego();

        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    // Obtener el tiempo restante (por si necesitas acceder a él desde otro script)
    public float TiempoRestante
    {
        get { return tiempoRestante; }
    }
}
