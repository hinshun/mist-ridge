using UnityEngine;
using System;
using System.Collections;
using Zenject;

namespace MistRidge
{
    public class TurnipView : MonoView
    {
        [SerializeField]
        private Transform emoteTransform;

        [SerializeField]
        private EmoteView emoteView;

        [SerializeField]
        private Transform turnipTarget;

        [SerializeField]
        private float popOutTime;

        [SerializeField]
        private float popOutDelay;

        private Animator animator;
        private PoolManager poolManager;
        private DialogueSignal.Trigger dialogueTrigger;
        private DialogueStateSignal.Trigger dialogueStateTrigger;

        private Hashtable popOutHashtable;

        [PostInject]
        public void Init(
                PoolManager poolManager,
                DialogueSignal.Trigger dialogueTrigger,
                DialogueStateSignal.Trigger dialogueStateTrigger)
        {
            this.poolManager = poolManager;
            this.dialogueTrigger = dialogueTrigger;
            this.dialogueStateTrigger = dialogueStateTrigger;
        }

        public void Alert()
        {
            poolManager.ReusePoolInstance(
                emoteView,
                emoteTransform.position,
                Quaternion.identity
            );

            iTween.MoveTo(gameObject, popOutHashtable);
        }

        public void OnPopOutStart()
        {
            animator.SetTrigger("PopOut");
        }

        public void OnPopOutComplete()
        {
            dialogueTrigger.Fire(DialogueType.TurnipTutorial);
            dialogueStateTrigger.Fire(DialogueStateType.Start);
        }

        private void Awake()
        {
            animator = GetComponent<Animator>();

            popOutHashtable = new Hashtable();
            popOutHashtable.Add("time", popOutTime);
            popOutHashtable.Add("position", turnipTarget);
            popOutHashtable.Add("delay", popOutDelay);
            popOutHashtable.Add("easetype", iTween.EaseType.easeInOutBack);
            popOutHashtable.Add("onstart", "OnPopOutStart");
            popOutHashtable.Add("oncomplete", "OnPopOutComplete");
        }
    }
}
