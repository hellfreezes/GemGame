using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemRenderer : MonoBehaviour {

    public Gem data;

    SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public void ChangeGem(Gem newGem)
    {
        data = newGem;
        sr.sprite = data.sprite;
    }

    private void OnMouseDown()
    {
        TableController.Instance.AcceptSelection(data);
    }
}
