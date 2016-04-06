using UnityEngine;
using System;
using Zenject;

namespace MistRidge
{
    public class SprintManager : IInitializable
    {
        private readonly DisplayManager displayManager;

        private int currentSprintNum;
        private int sprintCount;

        public SprintManager(DisplayManager displayManager)
        {
            this.displayManager = displayManager;
        }

        public void Initialize()
        {
            currentSprintNum = 0;
        }

        public void SetSprintCount(int count)
        {
            sprintCount = count;
        }

        public void SetSprintNum(int count)
        {
            currentSprintNum = count;
        }

        public void UpdateSprintText()
        {
            displayManager.UpdateCinematic(true);
            displayManager.UpdateSprint(true);
            displayManager.UpdateSprintText(currentSprintNum, sprintCount);
        }
    }
}
