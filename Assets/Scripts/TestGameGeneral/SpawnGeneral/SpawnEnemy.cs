using UnityEngine;

namespace Scripts.SpawnObject
{
    public class SpawnEnemy : AbsSpawn
    {
        [Header("Enemy spawn locations")]
        [SerializeField] private Transform[] pointsTransform;
        [Header("Types of enemies")]
        [SerializeField] private GameObject[] enemiesType;
        [Header("Enemy spawn Intervals")]
        [SerializeField] private float interval;

        private int spawnedEnemiesCount = 0;

        private void Start()
        {
            spawnInterval = interval;
            StartCoroutine(SpawnObjectCoroutine());
        }

        #region Spawn
        protected override void SpawnRegion()
        {
            int randPoint = Random.Range(0, pointsTransform.Length);
            int randEnemy = Random.Range(0, enemiesType.Length);

            GameObject enemy = Instantiate(enemiesType[randEnemy]);
            enemy.transform.position = pointsTransform[randPoint].position;


            spawnedEnemiesCount++;
        }
        #endregion
    }
}
