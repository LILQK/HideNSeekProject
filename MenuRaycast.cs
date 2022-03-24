using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuRaycast : MonoBehaviour
{

    private RaycastHit hits;
    private Ray beam;
    public Camera cam;

    private void FixedUpdate()
    {
        beam = cam.ScreenPointToRay(Input.mousePosition);

        if (Input.GetMouseButton(0)) {
        
            if (Physics.Raycast(beam, out hits, Mathf.Infinity))
            {
                Cursor.lockState = CursorLockMode.Locked;
                if (hits.collider.gameObject.name == "play") SceneManager.LoadScene("SampleScene");
            }
        }
    }


}
