using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour {

    // adapted from https://processing.org/examples/flocking.html

    public int numberOfCows = 100;
    public float areaSize = 100;
    public float cowSize = 10;
    public float maxSpeed = 2f;
    public float maxForce = 0.1f;
    public float separationWeight = 2.5f;
    public float alignmentWeight = 1;
    public float cohesionWeight = 1;

    public Camera cam;

    Flock flock;

	// Use this for initialization
	void Start () {
        
        flock = new Flock();
        for ( int i = 0; i < numberOfCows; i++ ) {
            var cow = new Boid(areaSize/2,areaSize/2, this);
            var cowObject = Instantiate(Resources.Load("cow")) as GameObject;
            cowObject.GetComponent<MoveCow>().cowBoid = cow;
            flock.AddBoid(cow);
        }

        cam.orthographicSize = areaSize/2;
        cam.transform.position = new Vector3 ( areaSize/2, 100, areaSize/2 );

	}
	
	// Update is called once per frame
    void Update () {
        flock.Update();
	}

//    void mousePressed() {
//        flock.AddBoid(new Boid(Input.mousePosition.x,Input.mousePosition.y));
//    }

}