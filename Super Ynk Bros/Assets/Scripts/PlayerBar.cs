using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BarType
{
    health,
    mana
}
public class PlayerBar : MonoBehaviour
{
    private Slider slider;

    public BarType type;

    private int[] health, mana;

    private int maxValue;
    private int value;

    public Text barText;

    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (type)
        {
            case BarType.health:
                health = PlayerController.sharedInstance.GetHealth();
                slider.value = health[0];
                slider.maxValue = health[1];
                barText.text = health[0].ToString() + "/" + health[1].ToString();
                break;
            case BarType.mana:
                mana = PlayerController.sharedInstance.GetMana();
                slider.value = mana[0];
                slider.maxValue = mana[1];
                barText.text = mana[0].ToString() + "/" + mana[1].ToString();
                break;
            default:
                break;
        }
    }
}
