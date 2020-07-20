using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    public float upSpeed;
    public float destroyWaitTime;
    public TextMeshProUGUI text;
    public float scaleSize;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, destroyWaitTime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(0, Time.deltaTime * upSpeed, 0);
        transform.localScale += new Vector3(Time.deltaTime * scaleSize, Time.deltaTime * scaleSize, 0);
    }

    public void setDamageText(float _damage)
    {
        text.text = _damage+"";
    }
}
