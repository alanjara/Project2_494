using UnityEngine;
using System.Collections;

public class Goal : MonoBehaviour {
    Renderer target;
    Color alternate = Color.yellow;
    Color original;
    float num;
    public static GameObject winBox;
    // Use this for initialization
    void Start() {
        target = GetComponent<Renderer>();
        original = target.material.color;
        winBox = gameObject;
    }

    // Update is called once per frame
    void Update() {
        Color temp = alternate;
        temp = temp * (1f - (Main.MCU.currentFrame % 10) / 10f) + ((Main.MCU.currentFrame % 10) / 10f) * original;
        target.material.color = temp;
    }
}
