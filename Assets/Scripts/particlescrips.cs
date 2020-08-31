using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particlescrips : MonoBehaviour
{
    // Start is called before the first frame update
    //这个函数是粒子效果

    public static particlescrips particl;

    private void Awake()
    {
        if(particl!=null)
        {
            Debug.LogError("particlescrips multiple");
        }
        particl = this;

    }


    public ParticleSystem instantiate(ParticleSystem prefab, Vector3 position)
    {
        ParticleSystem newparticlsystem = Instantiate(prefab, position, Quaternion.identity) as ParticleSystem;
        Destroy(newparticlsystem.gameObject, newparticlsystem.startLifetime);
        return newparticlsystem;
    }
}
