using UnityEngine;

namespace Salems_Draw
{
    public class MoveToSpawnpoint : MonoBehaviour
    {
        public void Awake()
        {
            MoveToSpawn();
        }

        public void Start()
        {
            MoveToSpawn();
        }

        private void MoveToSpawn()
        {
            var spawnpointGO = GameObject.FindWithTag("Spawnpoint");
            var spawnpoint = spawnpointGO?.GetComponent<Spawnpoint>();
            Debug.Log($"Moving to {spawnpoint?.Position}");
            transform.root.position = spawnpoint?.Position ?? Vector3.zero;
            Debug.Log($"Now at {transform.position}");
        }
    }
}
