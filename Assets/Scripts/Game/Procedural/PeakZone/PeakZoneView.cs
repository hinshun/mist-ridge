using UnityEngine;
using System;

namespace MistRidge
{
    public class PeakZoneView : MonoView
    {
        [SerializeField]
        private Transform peakTransform;

        [SerializeField]
        private PeakView peakView;

        [SerializeField]
        private Transform peakCheckpointTransform;

        public Transform PeakTransform
        {
            get
            {
                return peakTransform;
            }
        }

        public Transform PeakCheckpointTransform
        {
            get
            {
                return peakCheckpointTransform;
            }
        }

        public PeakView PeakView
        {
            get
            {
                return peakView;
            }
        }
    }
}
