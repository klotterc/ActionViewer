using Dalamud.Game.ClientState.Objects.SubKinds;
using Dalamud.Game.ClientState.Objects.Types;
using System.Collections.Generic;

namespace ActionViewer
{
    public interface IActionViewer
    {
        List<PlayerCharacter> getPlayerCharacters();
    }

    public sealed unsafe class ActionViewer : IActionViewer
    {
        public List<PlayerCharacter> getPlayerCharacters()
        {
            var playerCharacters = new List<PlayerCharacter>();

            foreach (GameObject gameObject in Services.Objects)
            {
                PlayerCharacter? playerCharacter = gameObject as PlayerCharacter;

                if (playerCharacter == null)//|| playerCharacter == Services.ClientState.LocalPlayer)
                {
                    continue;
                }

                playerCharacters.Add((gameObject as PlayerCharacter)!);
            }

            return playerCharacters;
        }
    }
}
