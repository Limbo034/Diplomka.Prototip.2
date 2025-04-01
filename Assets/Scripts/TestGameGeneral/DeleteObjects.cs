using UnityEngine;

namespace Scripts.GameOneControllers.SceneLogicOne.DeleteObjects
{
    public class DeleteObjects : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Metior") || other.CompareTag("Cookie") || other.CompareTag("Enemy"))
            {
                Destroy(other.gameObject);
            }
            if (other.CompareTag("EnemyBlue") || other.CompareTag("EnemyRed") || other.CompareTag("EnemyGreen"))
            {
                Destroy(other.gameObject);
            }
            if (other.CompareTag("BonusGreen") || other.CompareTag("BonusRed") || other.CompareTag("BonusBlue"))
            {
                Destroy(other.gameObject);
            }
        }
    }
}

