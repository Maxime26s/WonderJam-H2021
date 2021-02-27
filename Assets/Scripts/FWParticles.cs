using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FWParticles : MonoBehaviour
{
    public ParticleSystem ps;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Color[] colors = { Color.green, Color.red, Color.white, Color.blue, Color.yellow, Color.magenta };
        // initialize an array the size of our current particle count
        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[ps.particleCount];
        // *pass* this array to GetParticles...
        int num = ps.GetParticles(particles);

        for (int i = 0; i < num; i++)
        {

            particles[i].color = colors[Random.Range(0, 5)];
        }
        // re-assign modified array
        ps.SetParticles(particles, num);
    }
}
