using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Saving
{
    [ExecuteAlways]
    public class SaveableEntity : MonoBehaviour
    {
        static Dictionary<string, SaveableEntity> uniqueIDs = new Dictionary<string, SaveableEntity>();

        [SerializeField]
        private string uniqueID = "";

#if UNITY_EDITOR
        private void Update()
        {
            //check if the saveable object is in play mode or in hierarchy
            //phong ngua id thay doi khi chuyen hay load scene, hay khi spawn prefabs
            if (Application.IsPlaying(gameObject)) return;
            if (string.IsNullOrEmpty(gameObject.scene.path)) return;

            //tao id xong luu vao sandbox unity
            SerializedObject serializedObject = new SerializedObject(this);
            SerializedProperty property = serializedObject.FindProperty("uniqueID");

            if (string.IsNullOrEmpty(property.stringValue) || !IsUnique(property.stringValue))
            {
                property.stringValue = System.Guid.NewGuid().ToString();
                serializedObject.ApplyModifiedProperties();
            }

            uniqueIDs[property.stringValue] = this;
        }
#endif

        private bool IsUnique(string id)
        {
            if (!uniqueIDs.ContainsKey(id)) return true;

            if (uniqueIDs[id] == this) return true;

            if (uniqueIDs[id] == null)
            {
                uniqueIDs.Remove(id);
                return true;
            }

            if (uniqueIDs[id].GetUniqueID() != id)
            {
                uniqueIDs.Remove(id);
                return true;
            }

            return false;
        }

        public string GetUniqueID()
        {
            return uniqueID;
        }

        public object CaptureISaveableState()
        {
            Dictionary<string, object> saveableObjects = new Dictionary<string, object>();

            foreach (ISaveable temp in GetComponents<ISaveable>())
            {
                saveableObjects[temp.GetType().Name] = temp.CaptureState();
            }

            return saveableObjects;
        }

        public void RestoreISaveableState(object state)
        {
            Dictionary<string, object> saveableObjects = state as Dictionary<string, object>;

            foreach (ISaveable temp in GetComponents<ISaveable>())
            {
                string type = temp.GetType().Name;

                if (saveableObjects.ContainsKey(type)) {
                    temp.RestoreState(saveableObjects[type]);
                }
            }
        }
    }
}
