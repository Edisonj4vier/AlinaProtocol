using UnityEngine;
using UnityEngine.Events;

public class CharacterEvents
{
    // daño recibido y caracteres dañados
    public static UnityAction<GameObject, int> characterDamaged;
    // curación recibida y caracteres curados
    public static UnityAction<GameObject, int> characterHealed;
    

}