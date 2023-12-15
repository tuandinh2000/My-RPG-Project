using RPG.Controller;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.Saving
{
    public class SavingSystem : MonoBehaviour
    {
        public IEnumerator LoadLastScene(string saveFile)
        {
            Dictionary<string, object> state = LoadData(saveFile);
            if (state.ContainsKey("lastScene"))
            {
                int buildIndex = (int)state["lastScene"];
                if (buildIndex != SceneManager.GetActiveScene().buildIndex)
                {
                    yield return SceneManager.LoadSceneAsync(buildIndex);
                }
            }
            RestoreSaveableEntityState(state);
        }

        public void SaveAction(string dataName)
        {
            Dictionary<string, object> data = LoadData(dataName);

            CaptureSaveableEntityState(data); 
            SaveData(dataName, data);
        }

        public void LoadAction(string dataName)
        {
            RestoreSaveableEntityState(LoadData(dataName));
        }

        public void DeleteAction(string dataName)
        {

        }

        //save data dua theo path
        private void SaveData(string dataName, Dictionary<string, object> data)
        {
            string path = GetDefaultSavingFilesPath(dataName);

            print("Save to: " + path);

            using (FileStream stream = File.Open(path, FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, data);
            }
        }

        //lay data tu path, neu ko co tra lai cai moi
        private Dictionary<string, object> LoadData(string dataName)
        {
            string path = GetDefaultSavingFilesPath(dataName);

            if (!File.Exists(path))
            { 
                return new Dictionary<string, object>(); 
            }

            using (FileStream fileStream = File.Open(path, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                return (Dictionary<string, object>) formatter.Deserialize(fileStream);
            };
        }

        /*        private byte[] SerializeVector(Vector3 vector)
                {
                    byte[] vectorBytes = new byte[3 * 4]; //save the float type (4 bytes)

                    BitConverter.GetBytes(vector.x).CopyTo(vectorBytes, 0);
                    BitConverter.GetBytes(vector.y).CopyTo(vectorBytes, 4);
                    BitConverter.GetBytes(vector.z).CopyTo(vectorBytes, 8);

                    return vectorBytes;
                }

                private Vector3 DeserializeVector(Byte[] vectorBytes)
                {
                    Vector3 vector = new Vector3();

                    vector.x = BitConverter.ToSingle(vectorBytes, 0);
                    vector.y = BitConverter.ToSingle(vectorBytes, 4);
                    vector.z = BitConverter.ToSingle(vectorBytes, 8);

                    return vector;
                }*/

        //save + update them data moi vao dictionary
        private void CaptureSaveableEntityState(Dictionary<string, object> data)
        {
            foreach (SaveableEntity temp in FindObjectsOfType<SaveableEntity>())
            {
                data[temp.GetUniqueID()] = temp.CaptureISaveableState();
            }
        }

        //restore data da luu, do co nhieu scene nen can dk check
        private void RestoreSaveableEntityState(Dictionary<string, object> data)
        {
            foreach (SaveableEntity temp in FindObjectsOfType<SaveableEntity>())
            {
                string id = temp.GetUniqueID();

                if (data.ContainsKey(id))
                {
                    temp.RestoreISaveableState(data[temp.GetUniqueID()]);
                }
            }
        }

        private string GetDefaultSavingFilesPath(string data)
        {
            return Path.Combine(Application.persistentDataPath, data);
        }
    }
}
