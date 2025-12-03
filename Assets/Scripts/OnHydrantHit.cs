using UnityEngine;

public class OnHydrantHit : MonoBehaviour
{
    [SerializeField] private ParticleSystem waterEffect;

    private void OnCollisionEnter(Collision otherObject)
    {
        if (otherObject.gameObject.CompareTag("LawnMower"))
            if (waterEffect != null)
                waterEffect.Play();
    }
}