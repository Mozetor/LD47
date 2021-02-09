using System;
using System.Collections.Generic;
using UnityEngine;

namespace Enemies {
    [Serializable]
    public struct Path {
        public string name;
        public List<Vector3> pathPoints;

        public Vector3 this[int index] => pathPoints[index];
    }
}