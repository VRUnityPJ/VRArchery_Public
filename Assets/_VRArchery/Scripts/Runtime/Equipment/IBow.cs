using UnityEngine;

namespace _VRArchery.Scripts.Runtime.Equipment
{
    public interface IBow
    {
        public GameObject GetWirePointObject();
        public void ResetWirePointObject();
    }
}
