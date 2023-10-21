using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class parallax : MonoBehaviour
{
    public float parallaxSpeed = 1.0f;
    private Vector2 originalOffset;
    private Material material;

    private void Start()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            material = renderer.material;
            originalOffset = material.mainTextureOffset;
        }
        else
        {
            Debug.LogError("El objeto no tiene un Renderer con un material.");
            enabled = false;
        }
    }

    private void Update()
    {
        float mouseX = Mathf.Clamp(Input.mousePosition.x / Screen.width * 2 - 1, -1f, 1f);

        float xOffset = originalOffset.x + (mouseX * parallaxSpeed);

        material.mainTextureOffset = new Vector2(xOffset, originalOffset.y);
    }
}