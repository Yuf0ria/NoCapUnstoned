using UnityEngine;
using DG.Tweening;

public class App_Webb : MonoBehaviour
{
    //Stop using local. please

    private GameObject activepage;

    [Header("Web Pages")]
    [SerializeField] private Transform webPageClosedPosition; // = new Vector3(0, 2500, 0);
    [SerializeField] private Transform webPageOpenedPosition; // = new Vector3(0, 0, 0);
    [SerializeField] private RectTransform FishSearches; // = new Vector3(0, 0, 0);
    [SerializeField] private RectTransform FoodSearches; // = new Vector3(0, 0, 0);
    [SerializeField] private RectTransform SecuritySearches; // = new Vector3(0, 0, 0);

    [Header("Ads")]
    [SerializeField] private RectTransform adPrefab;

    [Header("Seach Box")]
    [SerializeField] private Transform searchBoxClosedPosition; // = new Vector3(0, 250, 0);
    [SerializeField] private Transform searchBoxOpenedPosition; // = new Vector3(0, -210, 0);
    [SerializeField] private bool searchBoxClosed;

    float transitionTime = 0.5f;
    public float TransitionMult = 1f; //This is for the slowing down of the App


    [SerializeField] GameObject SearchResultBox;

    //Hard code ones

    [Header("search results")]
    [SerializeField] private GameObject fishResults;
    [SerializeField] private GameObject foodResults;
    [SerializeField] private GameObject securityResults; 
    [SerializeField] private GameObject phishingResults;

    //[Header("Buttons")]


    /// <summary>
    /// Web Pages
    /// </summary>
    /// 

    private void Start()
    {
        if(searchBoxClosed) SearchResultBox.transform.position = searchBoxClosedPosition.position;
    }

    public void OpenWebPage(GameObject WebPage)
    {
        Debug.Log("Opening Web Page: " + WebPage.name);

        WebPage.transform.position = webPageClosedPosition.position;
        WebPage.gameObject.SetActive(true);
        SearchResultBox.transform.DOMove(searchBoxClosedPosition.position, transitionTime * TransitionMult).SetEase(Ease.OutCubic);

        if (activepage != null)
        {
            activepage.transform.DOMove(webPageClosedPosition.position, transitionTime * TransitionMult).SetEase(Ease.OutCubic);
            activepage = WebPage;
        }

        else
        {
            activepage = WebPage;
        }

        WebPage.transform.DOMove(webPageOpenedPosition.position, transitionTime * TransitionMult).SetEase(Ease.OutCubic);

        // POP UP CHANCE
        float roll = UnityEngine.Random.value; // gives 0.0 to 1.0

        if (roll <= 0.33f && !Phone_Statistics.isAdBlocker) // 33% Chance
        {
            OpenAd();
        }
    }

    public void ReturnToBrowser(GameObject BrowserMainPage)
    {
        Debug.Log("Returning to Browser Main Page...");

        BrowserMainPage.gameObject.SetActive(true);

        transform.DOMove(webPageClosedPosition.position, transitionTime * TransitionMult).SetEase(Ease.OutCubic);

        activepage = null;
    }

    /// <summary>
    /// Search Box
    /// </summary>

    public void ShowSearchSuggestions(GameObject searchBox)
    {
        Debug.Log("Showing Search Suggestions...");
        SearchResultBox = searchBox;
        //searchBoxClosedPosition = SearchResultBox.transform;
        //SearchResultBox.SetActive(true);

        if (!searchBoxClosed)
        {
            SearchResultBox.transform.DOMove(searchBoxClosedPosition.position, transitionTime * TransitionMult).SetEase(Ease.OutCubic);
            searchBoxClosed = true;
        }

        else         
        {
            SearchResultBox.transform.DOMove(searchBoxOpenedPosition.position, transitionTime * TransitionMult).SetEase(Ease.OutCubic);
            searchBoxClosed = false;
        }
            
    }

    public void SelectSearchResult(GameObject searchResult)
    {
        Debug.Log("Hiding Search Suggestions...");
        SearchResultBox.transform.DOMove(searchBoxClosedPosition.position, transitionTime * TransitionMult).SetEase(Ease.OutCubic)
        .OnComplete(() =>
        {
            //SearchResultBox.SetActive(false);
        });

        searchResult.transform.position = webPageClosedPosition.position;
        searchResult.gameObject.SetActive(true);

        searchResult.transform.DOMove(webPageOpenedPosition.position, transitionTime * TransitionMult).SetEase(Ease.OutCubic);
    }

    public void OpenSearchCategory(float i)
    {
        if(i == 1)
        {
            FishSearches.transform.DOMove(webPageOpenedPosition.transform.position, transitionTime).SetEase(Ease.OutCubic);
        }

        if (i == 2)
        {
            FoodSearches.transform.DOMove(webPageOpenedPosition.transform.position, transitionTime).SetEase(Ease.OutCubic);
        }

        if (i == 3)
        {
            SecuritySearches.transform.DOMove(webPageOpenedPosition.transform.position, transitionTime).SetEase(Ease.OutCubic);
        }
    }





    float popDuration = 0.35f;
    float closeDuration = 0.25f;
    public void OpenAd()
    {
        adPrefab.gameObject.SetActive(true);
        adPrefab.localScale = Vector3.zero;
        adPrefab.DOScale(1f, popDuration).SetEase(Ease.OutBack);
    }

    public void CloseAd()
    {
        adPrefab.DOScale(0f, closeDuration).SetEase(Ease.InBack).OnComplete(() =>
        {
            adPrefab.gameObject.SetActive(false);
        });
    }

}
