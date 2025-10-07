using UnityEngine;

public class HydrantHit : MonoBehaviour
{
    [SerializeField] private ParticleSystem waterEffect;

    void OnCollisionEnter(Collision otherObject)
    {
        if (otherObject.gameObject.tag == "LawnMower")
        {
            if (waterEffect != null)
            {
                waterEffect.Play();
            }
        }
    }
}
