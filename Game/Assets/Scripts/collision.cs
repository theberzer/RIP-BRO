using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class collision : MonoBehaviour {
    public GameObject canvas;
    public AudioClip clip;
    private bool rip = false;



    private void Update()
    {
        if (Input.GetKey(KeyCode.Space) && rip)
        {
            SceneManager.LoadScene("ripbro");
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if(this.gameObject.tag == "Green")
        {
            if (col.gameObject.tag == "LastChain")
            {
                GameObject.Find("RootSphere").GetComponent<movement>().addJoint();
                GameObject.Find("CameraObject").GetComponent<AudioSource>().Play();

                Destroy(this.gameObject);
            }
        } else if(this.gameObject.tag == "Red")
        {
            GameObject.Find("RipSound").GetComponent<AudioSource>().Play();
            GameObject textCanvas = Instantiate(canvas, new Vector3(0, 0, 0), Quaternion.identity);

            Text text = textCanvas.transform.GetChild(0).GetComponent<Text>();
            int Score = GameObject.Find("RootSphere").GetComponent<movement>().displayScore();
            text.text = "RIP BRO \n" + "Score: " + Score.ToString();

            Destroy(GameObject.Find("RootSphere").gameObject);
            rip = true;
        }

      

    }
}
