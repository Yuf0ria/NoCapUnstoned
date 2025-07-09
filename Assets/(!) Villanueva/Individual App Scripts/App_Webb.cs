using UnityEngine;
using DG.Tweening;

public class App_Webb : MonoBehaviour
{
    Vector3 WebPageClosedPosition = new Vector3(0, 2500, 0);
    Vector3 WebPageOpenedPosition = new Vector3(0, 0, 0);
    float WebPageTransitionTime = 1f;

    public void OpenWebPage(GameObject WebPage)
    {
        Debug.Log("Opening Web Page: " + WebPage.name);

        WebPage.transform.localPosition = WebPageClosedPosition;
        WebPage.gameObject.SetActive(true);

        WebPage.transform.DOLocalMove(WebPageOpenedPosition, WebPageTransitionTime).SetEase(Ease.OutCubic)
        .OnComplete(() =>
        {
            this.gameObject.SetActive(false);
        });
    }

    public void ReturnToBrowser(GameObject BrowserMainPage)
    {
        Debug.Log("Returning to Browser Main Page...");

        BrowserMainPage.gameObject.SetActive(true);

        transform.DOLocalMove(WebPageClosedPosition, WebPageTransitionTime).SetEase(Ease.OutCubic)
        .OnComplete(() =>
        {
            //this.gameObject.SetActive(false);
        });
    }



    Vector3 SearchBoxClosedPosition = new Vector3(0, 250, 0);
    Vector3 SearchBoxOpenedPosition = new Vector3(0, -210, 0);
    float SearchBoxTransitionTime = 0.5f;

    [SerializeField] GameObject SearchResultBox;

    public void ShowSearchSuggestions(GameObject searchBox)
    {
        Debug.Log("Showing Search Suggestions...");
        SearchResultBox = searchBox;
        SearchBoxClosedPosition = SearchResultBox.transform.localPosition;
        SearchResultBox.SetActive(true);
        
        SearchResultBox.transform.DOLocalMove(SearchBoxOpenedPosition, SearchBoxTransitionTime).SetEase(Ease.OutCubic);
    }

    public void SelectSearchResult(GameObject searchResult)
    {
        Debug.Log("Hiding Search Suggestions...");
        SearchResultBox.transform.DOLocalMove(SearchBoxClosedPosition, SearchBoxTransitionTime).SetEase(Ease.OutCubic)
        .OnComplete(() =>
        {
            SearchResultBox.SetActive(false);
        });

        searchResult.transform.localPosition = WebPageClosedPosition;
        searchResult.gameObject.SetActive(true);

        searchResult.transform.DOLocalMove(WebPageOpenedPosition, WebPageTransitionTime).SetEase(Ease.OutCubic);
    }


}
