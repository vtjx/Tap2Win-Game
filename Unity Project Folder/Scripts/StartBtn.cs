using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StartBtn : MonoBehaviour
{
    private float transparency;
    private bool plus;
    private bool minus;
    private TextMeshProUGUI tmp;

    // Start is called before the first frame update
    void Start()
    {
        transparency = 1;
        tmp = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        blink();
    }

    private void blink()
    {
        if (transparency == 1)
        {
            minus = true;
            plus = false;
        }
        else if (transparency == 0)
        {
            minus = false;
            plus = true;
        }

        if (minus)
        {
            transparency -= Time.deltaTime;
        }
        else if (plus)
        {
            transparency += Time.deltaTime;
        }

        if (transparency >= 1)
        {
            transparency = 1;
        }
        else if (transparency <= 0)
        {
            transparency = 0;
        }

        tmp.color = new Color(1, 1, 1, transparency);
    }
}
