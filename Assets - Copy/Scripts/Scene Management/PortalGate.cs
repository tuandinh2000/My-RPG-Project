using RPG.Controller;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement
{
    public class PortalGate : MonoBehaviour
    {
        [SerializeField] 
        private int loadScene = -1;
        [SerializeField] 
        private Transform spawnPoint;
        [SerializeField]
        private float fadeInTime;
        [SerializeField]
        private float fadeOutTime;

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<PlayerController>())
            {
                StartCoroutine(ActivePortalGate());
            }
        }

        private IEnumerator ActivePortalGate()
        {
            DontDestroyOnLoad(gameObject);
            var fader = FindObjectOfType<Fader>();

            yield return fader.FadeOut(fadeOutTime);
            yield return SceneManager.LoadSceneAsync(loadScene);

            var otherPortal = GetOtherPortal();
            UpdatePlayer(otherPortal);

            yield return new WaitForSeconds(0.5f);
            yield return fader.FadeIn(fadeInTime);

            Destroy(gameObject);
        }

        private void UpdatePlayer(PortalGate otherPortal)
        {
            GameObject player = FindAnyObjectByType<PlayerController>().gameObject;
            player.transform.position = otherPortal.spawnPoint.position;
            player.transform.rotation = otherPortal.spawnPoint.rotation;
        }

        private PortalGate GetOtherPortal()
        {
            foreach (PortalGate portal in FindObjectsOfType<PortalGate>())
            {
                if (portal == this) continue;

                return portal;
            }

            return null;
        }
    }
}
