using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using Zenject;

namespace MistRidge
{
    public class CinematicDisplayView : MonoView
    {
        [SerializeField]
        private RectTransform topBar;

        [SerializeField]
        private RectTransform  bottomBar;

        [SerializeField]
        private float slideTime;

        private bool tweening;
        private Hashtable slideInHashtable;
        private Hashtable slideOutHashtable;

        private PlayerManager playerManager;
        private CinematicSignal cinematicSignal;

        [PostInject]
        public void Init(
                PlayerManager playerManager,
                CinematicSignal cinematicSignal)
        {
            this.playerManager = playerManager;
            this.cinematicSignal = cinematicSignal;
            this.cinematicSignal.Event += OnCinematicRequest;
        }

        public void UpdateBars(float slideValue)
        {
            topBar.anchoredPosition3D = new Vector3(
                topBar.anchoredPosition3D.x,
                25 - slideValue,
                topBar.anchoredPosition3D.z
            );

            bottomBar.anchoredPosition3D = new Vector3(
                bottomBar.anchoredPosition3D.x,
                slideValue - 25,
                bottomBar.anchoredPosition3D.z
            );
        }

        public override void SetActive(bool isActive)
        {
            if (tweening)
            {
                return;
            }

            tweening = true;

            if (isActive)
            {
                playerManager.ChangePlayerControl(false);

                topBar.anchoredPosition3D = new Vector3(
                    topBar.anchoredPosition3D.x,
                    -25,
                    topBar.anchoredPosition3D.z
                );

                bottomBar.anchoredPosition3D = new Vector3(
                    bottomBar.anchoredPosition3D.x,
                    -25,
                    bottomBar.anchoredPosition3D.z
                );

                gameObject.SetActive(true);
                iTween.ValueTo(gameObject, slideInHashtable);
            }
            else
            {
                iTween.ValueTo(gameObject, slideOutHashtable);
            }
        }

        public void OnSlideInComplete()
        {
            tweening = false;
        }

        public void OnSlideOutComplete()
        {
            tweening = false;
            playerManager.ChangePlayerControl(true);
            gameObject.SetActive(false);
        }

        private void Awake()
        {
            tweening = false;

            slideInHashtable = new Hashtable();
            slideInHashtable.Add("from", 0);
            slideInHashtable.Add("to", 50);
            slideInHashtable.Add("time", slideTime);
            slideInHashtable.Add("onupdate", "UpdateBars");
            slideInHashtable.Add("oncomplete", "OnSlideInComplete");

            slideOutHashtable = new Hashtable();
            slideOutHashtable.Add("from", 50);
            slideOutHashtable.Add("to", 0);
            slideOutHashtable.Add("time", slideTime);
            slideOutHashtable.Add("onupdate", "UpdateBars");
            slideOutHashtable.Add("oncomplete", "OnSlideOutComplete");
        }

        private void OnCinematicRequest(CinematicTransitionType cinematicTransitionType)
        {
            switch(cinematicTransitionType)
            {
                case CinematicTransitionType.SlideIn:
                    SetActive(true);
                    break;

                case CinematicTransitionType.SlideOut:
                    SetActive(false);
                    break;
            }
        }
    }
}
