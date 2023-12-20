using UnityEngine;

public class HealthPickup : MonoBehaviour
{
   public int healthRestore = 10;
   public Vector3 spinRotationSpeed = new Vector3(0, 180, 0);
   private AudioSource _pickupSource;

   private void Awake()
   {
      _pickupSource = GetComponent<AudioSource>();
   }

   private void OnTriggerEnter2D(Collider2D collision)
   {
      Damageable damageable = collision.GetComponent<Damageable>();
      if (damageable)
      {
         bool wasHealed = damageable.Heal(healthRestore);

         if (wasHealed)
         {
            if (_pickupSource)
            {
               AudioSource.PlayClipAtPoint(_pickupSource.clip, gameObject.transform.position, _pickupSource.volume);
            }
            Destroy(gameObject);
         }
      }
   }

   private void Update()
   {
      transform.eulerAngles += spinRotationSpeed * Time.deltaTime;
   }
}
