using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour
{
    public int speed = 100;
    public Transform mainJoint;
    public GameObject prefabJoint;
    public GameObject prefabGlow;
    public GameObject green;
    public GameObject red;
    public KeyCode[] combo;
    public int currentIndex = 0;
    public Camera cam;
    

    private Transform selectedJoint;
    private Transform lastChain;
    private GameObject lastGlow;

    private Transform[] all;
    private List<GameObject> dots = new List<GameObject>();
    private List<Transform> joints = new List<Transform>();

    private int jointNumber = 1;
    private int score = 0;
    private float lvl = 1;
    private float lastLvl = 1;
    private float lvlScale;
    private float length = -0.8165858f;
    private float xMaxExclusive = 0.2f;
    private float xMinExclusive = -0.2f;
    private float yMaxExclusive = 0.2f;
    private float yMinExclusive = -0.2f;
    private float yMin = -0.8f;
    private float yMax = 0.8f;
    private float xMin = -0.8f;
    private float xMax = 0.8f;
    private float startTime = 0;
    private float rightTime = 0;
    private float spaceTime = 0;


    private bool lastX = true;
    private bool lastY = true;
    private bool rightArrow = false;
    private bool space = false;
    private bool rightTimer = false;
    private bool spaceTimer = false;
    private bool start = false;




    // Use this for initialization
    void Start()
    {
        lvlScale = lvl / 15;
        mainJoint.transform.eulerAngles = new Vector3(0, 0, 0);
        all = mainJoint.GetComponentsInChildren<Transform>();
        selectedJoint = all[1].transform;
        foreach (Transform a in all)
        {
            joints.Add(a);
        }
        lastChain = joints[joints.Count - 2];

        GameObject glow = Instantiate(prefabGlow, selectedJoint.position, Quaternion.identity);
        lastGlow = glow;
        spwaner();
    }

    // Update is called once per frame
    void Update()
    {

        if (lvl > lastLvl)
        {
            startTime += Time.deltaTime;

            if (startTime > 3)
            {
                lastLvl++;
                start = true;
                startTime = 0;
            }
        }

       // if (start)
        //{
            if ((rightTimer && space) || (spaceTimer && rightArrow))
            {
                rotate(false);
            }
            else if (rightArrow && !spaceTimer)
            {
                rotate(true);
            }
            else if (Input.GetKeyUp(KeyCode.Space))
            {
                nextJoint();
            }
        //}

        if (Input.GetKey("space"))
        {
            space = true;
            spaceTimer = true;
        }
        else
        {
            space = false;
            spaceTimer = false;
        }

        if (Input.GetKey("right"))
        {
            rightArrow = true;
            rightTimer = true;
        }
        else
        {
            rightArrow = false;
            rightTimer = false;
        }


        if (spaceTimer)
        {
            spaceTime += Time.deltaTime;
            if(spaceTime >= 0.5f)
            {
                spaceTimer = false;
                spaceTime = 0;
            }
        }

        if (rightTimer)
        {
            rightTime += Time.deltaTime;
            if (rightTime >= 0.5f)
            {
                rightTimer = false;
                rightTime = 0;
            }
        }
    }

    public void rotate(bool direction)
    {
        if (direction == false)
        {
            selectedJoint.Rotate(Vector3.back * speed * Time.deltaTime);
        }
        else
        {
            selectedJoint.Rotate(Vector3.forward * speed * Time.deltaTime);
        }
    }

    public void addJoint()
    {
        score++;
        lvlScale = lvl / 15;
        cam.orthographicSize += 0.2825692f;
        foreach (GameObject a in dots)
            Destroy(a);

        spwaner();

        lastChain.tag = "Chain";

        all = mainJoint.GetComponentsInChildren<Transform>();
        Transform lastEmptyJoint = all[all.Length - 1];
        foreach (Transform a in all)
        {
            a.eulerAngles = new Vector3(0, 0, 0);
        }
        lastGlow.transform.position = new Vector3(0, 0, 0);
        selectedJoint = joints[1];
        jointNumber = 1;

        GameObject gb = Instantiate(prefabJoint, new Vector3(0, length, 0), Quaternion.identity);
        length -= 0.2852384f;

        lastChain = gb.transform.GetChild(0);
        joints.Add(gb.transform);
        gb.transform.SetParent(lastEmptyJoint);
        lvl++;
        start = false;
    }

    public void nextJoint()
    {
        
        all = mainJoint.GetComponentsInChildren<Transform>();
        jointNumber = jointNumber + 3;
        if (jointNumber > all.Length - 2)
        {
            jointNumber = 1;
        }
        selectedJoint = all[jointNumber];

        Destroy(lastGlow.gameObject);
        GameObject glow = Instantiate(prefabGlow, selectedJoint.position, Quaternion.identity);
        lastGlow = glow;
    }

    public float ySpwaner()
    {
        var excluded = yMaxExclusive - yMinExclusive * lvlScale;
        var newMax = -1 * length - excluded;
        var outcome = Random.Range(length, newMax);

        var res = outcome > yMinExclusive * lvlScale ? outcome + excluded : outcome;
        if (outcome < length)
        {
            if (lastY == true)
            {
                PosNegCheck(res, false);
                return res;
            }
            else
            {
                PosNegCheck(-1f * res, false);
                return -1f * res;
            }
        }
        else if (outcome > 0)
        {

            if (lastY == true)
            {
                PosNegCheck(outcome + length, false);
                return outcome + length;
            }
            else
            {
                PosNegCheck(-1f * (outcome + length), false);
                return -1f * (outcome + length);
            }

        }
        else
        {
            if (lastY == true)
            {
                PosNegCheck(outcome - length, false);
                return outcome - length;
            }
            else
            {
                PosNegCheck(-1f * (outcome - length), false);
                return -1f * (outcome - length);
            }
        }
    }

    public float xSpwaner()
    {

        var excluded = xMaxExclusive - xMinExclusive;
        var newMax = -1 * length - excluded;
        var outcome = Random.Range(xMin, newMax);
        float res = outcome > xMinExclusive ? outcome + excluded : outcome;
        //if ((outcome < 0 && outcome > length) ||  ( outcome > 0 && outcome < -1 * length)){
            if (lastX == true)
            {
                PosNegCheck(res, true);
                
            }
            else
            {
                PosNegCheck(-1f * res, true);
                res = -1f * res;
            }
      //  }

        return res;
    }

    public bool PosNegCheck(float number, bool xy)
    {
        if (number < 0)
        {
            if (xy == true)
            {
                lastX = false;
            }
            else
            {
                lastY = false;
            }
            return false;
        }
        else
        {
            if (xy == true)
            {
                lastX = true;

            }
            else
            {
                lastY = true;
            }
            return true;
        }
    }

    public void spwaner()
    {
        
        for (var i = 0; i < lvl * 3; i++)
        {
            
            GameObject gb = Instantiate(red, new Vector3(xSpwaner(), ySpwaner(), 0), Quaternion.identity);

            dots.Add(gb);
        }

        var x = xSpwaner();
        var y = ySpwaner();

        if(x < -1 * length)
        {
            x /= 2;
        }

        if (y < -1 * length)
        {
            y /= 2;
        }
       GameObject go =  Instantiate(green, new Vector3(x, y, 0), Quaternion.identity);
        dots.Add(go);
    }

    public int displayScore()
    {
        return score;
    }


        
}
