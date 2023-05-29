// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: "DotAnimationC.cs" 
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace Appearition.Example
{
    /// <summary>
    /// Object handling a dot animation on a UI object. Once initialized, call the UpdateDisplay method during the waiting loop.
    /// </summary>
    public class DotAnimation : IDisposable
    {
        private List<Button> buttonsToTurnOff;
        private Text displayText;
        private string _backupText;
        private string _tmpText;
        private float _startTime;
        private float _delayBetweenDots;
        private int _maxAmountOfDots;

        /// <summary>
        /// UI animation object which adds dots at the end of a given text. Best used in a USING statement around a loop in a coroutine.
        /// </summary>
        /// <param name="newDisplayText">New display text.</param>
        /// <param name="newDelayBetweenDots">New delay between dots.</param>
        /// <param name="newMaxAmountOfDots">New max amount of dots.</param>
        public DotAnimation(Text newDisplayText, float newDelayBetweenDots, int newMaxAmountOfDots)
        {
            displayText = newDisplayText;
            _backupText = (displayText == null ? "" : displayText.text);
            _startTime = Time.time;
            _delayBetweenDots = newDelayBetweenDots;
            _maxAmountOfDots = newMaxAmountOfDots;
        }

        /// <summary>
        /// UI animation object which adds dots at the end of a given text, and changes the interactive state of a single button. 
        /// Best used in a USING statement around a loop in a coroutine.
        /// </summary>
        /// <param name="newButtonToTurnOff">New button to turn off.</param>
        /// <param name="newDisplayText">New display text.</param>
        /// <param name="newDelayBetweenDots">New delay between dots.</param>
        /// <param name="newMaxAmountOfDots">New max amount of dots.</param>
        public DotAnimation(Button newButtonToTurnOff, Text newDisplayText, float newDelayBetweenDots, int newMaxAmountOfDots)
        {
            //Store the new variables
            buttonsToTurnOff = new List<Button>();
            buttonsToTurnOff.Add(newButtonToTurnOff);
            displayText = newDisplayText;
            _backupText = (displayText == null ? "" : displayText.text);
            _startTime = Time.time;
            _delayBetweenDots = newDelayBetweenDots;
            _maxAmountOfDots = newMaxAmountOfDots;

            //Initialize them
            ChangeButtonsState(false);
        }

        /// <summary>
        /// UI animation object which adds dots at the end of a given text, and changes the interactible state of several button. 
        /// Best used in a USING statement around a loop in a coroutine.
        /// </summary>
        /// <param name="newButtonsToTurnOff">New buttons to turn off.</param>
        /// <param name="newDisplayText">New display text.</param>
        /// <param name="newDelayBetweenDots">New delay between dots.</param>
        /// <param name="newMaxAmountOfDots">New max amount of dots.</param>
        public DotAnimation(List<Button> newButtonsToTurnOff, Text newDisplayText, float newDelayBetweenDots, int newMaxAmountOfDots)
        {
            //Store the new variables
            buttonsToTurnOff = newButtonsToTurnOff;
            displayText = newDisplayText;
            _backupText = (displayText == null ? "" : displayText.text);
            _startTime = Time.time;
            _delayBetweenDots = newDelayBetweenDots;
            _maxAmountOfDots = newMaxAmountOfDots;

            //Initialize them
            ChangeButtonsState(false);
        }

        public void UpdateDisplay()
        {
            _tmpText = "";
            for (int i = 0; i < ((Time.time - _startTime) / _delayBetweenDots) % _maxAmountOfDots; i++)
            {
                _tmpText += ".";
                if (i < _maxAmountOfDots)
                    _tmpText += " ";
            }

            displayText.text = _backupText + " " + _tmpText;
        }

        void ChangeButtonsState(bool shouldBeInteractible)
        {
            if (buttonsToTurnOff != null)
            {
                for (int i = 0; i < buttonsToTurnOff.Count; i++)
                {
                    if (buttonsToTurnOff[i] != null)
                        buttonsToTurnOff[i].interactable = shouldBeInteractible;
                }
            }
        }

        /// <summary>
        /// Called whenever the object is not used anymore.
        /// </summary>
        /// <remarks>Call <see cref="Dispose"/> when you are finished using the <see cref="Appearition.Example.DotAnimation"/>. The
        /// <see cref="Dispose"/> method leaves the <see cref="Appearition.Example.DotAnimation"/> in an unusable state. After
        /// calling <see cref="Dispose"/>, you must release all references to the
        /// <see cref="Appearition.Example.DotAnimation"/> so the garbage collector can reclaim the memory that the
        /// <see cref="Appearition.Example.DotAnimation"/> was occupying.</remarks>
        public void Dispose()
        {
            if (displayText != null)
                displayText.text = _backupText;
            ChangeButtonsState(true);
        }
    }
}