using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TButton : MonoBehaviour
{
    [SerializeField] string btnId = "";
    public System.Action<bool> OnStatChange;
    [SerializeField] Sprite OnSprite;
    [SerializeField] Sprite offSprite;
    [SerializeField] bool is_On = false;
    [SerializeField] Button btn;

    private void Awake()
    {
        if (btn == null)
            btn = GetComponent<Button>();
        btn.onClick.AddListener(OnBtnClick);
    }
    private void OnDestroy()
    {
        btn.onClick.RemoveAllListeners();
    }

    void Start()
    {
        btn.image.sprite = isOn ? OnSprite : offSprite;
    }


    void OnBtnClick()
    {
        isOn = !isOn;
        btn.image.sprite = isOn ? OnSprite : offSprite;
        OnStatChange?.Invoke(isOn);
    }

  

    public bool isOn
    {
        get
        {
            if(btnId!="")
                is_On = PlayerPrefs.GetInt(btnId, 1) == 1;
            return is_On;
        }
        set
        {
            is_On = value;
            if(btnId!="")
            {
                PlayerPrefs.SetInt(btnId, (is_On ? 1 : 0));
                PlayerPrefs.Save();
            }    
        }
    }

}
