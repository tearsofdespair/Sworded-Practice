using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinChanger : MonoBehaviour
{
    public GameObject Bot;
    public GameObject Skin;
    public Material NewMainMat;
    public Material MainMat;
    public Material NewHandsMat;
    public Material HandsMat;
    public Shader mainshader;
    public Shader handsshader;
    public MeshRenderer[] renderer;
    public MeshRenderer[] handsrenderer;
    public CableComponent[] cable;
    public BalanceSheet balance;
    void Awake()
    {
        if (PlayerPrefs.GetInt("Internet") == 1)
        {
            Bot.SetActive(false);
            Skin.SetActive(true);
            NewMainMat = new Material(mainshader);
            NewHandsMat = new Material(handsshader);
            NewMainMat.CopyPropertiesFromMaterial(MainMat);
            NewHandsMat.CopyPropertiesFromMaterial(HandsMat);
            int skin = Random.Range(0, 6);
            NewMainMat.mainTextureOffset = new Vector2(skin * 1f / 6, 1);
            NewHandsMat.color = balance.skin[skin].HandsColor;
            for (int i = 0; i < renderer.Length; i++)
            {
                renderer[i].materials[0] = NewMainMat;
                renderer[i].materials[0].mainTextureOffset = new Vector2(skin * 1f / 6, 1);
            }
            for (int i = 0; i < handsrenderer.Length; i++)
            {
                handsrenderer[i].material = NewHandsMat;
            }
            cable[0].cableMaterial = NewHandsMat;
            cable[1].cableMaterial = NewHandsMat;
            if (handsrenderer[0].material != NewHandsMat)
            {
                for (int i = 0; i < handsrenderer.Length; i++)
                {
                    handsrenderer[i].material = NewHandsMat;
                }
                cable[0].cableMaterial = NewHandsMat;
                cable[1].cableMaterial = NewHandsMat;
            }
        }
  
        Destroy(this);
    }
}
