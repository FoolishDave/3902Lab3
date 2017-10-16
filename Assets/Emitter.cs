using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Emitter : MonoBehaviour {

    public enum ParticleShape { Sphere, Cube }
    public enum EmitterArea { Point, Sphere, Box }

    public EmitterArea EmitterShape;
    public Rect EmitterRect;
    public float EmitterRad;
    public Vector3 EmitterDirection;
    public float DirectionVariation;
    public float EmissionRate;

    public struct ParticleInfo {
        public float TimeAlive;
        public float Lifespan;

		public ParticleShape Shape;
		public float AverageLifespan;
		public float LifespanVariation;
		public bool KillOnCollision;
		public bool UseGravity;

		public bool ScaleOverLifespan;
		public Vector3 StartScale;
        public Vector3 EndScale;

		public bool ColorChange;
		public Color StartColor;
		public Color EndColor;
    }

    public ParticleInfo Info;

    private float lastEmission;
    private GameObject particleParent;

    void Start()
    {
        particleParent = GameObject.Find("Particles");
    }

    // Update is called once per frame
    void FixedUpdate () {
        if (lastEmission > EmissionRate)
        {
            SpawnParticle();
        }

        lastEmission += Time.fixedDeltaTime;
	}

    private void SpawnParticle()
    {
        GameObject newParticle;
        switch (Info.Shape)
        {
            case ParticleShape.Cube:
                newParticle = GameObject.CreatePrimitive(PrimitiveType.Cube);
                break;
            case ParticleShape.Sphere:
                newParticle = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                break;
            default:
                break;
        }
    }

}

[CustomEditor(typeof(Emitter))]
public class EmitterEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Emitter script = (Emitter)target;

        script.EmitterShape = (Emitter.EmitterArea)EditorGUILayout.EnumPopup("Emitter Shape: ", script.EmitterShape);
        if (script.EmitterShape == Emitter.EmitterArea.Box)
        {
            EditorGUI.indentLevel++;
            script.EmitterRect = EditorGUILayout.RectField("Emitter Rect: ", script.EmitterRect);
            EditorGUI.indentLevel--;
        }

        if (script.EmitterShape == Emitter.EmitterArea.Sphere)
        {
            EditorGUI.indentLevel++;
            script.EmitterRad = EditorGUILayout.FloatField("Emitter Radius: ", script.EmitterRad);
            EditorGUI.indentLevel--;
        }

        EditorGUILayout.Separator();
        EditorGUILayout.LabelField("Particle Information");
        EditorGUI.indentLevel++;
        script.Info.Shape = (Emitter.ParticleShape)EditorGUILayout.EnumPopup("Shape: ", script.Info.Shape);
        script.Info.AverageLifespan = EditorGUILayout.FloatField("Average Lifespan: ", script.Info.AverageLifespan);
        script.Info.LifespanVariation = EditorGUILayout.FloatField("Lifespan Variation: ", script.Info.LifespanVariation);
        script.Info.KillOnCollision = EditorGUILayout.Toggle("Kill On Collision: ",script.Info.KillOnCollision);
        script.Info.UseGravity = EditorGUILayout.Toggle("Affected By Gravity: ", script.Info.UseGravity);
		script.Info.ScaleOverLifespan = EditorGUILayout.Toggle("Scale Over Lifespan: ", script.Info.ScaleOverLifespan);
		if (script.Info.ScaleOverLifespan)
        {
            EditorGUI.indentLevel++;
            script.Info.StartScale = EditorGUILayout.Vector3Field("Start Scale: ", script.Info.StartScale);
            script.Info.EndScale = EditorGUILayout.Vector3Field("End Scale: ", script.Info.EndScale);
            EditorGUI.indentLevel--;
        }
        script.Info.ColorChange = EditorGUILayout.Toggle("Change Color: ", script.Info.ColorChange);
        if (script.Info.ColorChange)
        {
            EditorGUI.indentLevel++;
            script.Info.StartColor = EditorGUILayout.ColorField("Start Color: ", script.Info.StartColor);
            script.Info.EndColor = EditorGUILayout.ColorField("End Color: ", script.Info.EndColor);
            EditorGUI.indentLevel--;
        }
        EditorGUI.indentLevel--;
    }
}
