using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class LaneItem : MonoBehaviour
    {
        [SerializeField] string id;

        public string GetID()
        {
            return id;
        }
    }
}
