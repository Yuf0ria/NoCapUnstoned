using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Navigation : MonoBehaviour
{
    //[Header("Coords - Main Menu")]
    //[SerializeField] private GameObject naviameObject;

    [Header("Coords - Main Menu")]
    [SerializeField] private Transform hidePosition;
    [SerializeField] private Transform bottomHidePosition;
    [SerializeField] private Transform showPosition;

    [Header("Coords - Story")]
    [SerializeField] private Transform standardPosition;
    [SerializeField] private Transform leftPosition;
    [SerializeField] private Transform rightPosition;

    [Header("Stories - Index")]    
    [SerializeField] private int nextIndex = 1;
    [SerializeField] private int storyIndex = 0;
    [SerializeField] private int prevIndex = -1;

    [Header("Stories - GameObjects")]
    [SerializeField] private GameObject[] stories;
    [SerializeField] private GameObject previousStory;
    [SerializeField] private GameObject currentStory;    
    [SerializeField] private GameObject upcomingStory;

    [Header("Stories - Profiles")]
    [SerializeField] private Sprite[] profilePictures;
    [SerializeField] private Image profile;

    float TransitionTime = 0.5f; //Both has the same time transitions so...

    private void Start()
    {
        storyIndex = 0;
        ordering(); 
    }

    private void Update()
    {
        profile.sprite = profilePictures[storyIndex];
    }

    /// <summary>
    /// UP AND DOWN
    /// </summary>

    //SHOW
    public void openPage(GameObject pageType)//, GameObject pageType)
    {
        pageType.SetActive(true);
        pageType.transform.position = bottomHidePosition.position;


        transform.DOMove(showPosition.position, TransitionTime).SetEase(Ease.OutCubic);
    }

    //CLOSE
    public void returnToMain(GameObject pageType)
    {
        transform.DOMove(hidePosition.position, TransitionTime).SetEase(Ease.OutCubic)
            .OnComplete(() =>
             {
                 pageType.SetActive(false);
             });
    }

    /// <summary>
    /// LEFT AND RIGHT
    /// <summary>

    //NEXT
    public void nextStory()//, GameObject pageType)
    {
        //I'm trying so hard to figure out how to do this
        storyIndex++;
        if (storyIndex >= stories.Length)
        {
            storyIndex = 0;
        }

        upcomingStory.transform.position = rightPosition.position;

        //Hide
        currentStory.transform.DOMove(leftPosition.position, TransitionTime).SetEase(Ease.OutCubic);
        //Show
        upcomingStory.transform.DOMove(standardPosition.position, TransitionTime).SetEase(Ease.OutCubic)
            .OnComplete(() =>
            {
                ordering();
            });
    }

    //BACK
    public void backStory()
    {
        storyIndex--;
        if (storyIndex < 0)
        {
            storyIndex = stories.Length - 1;
        }

        previousStory.transform.position = leftPosition.position; 

        //Hide
        currentStory.transform.DOMove(rightPosition.position, TransitionTime).SetEase(Ease.OutCubic);
        //Show
        previousStory.transform.DOMove(standardPosition.position, TransitionTime).SetEase(Ease.OutCubic)
            .OnComplete(() =>
            {
                ordering();
            });
    }
    
    private void ordering()
    {
        //making a cycle
        prevIndex = storyIndex - 1;
        if (prevIndex < 0)
        {
            prevIndex = stories.Length - 1;
        }

        nextIndex = storyIndex + 1;
        if (nextIndex >= stories.Length)
        {
            nextIndex = 0;
        }

        if (stories.Length > 0)
        {
            previousStory = stories[prevIndex];
            currentStory = stories[storyIndex];
            upcomingStory = stories[nextIndex];
        }
    }
}
