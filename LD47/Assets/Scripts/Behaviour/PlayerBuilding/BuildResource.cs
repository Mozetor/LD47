using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Economy {
    [System.Serializable]
    public class BuildResource {
        /// <summary> Amount of resources </summary>
        public int resourceAmount;
        /// <summary> Type of resource </summary>
        public BuildResourceType resourceType;
    }
}
