using System;
using UnityEngine;

namespace Assets.WaveSpawner {
    /// <summary>
    /// This is the base class for any object, that is spawnable.
    /// </summary>
    public abstract class Spawnable : MonoBehaviour {
        /// <summary> The callback called on death. </summary>
        protected Action<Spawnable> onDeath;


        /// <summary>
        /// Adds a callback, that the spawnable executes on death/destruction.
        /// </summary>
        /// <param name="action"> Tha action called on death. </param>
        public void AddOnDeath(Action action) {
            onDeath += _ => action();
        }

        /// <summary>
        /// Adds a callback, that the spawnable executes on death/destruction.
        /// </summary>
        /// <param name="action"> Tha action called on death. </param>
        public void AddOnDeath(Action<Spawnable> action) {
            this.onDeath += action;
        }

        /// <summary>
        /// Sets up the behaviour, depending on the spawnpoint used.
        /// </summary>
        /// <param name="spawnIndex"> The index of the spawnpoint this entity was spawned at. </param>
        public virtual void SetUp(int spawnIndex) { }

        /// <summary>
        /// This is called when the spawnable dies.
        /// </summary>
        protected virtual void Die() {
            onDeath?.Invoke(this);
        }
    }
}
