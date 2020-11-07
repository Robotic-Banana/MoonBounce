using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBoardTowardMainCamera : MonoBehaviour {
    // Update is called once per frame
    void Update () {
        this.gameObject.transform.LookAt (Camera.main.transform.position);
        transform.RotateAround (transform.position, transform.up, 180);
        transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles.x, transform.localRotation.eulerAngles.y, 0);
    }
}