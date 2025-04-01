using System.Collections;
using UnityEngine;

namespace Scripts.SpawnObject
{
    public abstract class AbsSpawn : MonoBehaviour
    {
        protected float spawnInterval = 2f;

        protected IEnumerator SpawnObjectCoroutine()
        {
            while (true)
            {
                SpawnRegion();
                yield return new WaitForSeconds(spawnInterval);
            }
        }

        protected abstract void SpawnRegion();
    }
}
