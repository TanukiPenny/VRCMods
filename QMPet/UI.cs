using System.Collections;
using ReMod.Core.Managers;
using ReMod.Core.VRChat;
using UnityEngine;
using UnityEngine.UI;


namespace QMPet;

public class UI
{
    public static GameObject QMPetFront, QMPetBack;
    private static GameObject VRCat, Container, QMPet, Character;
    private static RawImage QMPetFront_Img, QMPetBack_Img;
    private static VRC.UI.Elements.QuickMenu QuickMenu;
    private static RectTransform QMPetFront_Rect, QMPetBack_Rect;

    public static IEnumerator UIInit()
    {
        while (GameObject.Find("UserInterface").GetComponentInChildren<VRC.UI.Elements.QuickMenu>(true) == null)
            yield return null;

        PreparePet();
        AssignImgs();

        Main.Log.Msg("Set up done!");
    }

    public static void PreparePet()
    {
        QuickMenu = QuickMenuEx.Instance;
        Container = QuickMenu.gameObject.transform.FindChild("Container").gameObject;
        
        VRCat = Container.transform.FindChild("ThankYouCharacter").gameObject;
        QMPet = GameObject.Instantiate(VRCat, Container.transform);
        QMPet.name = "QMPet";

        Object.DestroyImmediate(VRCat);
        Object.DestroyImmediate(QMPet.transform.FindChild("Dialog Bubble").gameObject);
        Object.DestroyImmediate(QMPet.GetComponent<VRCPlusThankYou>());

        //Character = QMPet.transform.FindChild("Character").gameObject;
        QMPetFront = QMPet.transform.FindChild("Character").FindChild("VRCat_Front").gameObject;
        QMPetFront_Img = QMPetFront.GetComponent<RawImage>();
        QMPetFront_Rect = QMPetFront.GetComponent<RectTransform>();
        QMPetBack = QMPet.transform.FindChild("Character").FindChild("VRCat_Back").gameObject;
        QMPetBack_Img = QMPetBack.GetComponent<RawImage>();
        QMPetBack_Rect = QMPetBack.GetComponent<RectTransform>();
        
        QMPetFront.transform.localPosition = new Vector3(220, 25, 0);
        QMPetBack.transform.localPosition = new Vector3(220, 25, 0);
        QuickMenu.field_Public_GameObject_2 = QMPet;
        QuickMenu.field_Public_GameObject_3 = QMPetFront;
        QuickMenu.field_Public_GameObject_4 = QMPetBack;
        QMPetFront_Rect.pivot = new Vector2(1, 0);
        QMPetBack_Rect.pivot = new Vector2(1, 0);

        QMPetFront.transform.localScale = new Vector3(Main.scale.Value, Main.scale.Value, Main.scale.Value);
        QMPetBack.transform.localScale = new Vector3(Main.scale.Value, Main.scale.Value, Main.scale.Value);
        
        QMPetFront_Img.texture.wrapMode = TextureWrapMode.Clamp;
        QMPetFront_Img.texture.wrapModeU = TextureWrapMode.Clamp;
        QMPetFront_Img.texture.wrapModeW = TextureWrapMode.Clamp;
        QMPetFront_Img.texture.wrapModeV = TextureWrapMode.Clamp;

        QMPetFront_Img.texture.filterMode = FilterMode.Bilinear;
        QMPetFront_Img.texture.mipMapBias = 0;
        
        QMPetBack_Img.texture.wrapMode = TextureWrapMode.Clamp;
        QMPetBack_Img.texture.wrapModeU = TextureWrapMode.Clamp;
        QMPetBack_Img.texture.wrapModeW = TextureWrapMode.Clamp;
        QMPetBack_Img.texture.wrapModeV = TextureWrapMode.Clamp;

        QMPetBack_Img.texture.filterMode = FilterMode.Bilinear;
        QMPetBack_Img.texture.mipMapBias = 0;

        QMPetFront_Img.texture = null;
        QMPetBack_Img.texture = null;
    }

    private static void AssignImgs()
    {
        if (Main.FrontLoaded)
        {
            QMPetFront_Img.texture = ResourceManager.GetTexture("FrontTexture");
        }
        else
        {
            QMPetFront.active = false;
        }
        if (Main.BackLoaded)
        {
            QMPetFront_Img.texture = ResourceManager.GetTexture("BackTexture");
        }
        else
        {
            QMPetBack.active = false;
        }
    }
}