using UnityEngine;
using Zenject;

namespace MistRidge
{
    public class PlayerFacade : Facade
    {
        private readonly Grounding grounding;

        public PlayerFacade(
            Grounding grounding,
            PlayerStateMachine playerStateMachine)
        {
            this.grounding = grounding;
        }

        public Vector3 GroundingPosition
        {
            get
            {
                return grounding.PrimaryPoint;
            }
        }

        public class Factory : FacadeFactory<Input, PlayerFacade>
        {
        }
    }
}
