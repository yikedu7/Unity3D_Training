using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.Manager;


public class Scence : MonoBehaviour {

    public int round { get; set; }
    public int diskCount { get; private set; }
    private float diskScale;
    private Color diskColor;
    private Vector3 startPosition;
    private Vector3 startDiretion;
    private float diskSpeed;

    List<GameObject> usingDisks;

    public void Reset(int round)
    {
        this.round = round;
        this.diskCount = round;
        this.diskScale = 1;
        this.diskSpeed = 4;
        if (round % 2 == 1)
        {
            this.diskColor = Color.red;
            this.startPosition = new Vector3(-5f, 3f, -15f);
            this.startDiretion = new Vector3(0f, 0f, 0f);
        }
        else
        {
            this.diskColor = Color.green;
            this.startPosition = new Vector3(5f, 3f, -15f);
            this.startDiretion = new Vector3(0f, 0f, 0f);
        }
        for (int i = 0; i < round; i++)
        {
            this.diskScale *= 0.8f;
            this.diskSpeed *= 1.1f;
        }
    }

    public void sendDisk(List<GameObject> usingDisks)
    {
        this.usingDisks = usingDisks;
        this.Reset(round);
        for (int i = 0; i < usingDisks.Count; i++)
        {
            var localScale = usingDisks[i].transform.localScale;
            usingDisks[i].transform.localScale = new Vector3(localScale.x*diskScale, localScale.y * diskScale, localScale.z * diskScale);
            usingDisks[i].GetComponent<Renderer>().material.color = diskColor;
            usingDisks[i].transform.position = new Vector3(startPosition.x, startPosition.y + i, startPosition.z);
            usingDisks[i].GetComponent<Rigidbody>().AddForce(startDiretion * Random.Range(diskSpeed * 5, diskSpeed * 10) / 10, ForceMode.Impulse);
        }
    }

    public void destroyDisk(GameObject disk)
    {
        disk.transform.position = new Vector3(0f, -99f, 0f);
    }

    private void Start()
    {
        this.round = 1;
        Reset(round);
    }

    private void Update()
    {
        for (int i = 0; i < usingDisks.Count; i++)
        {
            if (usingDisks[i].transform.position.y < 0)
            {
                
            }
        }
    }
}
