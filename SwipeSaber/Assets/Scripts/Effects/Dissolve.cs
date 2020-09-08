using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dissolve : MonoBehaviour
{
    Material material;
    float fade = 1.0f;
    public bool isDissolving = false;

    SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        material = spriteRenderer.material;
    }

    private void Update()
    {
        if (isDissolving)
        {
            Dissolving();
        }
    }

    public void StartDissolve(float scale)
    {
        material.SetFloat("_Scale", scale);
        isDissolving = true;
    }

    public void Dissolving()
    {
        fade -= Time.deltaTime;

        if (fade <= 0f)
        {
            fade = 0;
            isDissolving = false;
            gameObject.SetActive(false);

        }

        material.SetFloat("_Fade", fade);
    }

    private void OnEnable()
    {
        if (material == null)
        {
            Start();
        }

        isDissolving = false;
        fade = 1.0f;
        material.SetFloat("_Fade", fade);
        spriteRenderer.enabled = true;
    }
}
