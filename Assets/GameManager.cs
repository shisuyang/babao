using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.PostProcessing;
public class GameManager : MonoBehaviour
{
    public List<Box> boxes;
    public Image image;
    public Button open, close;
    public int index;
    public EventSystem eventSystem;
    public DepthOfField depthOfField;
    public PostProcessVolume postProcessVolume;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < boxes.Count; i++)
        {
            if (boxes[i].boxTop == null) continue;
            boxes[i].topStartPos = boxes[i].boxTop.position.y;
            boxes[i].buttomStartPos = boxes[i].boxButtom.position.y;

        }
        open.onClick.AddListener(ClickOpen);
        close.onClick.AddListener(ClicClose);
        postProcessVolume.sharedProfile.TryGetSettings<DepthOfField>(out depthOfField);
    }

    public void ClickOpen()
    {
        if (index < 0 || index > boxes.Count) return;

        var x = index;

        open.interactable = false;
        close.interactable = false;
        float xx = 47 - index * 4.5f;
        Camera.main.DOFieldOfView(xx, 3f);
        float distance = 48 + index * 0.3f;
        depthOfField.enabled.value = true;
        depthOfField.focusDistance = new FloatParameter
        {
            value = distance,
            overrideState = true
        };
        boxes[x].boxTop.DOLocalRotate(new Vector3(-120, 0, 0), 2f).OnComplete(delegate
        {
            index++;
            image.DOFade(0, 2f).OnComplete(delegate
            {
                image.sprite = boxes[index].sprite;
                image.SetNativeSize();
                image.DOFade(1, 2f);
            });


            boxes[x].box.transform.DOLocalMoveY(-70, 3f).OnComplete(delegate
            {
                boxes[x].box.SetActive(false);
                open.interactable = true;
                close.interactable = true;
                if (index == boxes.Count - 1)
                {
                    open.interactable = false;
                    close.interactable = true;
                }
            });
        });
        // boxes[x].boxTop.DOLocalMoveY(50, 3f).OnComplete(delegate
        // {

        //     index++;
        //     boxes[x].boxTop.gameObject.SetActive(false);
        //     boxes[x].boxButtom.gameObject.SetActive(false);
        //     image.sprite = boxes[index].sprite;
        //     image.SetNativeSize();
        //     open.interactable = true;
        //     close.interactable = true;
        //     if (index == boxes.Count - 1)
        //     {
        //         open.interactable = false;
        //         close.interactable = true;
        //     }

        // });
        // boxes[index].boxButtom.DOLocalMoveY(-70, 3f);


    }
    public void ClicClose()
    {
        if (index < 0 || index > boxes.Count) return;
        image.DOFade(0, 2f);
        index--;
        var x = index;
        open.interactable = false;
        close.interactable = false; ;
        boxes[x].box.SetActive(true);

        boxes[x].box.transform.DOLocalMoveY(0, 3f).OnComplete(delegate
        {
            image.sprite = boxes[index].sprite;
            image.SetNativeSize();
            image.DOFade(1, 2f);

            boxes[x].boxTop.DOLocalRotate(new Vector3(120, 0, 0), 2f,RotateMode.LocalAxisAdd).OnComplete(delegate
                {
                    open.interactable = true;
                    close.interactable = true;
                    if (index == 0)
                    {
                        open.interactable = true;
                        close.interactable = false;
                    }
                });
        });

        // boxes[x].boxTop.DOLocalMoveY(boxes[x].topStartPos, 3f).OnComplete(delegate
        // {

        //     image.sprite = boxes[index].sprite;
        //     image.SetNativeSize();
        //     open.interactable = true;
        //     close.interactable = true;
        //     if (index == 0)
        //     {
        //         open.interactable = true;
        //         close.interactable = false;
        //     }
       // // });
        boxes[index].boxButtom.DOLocalMoveY(boxes[x].buttomStartPos, 3f);
        float xx = 47 - index * 4.5f;
        Camera.main.DOFieldOfView(xx, 3f);
        float distance = 48 + index * 0.3f;
        depthOfField.enabled.value = true;
        depthOfField.focusDistance = new FloatParameter
        {
            value = distance,
            overrideState = true
        };
    }
    // Update is called once per frame
    void Update()
    {


    }
    [Serializable]
    public class Box
    {
        public GameObject box;
        public Transform boxTop;
        public Transform boxButtom;
        public Sprite sprite;
        public float topStartPos;
        public float buttomStartPos;
    }
}
