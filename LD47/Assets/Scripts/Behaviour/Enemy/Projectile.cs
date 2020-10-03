using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Assets.Enemies {
    public class Projectile : MonoBehaviour {

        public float speed;
        public float lifeTime;
        public int damage;
        public Vector3 direction;
        private new Rigidbody2D rigidbody;

        private void Start() {
            rigidbody = GetComponent<Rigidbody2D>();
            this.transform.rotation = Quaternion.FromToRotation(Vector3.right, direction);
        }

        private void Update() {
            rigidbody.MovePosition(this.transform.position + direction.normalized * Time.deltaTime * speed);
            lifeTime -= Time.deltaTime;
            if (lifeTime <= 0) {
                Destroy(this.gameObject);
            }
        }

        private void OnCollisionEnter2D(Collision2D collision) {
            var city = collision.gameObject.GetComponent<CityController>();
            if (city != null) {
                city.Damage(damage);
            }
            Destroy(this.gameObject);
        }
    }
}
