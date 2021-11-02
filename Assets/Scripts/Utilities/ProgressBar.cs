using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

[ExecuteInEditMode]
public class ProgressBar : MonoBehaviour
{
    public float maximum;
    public float current;
    public float speed = 2;
    public Image mask;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        GetCurrentFill();
    }

    void GetCurrentFill()
    {
        float fillAmount = current / maximum;
        mask.fillAmount = Mathf.Lerp(mask.fillAmount, fillAmount, Time.deltaTime * speed);

        if (mask.fillAmount == maximum)
            mask.fillAmount = 0;
    }
}
