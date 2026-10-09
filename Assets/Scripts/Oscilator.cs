using UnityEngine;

public class Oscilator : MonoBehaviour
{
    [SerializeField] Vector3 movementVector;
    Vector3 startPosition, endPosition;

    [SerializeField] float speed = 1f;
    float movementFactor;

    void Start()
    {
        startPosition = transform.position;
        endPosition = startPosition + movementVector;
    }

    void Update()
    {
        Oscilate();   
    }

    private void Oscilate()
    {
        // Calcula un valor que va y viene entre 0 y 1 según el tiempo transcurrido.
        // Esto hace que el objeto "oscile" de forma continua.
        movementFactor = Mathf.PingPong(Time.time * speed, 1f);

        // Mueve el objeto entre la posición inicial y la final.
        // Cuando movementFactor vale 0 -> está en startPosition.
        // Cuando vale 1 -> está en endPosition.
        // Los valores intermedios lo colocan en una posición intermedia.
        transform.position = Vector3.Lerp(startPosition, endPosition, movementFactor);
    }
}
