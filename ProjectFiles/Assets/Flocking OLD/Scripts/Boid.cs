using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// The Boid class
// adapted from https://processing.org/examples/flocking.html



public class Boid {

    public Vector2 location;
    public Vector2 velocity;
    Vector2 acceleration;
    float size;        // size of boid
    float viewSize;    // size of (square) view
    float maxForce;    // Maximum steering force
    float maxSpeed;    // Maximum speed
    float separationWeight;
    float alignmentWeight;
    float cohesionWeight;
    Main main;

    public Boid(float x, float y, Main _main) {
        main = _main;
        acceleration = new Vector2(0, 0);

        // This is a new Vector2 method not yet implemented in JS
        // velocity = Vector2.random2D();

        // Leaving the code temporarily this way so that this example runs in JS
        float angle = Random.Range(0,Mathf.PI*2);
        velocity = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

        location = new Vector2(x,y);
    }

    public void Update(List<Boid> boids) {

        viewSize = main.areaSize;
        size = main.cowSize;
        maxSpeed = main.maxSpeed;
        maxForce = main.maxForce;
        separationWeight = main.separationWeight;
        alignmentWeight = main.alignmentWeight;
        cohesionWeight = main.cohesionWeight;

        Flock(boids);
        UpdateLocation();
        Borders();
    }

    void ApplyForce(Vector2 force) {
        // We could add mass here if we want A = F / M
        acceleration += force;
    }

    // We accumulate a new acceleration each time based on three rules
    void Flock(List<Boid> boids) {
        Vector2 separationForce = Separate(boids);   // Separation
        Vector2 alignmentForce  = Align(boids);      // Alignment
        Vector2 cohesionForce   = Cohesion(boids);   // Cohesion
        // Arbitrarily weight these forces
        separationForce *= separationWeight;
        alignmentForce *= alignmentWeight;
        cohesionForce *= cohesionWeight;
        // Add the force vectors to acceleration
        ApplyForce(separationForce);
        ApplyForce(alignmentForce);
        ApplyForce(cohesionForce);
    }

    // Method to update location
    void UpdateLocation() {
        // Update velocity
        velocity += acceleration;
        // Limit speed
        velocity = Limit(velocity, maxSpeed);
        location += velocity;
        // Reset accelertion to 0 each cycle
        acceleration *= 0;
    }

    // A method that calculates and applies a steering force towards a target
    // STEER = DESIRED MINUS VELOCITY
    Vector2 Seek(Vector2 target) {
        Vector2 desired = target -location;  // A vector pointing from the location to the target
        // Scale to maximum speed
        desired.Normalize();
        desired *= maxSpeed;

        // Above two lines of code below could be condensed with new Vector2 setMag() method
        // Not using this method until Processing.js catches up
        // desired.setMag(maxspeed);

        // Steering = Desired minus Velocity
        Vector2 steer = desired - velocity;
        steer = Limit(steer,maxForce);  // Limit to maximum steering force
        return steer;
    }

    // Wraparound
    void Borders() {
        if (location.x < -size) location.x = viewSize+size;
        if (location.y < -size) location.y = viewSize+size;
        if (location.x > viewSize+size) location.x = -size;
        if (location.y > viewSize+size) location.y = -size;
    }

    // Separation
    // Method checks for nearby boids and steers away
    Vector2 Separate (List<Boid> boids) {
        float desiredseparation = size;
        Vector2 steer = new Vector2(0, 0);
        int count = 0;
        // For every boid in the system, check if it's too close
        foreach (var other in boids) {
            //            if ( other == this ) continue;
            Vector2 diff = location - other.location;
            float d = Vector2.SqrMagnitude(diff);
            // If the distance is greater than 0 and less than an arbitrary amount (0 when you are yourself)
            if (d > 0 && d < desiredseparation*desiredseparation) {
                // Calculate vector pointing away from neighbor
                diff /= d;        // Weight by distance
                steer += diff;
                count++;            // Keep track of how many
            }
        }
        // Average -- divide by how many
        if (count > 0) {
            steer /= (float)count;
        }

        // As long as the vector is greater than 0
        if (steer.sqrMagnitude > 0) {
            // First two lines of code below could be condensed with new Vector2 setMag() method
            // Not using this method until Processing.js catches up
            // steer.setMag(maxspeed);

            // Implement Reynolds: Steering = Desired - Velocity
            steer.Normalize();
            steer *= maxSpeed;
            steer -= velocity;
            steer = Limit(steer, maxForce);
        }
        return steer;
    }

    // Alignment
    // For every nearby boid in the system, calculate the average velocity
    Vector2 Align (List<Boid> boids) {
        float neighbordist = 50;
        Vector2 sum = new Vector2(0, 0);
        int count = 0;
        foreach (var other in boids) {
            float d = Vector2.Distance(location, other.location);
            if ((d > 0) && (d < neighbordist)) {
                sum += other.velocity;
                count++;
            }
        }
        if (count > 0) {
            sum /= (float)count;
            // First two lines of code below could be condensed with new Vector2 setMag() method
            // Not using this method until Processing.js catches up
            // sum.setMag(maxspeed);

            // Implement Reynolds: Steering = Desired - Velocity
            sum.Normalize();
            sum *= maxSpeed;
            Vector2 steer = sum - velocity;
            steer = Limit(steer,maxForce);
            return steer;
        } 
        else {
            return new Vector2(0, 0);
        }
    }

    // Cohesion
    // For the average location (i.e. center) of all nearby boids, calculate steering vector towards that location
    Vector2 Cohesion (List<Boid> boids) {
        float neighbordist = 50;
        Vector2 sum = new Vector2(0, 0);   // Start with empty vector to accumulate all locations
        int count = 0;
        foreach (var other in boids) {
            float d = Vector2.Distance(location, other.location);
            if ((d > 0) && (d < neighbordist)) {
                sum += other.location; // Add location
                count++;
            }
        }
        if (count > 0) {
            sum /= count;
            return Seek(sum);  // Steer towards the location
        } 
        else {
            return new Vector2(0, 0);
        }
    }

    Vector2 Limit ( Vector2 vector, float max ) {
        if ( vector.sqrMagnitude > max*max )
            return vector.normalized*max;
        else
            return vector;
    }
}
