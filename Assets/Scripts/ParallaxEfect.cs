using UnityEngine;

public class ParallaxEfect : MonoBehaviour
{
    public Camera cam;
    public Transform followTarget;
    
    // Posición inicial del game object parallax
    Vector2 _startingPosition;
    
    // Valor Z inicial del game object parallax
    private float _startingZ;

    // Distancia que se ha desplazado la cámara desde la posición inicial del objeto de parallax.
    private Vector2 CamMoveSinceStart => (Vector2)cam.transform.position - _startingPosition;

    // Si el objeto está delante del objetivo, utilice el plano del clip cercano. Si está detrás del objetivo, utiliza el plano de recorte lejano.
    float ClippingPlane => (cam.transform.position.z + (ZDistanceFromTarget > 0 ? cam.farClipPlane : cam.nearClipPlane));

    float ZDistanceFromTarget => transform.position.z - followTarget.position.z;
    // Cuanto más lejos esté el objeto del jugador, más rápido se moverá el objeto ParallaxEffect. Arrastra su valor Z más cerca del objetivo para que se mueva más despacio.
    private float ParallaxFactor => Mathf.Abs(ZDistanceFromTarget) / ClippingPlane;
    // Start is called before the first frame update
    void Start()
    {
        _startingPosition = transform.position;
        _startingZ = transform.position.z;
        
    }

    // Update is called once per frame
    void Update()
    {
        // Cuando el objetivo se mueve, mueve el objeto de parallax la misma distancia multiplicada por un multiplicador.
        Vector2 newPosition = _startingPosition + CamMoveSinceStart * ParallaxFactor;
        
        // La posición X/Y cambia en función de la velocidad de desplazamiento del objetivo multiplicada por el factor de parallax, pero z se mantiene constante.
        transform.position = new Vector3(newPosition.x, newPosition.y, _startingZ);
    }
}
