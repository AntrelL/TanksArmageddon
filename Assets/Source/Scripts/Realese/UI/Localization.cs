using Agava.YandexGames;
using Lean.Localization;
using UnityEngine;

public class Localization : MonoBehaviour
{
    private const string EnglishCode = "English";
    private const string RussianCode = "Russian";
    private const string TurkishCode = "Turkish";
    private const string Turkish = "tr";
    private const string Russian = "ru";
    private const string English = "en";

    [SerializeField] private LeanLocalization _leanLanguage;

    private void Awake()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        Debug.Log("TRY TO CHANGE LANGUAGE");
        ChangeLanguage();
#endif
    }

    private void ChangeLanguage()
    {
        string languageCode = YandexGamesSdk.Environment.i18n.lang;

        Debug.LogError("languageCode: " + languageCode);
        Debug.Log("languageCode: " + languageCode);

        switch (languageCode)
        {
            case English:
                _leanLanguage.SetCurrentLanguage(EnglishCode);

                break;
            case Turkish:
                _leanLanguage.SetCurrentLanguage(TurkishCode);

                break;
            case Russian:
                _leanLanguage.SetCurrentLanguage(RussianCode);

                break;
        }
    }
}
