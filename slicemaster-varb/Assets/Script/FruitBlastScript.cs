using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FruitBlastScript : MonoBehaviour {

    HingeJoint2D hinge;
    public JointMotor2D mot;
    [SerializeField]
    float[] RotSpeed;
    GameManager gm;
    // for initialization
    void Start () {
        hinge = GetComponent<HingeJoint2D>();
        mot = hinge.motor;
        mot.motorSpeed = Random.Range(100, -100);
        GetComponent<HingeJoint2D>().motor = mot;
        gm = FindObjectOfType<GameManager>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Knife")
        {
            col.GetComponent<KnifeScript>().FruitsDes++;
            transform.GetChild(3).GetChild(0).GetComponent<Text>().text = "+" + col.GetComponent<KnifeScript>().FruitsDes.ToString();
            gm.ScoreCount += col.GetComponent<KnifeScript>().FruitsDes;
            gm.ScoreSet();
            gm.SliderSet();
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(1).gameObject.SetActive(true);
            transform.GetChild(2).gameObject.SetActive(true);
            transform.GetChild(3).gameObject.SetActive(true);
            Destroy(transform.GetChild(0).gameObject, 2);
            Destroy(transform.GetChild(3).gameObject, 2);
            transform.GetChild(1).GetComponent<Rigidbody2D>().AddForce(Vector3.up * 200);
            transform.GetChild(2).GetComponent<Rigidbody2D>().AddForce(Vector3.up * 200);
            transform.GetChild(1).GetComponent<Rigidbody2D>().AddForce(Vector3.right * 500);
            transform.GetChild(2).GetComponent<Rigidbody2D>().AddForce(Vector3.left * 500);
            transform.GetChild(1).GetComponent<Rigidbody2D>().AddTorque(Random.Range(-500, 500));
            transform.GetChild(2).GetComponent<Rigidbody2D>().AddTorque(Random.Range(-500, 500));
            transform.GetChild(0).eulerAngles = Vector3.zero;
            transform.GetChild(1).eulerAngles = Vector3.zero;
            transform.GetChild(2).eulerAngles = Vector3.zero;
            transform.GetChild(3).eulerAngles = Vector3.zero;
            transform.GetChild(3).SetParent(null);
            transform.GetChild(2).SetParent(null);
            transform.GetChild(1).SetParent(null);
            transform.GetChild(0).SetParent(null);
        }
    }
}
