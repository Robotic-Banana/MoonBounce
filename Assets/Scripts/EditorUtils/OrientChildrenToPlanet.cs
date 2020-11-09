using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// [CustomEditor(typeof(OrientChildrenToPlanet))]
// public class OrientChildrenToPlanetEditor : Editor {
//     OrientChildrenToPlanet script;

//     void OnEnable()
//     {
//         script = (OrientChildrenToPlanet) target;
//     }

//     public override void OnInspectorGUI() {
//         base.OnInspectorGUI();

//     }
// }

[ExecuteInEditMode]
public class OrientChildrenToPlanet : MonoBehaviour {
    public SphereCollider ourPlanet;
    private Transform centre;
    private float radius;

    public bool recalculateOrientationOfNewlyAddedChildren;

    private List<Transform> childrenTransforms;

    void OnEnable () {

        if (ourPlanet == null) {
            Debug.LogError ("Your OrientChildrenToPlanet script doesn't have an associated planet, add a planet in the inspector");
            return;

        }

        RecalculateChildren ();

        radius = ourPlanet.radius * ourPlanet.transform.localScale.y;
        centre = ourPlanet.transform;

    }

    void Update () {
        if (ourPlanet == null) {
            Debug.LogError ("Your OrientChildrenToPlanet script doesn't have an associated planet, add a planet in the inspector");
            return;

        }

        if (centre == null || recalculateOrientationOfNewlyAddedChildren) {
            RecalculateChildren ();

            radius = ourPlanet.radius * ourPlanet.transform.localScale.y;
            centre = ourPlanet.transform;

            recalculateOrientationOfNewlyAddedChildren = false;

        }

        foreach (Transform child in childrenTransforms) {
            Vector3 surfaceNormal = child.position - centre.position;
            surfaceNormal.Normalize ();

            child.rotation = Quaternion.FromToRotation (child.up, surfaceNormal) * child.rotation;

        }
    }

    private void RecalculateChildren () {
        childrenTransforms = new List<Transform> ();

        foreach (Transform child in transform) {
            childrenTransforms.Add (child);

        }
    }
}