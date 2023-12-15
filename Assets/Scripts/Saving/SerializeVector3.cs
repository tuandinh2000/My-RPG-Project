using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Saving
{
    [System.Serializable]
    public class SerializeVector3
    {
        private float x, y, z;

        public SerializeVector3(Vector3 vector3)
        {
            this.x = vector3.x;
            this.y = vector3.y;
            this.z = vector3.z;
        }

        public Vector3 ReturnToVector3()
        {
            return new Vector3(x, y, z);
        }
    }
}
