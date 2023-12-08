using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class PeristentObject : MonoBehaviour
    {
        [SerializeField] 
        private GameObject spawner;

        static bool isSpawned;

        private void Awake()
        {
            if (isSpawned) return;

            Spawn();
            isSpawned = true;
        }

        private void Spawn()
        {
            GameObject persistentObject = Instantiate(spawner);
            DontDestroyOnLoad(persistentObject);
        }
    }
}
