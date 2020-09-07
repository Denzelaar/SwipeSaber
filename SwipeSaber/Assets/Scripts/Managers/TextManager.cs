using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextManager :  Singleton<GeneralManager>
{
    public List<Text> gameTexts;

    void UpdateText(string textName, string updateText)
    {
        foreach (var item in gameTexts)
        {
            if(item.gameObject.name == textName)
            {
                item.text = updateText;
            }
        }
    }
}
