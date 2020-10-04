using UnityEngine;

public abstract class Projectile : MonoBehaviour {
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

    protected abstract bool IsValidCollision(Collision2D collision);

    protected abstract void OnValidCollision(Collision2D collision);

    private void OnCollisionEnter2D(Collision2D collision) {
        if (IsValidCollision(collision)) {
            OnValidCollision(collision);
            Destroy(this.gameObject);
        }
    }
}
