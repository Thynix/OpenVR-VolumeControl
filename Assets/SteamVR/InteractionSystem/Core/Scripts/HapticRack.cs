﻿//======= Copyright (c) Valve Corporation, All rights reserved. ===============
//
// Purpose: Triggers haptic pulses based on a linear mapping
//
//=============================================================================

using UnityEngine;
using UnityEngine.Events;
using System.Collections;

namespace Valve.VR.InteractionSystem
{
    //-------------------------------------------------------------------------
    [RequireComponent(typeof(Interactable))]
    public class HapticRack : MonoBehaviour
    {
        [Tooltip("The linear mapping driving the haptic rack")]
        public LinearMapping linearMapping;

        [Tooltip("The number of haptic pulses evenly distributed along the mapping")]
        public int teethCount = 128;

        [Tooltip("Minimum duration of the haptic pulse")]
        public int minimumPulseDuration = 500;

        [Tooltip("Maximum duration of the haptic pulse")]
        public int maximumPulseDuration = 900;

        [Tooltip("This event is triggered every time a haptic pulse is made")]
        public UnityEvent onPulse;

        private Hand _hand;
        private int previousToothIndex = -1;

        //-------------------------------------------------
        void Awake()
        {
            if (linearMapping == null)
            {
                linearMapping = GetComponent<LinearMapping>();
            }
        }


        //-------------------------------------------------
        protected void OnAttachedToHand(Hand hand)
        {
            this._hand = hand;
        }

        //-------------------------------------------------
        protected void Update()
        {
            int currentToothIndex = Mathf.RoundToInt(linearMapping.value * teethCount - 0.5f);
            if (currentToothIndex != previousToothIndex)
            {
                Pulse(_hand);
                previousToothIndex = currentToothIndex;
            }
        }

        //-------------------------------------------------
        private void Pulse(Hand hand)
        {
            if (hand == null)
            {
                Debug.Log("Not pulsing due to unset hand");
                return;
            }

            ushort duration = (ushort) Random.Range(minimumPulseDuration, maximumPulseDuration + 1);
            hand.TriggerHapticPulse(duration);

            onPulse.Invoke();
        }
    }
}