using UnityEngine;

public class Projectile : Mirror.NetworkBehaviour {
    public float destroyAfter = 5;
    public Rigidbody rigidBody;
    public float force = 1000;

    public float damage = 25;

        public override void OnStartServer() {
            Invoke(nameof(DestroySelf), destroyAfter);
        }

        // set velocity for server and client. this way we don't have to sync the
        // position, because both the server and the client simulate it.
        void Start() {
            rigidBody.AddForce(transform.forward * force);
        }

        // destroy for everyone on the server
        [Mirror.Server]
        void DestroySelf() {
            Mirror.NetworkServer.Destroy(gameObject);
        }

        // ServerCallback because we don't want a warning if OnTriggerEnter is
        // called on the client
    [Mirror.ServerCallback]
    void OnTriggerEnter(Collider co) {
        Mirror.NetworkServer.Destroy(gameObject);

        Health hp = co.gameObject.GetComponent<Health>();
        if (hp != null) {
            Robot robo = co.gameObject.GetComponent<Robot>();
            //either damage or destroy the target
            if (robo != null) {
                robo.Damage(damage);
            } else {
                Destroy(co.gameObject);
            }
        }

    }
}
