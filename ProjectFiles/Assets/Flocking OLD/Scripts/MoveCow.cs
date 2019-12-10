using UnityEngine;
using System.Collections;

public class MoveCow : MonoBehaviour {
    
    public float timeOffset = 0;
    public float radius = 50;
    public float speed = 1;
    public Boid cowBoid;

    // Use this for initialization
    void Start () {
    }

    // Update is called once per frame
    void Update () {
        transform.position = new Vector3( cowBoid.location.x, 0, cowBoid.location.y );
        transform.rotation = Quaternion.FromToRotation( -Vector3.right, new Vector3( cowBoid.velocity.x, 0, cowBoid.velocity.y ));  // aligns the cow with the boid's velocity
    }
}
