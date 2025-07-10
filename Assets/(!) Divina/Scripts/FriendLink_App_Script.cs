using DG.Tweening;
using UnityEngine;

public class FriendLink_App_Script : MonoBehaviour
{
    Vector3 FriendLinkClosedPosition = new Vector3(0, -2500, 0);
    Vector3 FriendLinkOpenedPosition = new Vector3(0, 0, 0);
    float FriendLinkTransitionDuration = 0.5f;

    public void OpenFriendLinkCategory(GameObject FriendLinkCategoryPage)
    {
        Debug.Log("Opening FriendLink Category: " + FriendLinkCategoryPage.name);

        FriendLinkCategoryPage.transform.localPosition = FriendLinkClosedPosition;
        FriendLinkCategoryPage.gameObject.SetActive(true);

        this.gameObject.SetActive(false);

        FriendLinkCategoryPage.transform.DOLocalMove(FriendLinkOpenedPosition, FriendLinkTransitionDuration).SetEase(Ease.OutCubic);
    }

    public void ReturnToFriendLinkMain(GameObject FriendLinkMainPage)
    {
        Debug.Log("Returning to Main FriendLink Page...");

        FriendLinkMainPage.gameObject.SetActive(true);

        transform.DOLocalMove(FriendLinkClosedPosition, FriendLinkTransitionDuration).SetEase(Ease.OutCubic)
        .OnComplete(() =>
        {
            this.gameObject.SetActive(false);
        });
    }
}
