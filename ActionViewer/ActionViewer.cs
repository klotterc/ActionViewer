using Dalamud.Game.ClientState.Objects.SubKinds;
using Dalamud.Game.ClientState.Objects.Types;
using Dalamud.Utility;
using System.Collections.Generic;

namespace ActionViewer
{
    public interface IActionViewer
    {
        List<IPlayerCharacter> getPlayerCharacters();
    }

    public sealed unsafe class ActionViewer : IActionViewer
    {
        public List<IPlayerCharacter> getPlayerCharacters()
        {
            var playerCharacters = new List<IPlayerCharacter>();

            foreach (IGameObject gameObject in Services.Objects)
            {
				IPlayerCharacter? playerCharacter = gameObject as IPlayerCharacter;

                if (playerCharacter == null || playerCharacter.ClassJob.Id == 0)//|| playerCharacter == Services.ClientState.LocalPlayer)
                {
                    continue;
                }

                playerCharacters.Add((gameObject as IPlayerCharacter)!);
            }

            return playerCharacters;
        }
    }
}
