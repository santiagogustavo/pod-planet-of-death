using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuButtonController : MonoBehaviour {
    Image imageComponent;
    Sprite originalSprite;

    [SerializeField]
    Sprite pressedSprite;

    void Start() {
        imageComponent = GetComponent<Image>();
        originalSprite = imageComponent.sprite;
    }

    public void SetImageAsOriginal() {
        imageComponent.sprite = originalSprite;
    }

    public void SetImageAsPressed() {
        imageComponent.sprite = pressedSprite;
    }
}
