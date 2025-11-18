using UnityEngine;
using DG.Tweening;

public class App_Webb : MonoBehaviour
{
    [Header("Web Page")]
    [SerializeField] private Transform webPageClosedPosition; // = new Vector3(0, 2500, 0);
    [SerializeField] private Transform webPageOpenedPosition; // = new Vector3(0, 0, 0);

    [Header("Seach Box")]
    [SerializeField] private Transform searchBoxClosedPosition; // = new Vector3(0, 250, 0);
    [SerializeField] private Transform searchBoxOpenedPosition; // = new Vector3(0, -210, 0);

    float transitionTime = 0.5f;

    [SerializeField] GameObject SearchResultBox;  

    /// <summary>
    /// Web Pages
    /// </summary>

    public void OpenWebPage(GameObject WebPage)
    {
        Debug.Log("Opening Web Page: " + WebPage.name);

        WebPage.transform.position = webPageClosedPosition.position;
        WebPage.gameObject.SetActive(true);

        WebPage.transform.DOMove(webPageOpenedPosition.position, transitionTime).SetEase(Ease.OutCubic)
        .OnComplete(() =>
        {
            //this.gameObject.SetActive(false);
        });
    }

    public void ReturnToBrowser(GameObject BrowserMainPage)
    {
        Debug.Log("Returning to Browser Main Page...");

        BrowserMainPage.gameObject.SetActive(true);

        transform.DOMove(webPageClosedPosition.position, transitionTime).SetEase(Ease.OutCubic)
        .OnComplete(() =>
        {
            //this.gameObject.SetActive(false);
        });
    }

    /// <summary>
    /// Search Box
    /// </summary>

    public void ShowSearchSuggestions(GameObject searchBox)
    {
        Debug.Log("Showing Search Suggestions...");
        SearchResultBox = searchBox;
        searchBoxClosedPosition = SearchResultBox.transform;
        SearchResultBox.SetActive(true);
        
        SearchResultBox.transform.DOMove(searchBoxOpenedPosition.position, transitionTime).SetEase(Ease.OutCubic);
    }

    public void SelectSearchResult(GameObject searchResult)
    {
        Debug.Log("Hiding Search Suggestions...");
        SearchResultBox.transform.DOMove(searchBoxClosedPosition.position, transitionTime).SetEase(Ease.OutCubic)
        .OnComplete(() =>
        {
            SearchResultBox.SetActive(false);
        });

        searchResult.transform.localPosition = webPageClosedPosition.position;
        searchResult.gameObject.SetActive(true);

        searchResult.transform.DOLocalMove(webPageOpenedPosition.position, transitionTime).SetEase(Ease.OutCubic);
    }


}
