using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CheeseBar : MonoBehaviour {
    public const float maxhealth = 100f;
    [Range(0.0f, maxhealth)]
    public float health = 100f;
    [SerializeField] private GameObject baseRendererObj;
    [SerializeField] private GameObject baseBRendererObj;
    [SerializeField] private GameObject fillRendererObj;
    [SerializeField] private GameObject fillBRendererObj;
    [SerializeField] private GameObject iconRendererObj;
    [SerializeField] private GameObject iconBRendererObj;
    [SerializeField] private Sprite[] baseSprites;
    [SerializeField] private Sprite[] fillSprites;
    [SerializeField] private Sprite[] iconSprites;
    private Image baseRenderer;
    private Image baseBRenderer;
    private Image fillRenderer;
    private Image fillBRenderer;
    private Image iconRenderer;
    private Image iconBRenderer;

    void Start() {
        baseRenderer = baseRendererObj.GetComponent<Image>();
        baseBRenderer = baseBRendererObj.GetComponent<Image>();
        fillRenderer = fillRendererObj.GetComponent<Image>();
        fillBRenderer = fillBRendererObj.GetComponent<Image>();
        iconRenderer = iconRendererObj.GetComponent<Image>();
        iconBRenderer = iconBRendererObj.GetComponent<Image>();
    }

    void Update() {
        float mul = maxhealth / (baseSprites.Length - 1);
        int index = (int)Mathf.Ceil((maxhealth - health) / mul);
        int nindex = index - 1;
        float alpha = (health % mul) / (mul - 1f);
        baseRenderer.sprite = baseSprites[index];
        baseBRenderer.sprite = index == 0 ? baseSprites[index] : baseSprites[nindex];
        baseBRenderer.color = baseBRenderer.color.WithAlpha(alpha);
        fillRenderer.sprite = fillSprites[index];
        fillBRenderer.sprite = index == 0 ? fillSprites[index] : fillSprites[nindex];
        fillBRenderer.color = fillBRenderer.color.WithAlpha(alpha);
        iconRenderer.sprite = iconSprites[index];
        iconBRenderer.sprite = index == 0 ? iconSprites[index] : iconSprites[nindex];
        iconBRenderer.color = iconBRenderer.color.WithAlpha(alpha);
    }
}