using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleBehaviour : MonoBehaviour {

    public Emitter.ParticleInfo SelfInfo;
    private Material mat;

	// Use this for initialization
	void Start () {
        mat = GetComponent<Renderer>().material;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        float aliveDec = SelfInfo.TimeAlive / SelfInfo.Lifespan;
        if (SelfInfo.ScaleOverLifespan)
            transform.localScale = Vector3.Lerp(transform.localScale, SelfInfo.EndScale, aliveDec);

        if (SelfInfo.ColorChange)
            mat.color = Color.Lerp(mat.color, SelfInfo.EndColor, aliveDec);
        SelfInfo.TimeAlive += Time.fixedDeltaTime;
        if (SelfInfo.TimeAlive > SelfInfo.Lifespan)
            Destroy(gameObject);
	}
}
