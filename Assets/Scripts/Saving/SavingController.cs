using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Saving
{
    public class SavingController : MonoBehaviour
    {
        const string defaultSavingPath = "save";

        private IEnumerator Start()
        {
            yield return GetComponent<SavingSystem>().LoadLastScene(defaultSavingPath);
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                if (Input.GetKeyDown(KeyCode.S)) {
                    Save();
                }

                if (Input.GetKeyDown(KeyCode.L))
                {
                    Load();
                }

                if (Input.GetKeyDown(KeyCode.D))
                {
                    Delete();
                }
            }
        }

        public void Save()
        {
            GetComponent<SavingSystem>().SaveAction(defaultSavingPath);
        }

        public void Load()
        {
            GetComponent<SavingSystem>().LoadAction(defaultSavingPath);
        }

        public void Delete()
        {
            GetComponent<SavingSystem>().DeleteAction(defaultSavingPath);
        }
    }
}

