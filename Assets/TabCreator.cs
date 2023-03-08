using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using TMPro;
using UnityEngine;

public class TabCreator : MonoBehaviour {

    //Tab Positioning;
    [SerializeField] GameObject tab, scroller, smallTab;
    [SerializeField] Sprite activeTabSprite, inactiveSprite;
    [SerializeField] Transform canvas;
    [SerializeField] float yy;
    [SerializeField] int tabSize;

    //Data Handlers;
    List<GameObject> buttons = new List<GameObject>();
    public bool hasNormalNumbering = false;
    [HideInInspector] public bool hasScrollers = false, hasSmallTabs = false;

    //Math Handlers;
    int tabRounder, arrowPointer, tabMedian, minusMedian, medianRounder, finalArrowPos;
    public int tabsToCreate, tabsToSkip, tabMainIndex, maxTabs, currentTab = 1;

    private void Start() {
        //inactiveSprite = GetComponent<SpriteRenderer>().sprite;

        //Adding the extra tabs;
        HasMoreTabs(hasScrollers);
        HasMoreTabs(hasSmallTabs);

        //Tab math;
        float tabOperation = (maxTabs / 2f);
        tabMedian = Mathf.RoundToInt(tabOperation + 0.1f);
        tabRounder = tabOperation % 1 > 0 ? 1 : 0;
        if(tabOperation % 1 > 0) tabRounder = 1;
        minusMedian = tabMedian * -1;
        medianRounder = tabMedian - tabRounder;

        for(int i = minusMedian; i < medianRounder; i++) {
            GameObject instance;
            Vector3 pos = new Vector3(tabSize * i + (yy * tabRounder), 0, 0);
            int realI = i + tabMedian; //From 0 onwards;

            //Spawning the scrollers;
            switch(hasSmallTabs) {
                case true:
                    arrowPointer = -2;
                    finalArrowPos = medianRounder + arrowPointer;
                    instance = CallScroller(i, minusMedian + 1, finalArrowPos, hasScrollers, scroller);
                    if(instance == null) instance = CallScroller(i, minusMedian, medianRounder - 1, hasSmallTabs, smallTab);
                    break;

                case false:
                    arrowPointer = -1;
                    finalArrowPos = medianRounder + arrowPointer;
                    instance = CallScroller(i, minusMedian, finalArrowPos, hasScrollers, scroller);
                    break;
            }

            if(instance == null) instance = tab;


            //Creating & managing the button;
            GameObject button = Instantiate(instance, canvas);
            button.name = (realI).ToString();
            buttons.Add(button);
            button.transform.localPosition = pos;
            if(hasScrollers && i == finalArrowPos) button.transform.localScale = Vector3.Scale(button.transform.localScale, new Vector3(-1, 1, 1));

            //Tab Components;
            int mainTab = 0;

            if(hasNormalNumbering) {
                tabMainIndex = 0;
                mainTab = i;
            } else {
                mainTab = i + tabMedian;
            }
        }
    }

    private void HasMoreTabs(bool check) {
        if(check) {
            tabsToCreate += 2;
            maxTabs += 2;
            tabsToSkip += 2;
        } else hasSmallTabs = false;
    }

    private GameObject CallScroller(int i, int start, int end, bool check, GameObject obj) {
        if(check) { //Assigning the side scrollers to spawn;
            if(i == start || i == end) {
                return obj;
            }
        }
        return null;
    }

    private bool IsMainTab(int i) {
        return i == tabMainIndex? true: false;
    }

    public bool refresh = true;

    private void Update() {
        if(refresh) {
            CalculateTabIndexes();
        }
    }

    private void CalculateTabIndexes() {
        int pointer = currentTab;
        int realTabs = tabsToCreate - tabsToSkip;
        int arrowPos = Mathf.Abs(arrowPointer) - 1;
        int endArrowPos = maxTabs - finalArrowPos;
        int maxPointer = 0;

        for(int i = 0; i < maxTabs; i++) {
            int tabSendTo = pointer;

            if(hasScrollers) {
                if(i == arrowPos) {
                    tabSendTo = pointer - 1 <= 0 ? realTabs : pointer - 1;
                    ChangeText(buttons[i], tabSendTo, i); //Left Arrow;
                    continue;
                }
                else if(i == endArrowPos) {
                    tabSendTo = pointer + 1 > realTabs ? 1 : pointer + 1;
                    ChangeText(buttons[i], tabSendTo, i); //Right Arrow;
                    continue;
                }
            }

            int tabIndex = currentTab - tabsToSkip + i;

            switch(hasNormalNumbering) {
                case true:
                    tabSendTo = i + 1;
                break;

                default:
                    tabIndex = currentTab - tabsToSkip + i;


                    if(tabIndex <= 0) tabSendTo = realTabs + tabIndex;
                    else {
                        if(tabIndex > realTabs) {
                            tabSendTo = 1 + maxPointer;
                            maxPointer++;
                        } else tabSendTo = tabIndex;
                    }
                    /*if(i < realTabs) {
                        tabSendTo = tabIndex <= 0 ? tabIndex + realTabs : tabIndex;
                    }
                    else tabSendTo = tabIndex >= realTabs ? tabIndex - realTabs : tabIndex;*/
                    break;
            }

            ChangeText(buttons[i], tabSendTo, i);
        }

        refresh = false;
    }

    [SerializeField] Color tabTextColor;

    private void ChangeText(GameObject button, int index, int i) {
        try {
            TextMeshProUGUI textMesh = button.GetComponentInChildren<TextMeshProUGUI>();
            textMesh.text = index.ToString();
            Transform buttonContainer = button.transform.Find("Tab Button");
            if(IsMainTab(i)) {
                textMesh.color = Color.white;

                try { //Set active color tab;
                    buttonContainer.GetComponent<UnityEngine.UI.Image>().sprite = activeTabSprite;
                    TabButtonFunctions.ScaleUp(button);
                }
                catch { }

                button.transform.localScale = Vector3.one * 0.45f;
            }
            else {
                textMesh.color = tabTextColor;
                buttonContainer.GetComponent<UnityEngine.UI.Image>().sprite = inactiveSprite;
                TabButtonFunctions.ScaleDown(button);
            }
        }
        catch { }

        try { button.GetComponentInChildren<TabButtonFunctions>().index = index; } catch { }
    }
}
