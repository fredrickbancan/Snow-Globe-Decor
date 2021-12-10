using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoSelector : MonoBehaviour
{
    [SerializeField] AmmoGui[] ammoGuiIcons;
    private int currentSelectedIcon = 0;
    public GameSound selectIconSound;

    private void Awake()
    {
        GameManager.weaponChangeNext += NextIcon;
        GameManager.weaponChangePrev += PrevIcon;

        ammoGuiIcons[currentSelectedIcon].Selected();
    }

    private void OnDestroy()
    {
        GameManager.weaponChangeNext -= NextIcon;
        GameManager.weaponChangePrev -= PrevIcon;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void NextIcon()
    {
        if (currentSelectedIcon == ammoGuiIcons.Length - 1)
            return;

        currentSelectedIcon++;

        for (int i = 0; i < ammoGuiIcons.Length; i++)
        {
            ammoGuiIcons[i].Deselected();
        }

        ammoGuiIcons[currentSelectedIcon].Selected();
        SoundManager.Instance.PlaySound2D(selectIconSound);
    }

    private void PrevIcon()
    {
        if (currentSelectedIcon == 0)
            return;

        currentSelectedIcon--;

        for (int i = 0; i < ammoGuiIcons.Length; i++)
        {
            ammoGuiIcons[i].Deselected();
        }

        ammoGuiIcons[currentSelectedIcon].Selected();
        SoundManager.Instance.PlaySound2D(selectIconSound);
    }
}
