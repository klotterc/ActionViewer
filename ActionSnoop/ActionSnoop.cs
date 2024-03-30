using Dalamud.Game.ClientState.Objects.SubKinds;
using Dalamud.Game.ClientState.Objects.Types;
using System.Collections.Generic;

namespace ActionSnoop
{
    public interface IActionSnoop
    {
        //const uint AethericMimicryActionID = 18322;

        //void MimicRole(IMimicryRole mimicryRole
        List<PlayerCharacter> getPlayerCharacters();
        List<PlayerCharacter> getDummyList();
    }

    public sealed unsafe class ActionSnoop : IActionSnoop
    {
        public List<PlayerCharacter> getPlayerCharacters()
        {
            var playerCharactersMatchingRole = new List<PlayerCharacter>();

            foreach (GameObject gameObject in Services.Objects)
            {
                PlayerCharacter? playerCharacter = gameObject as PlayerCharacter;

                if (playerCharacter == null)//|| playerCharacter == Services.ClientState.LocalPlayer)
                {
                    continue;
                }

                playerCharactersMatchingRole.Add((gameObject as PlayerCharacter)!);
            }

            return playerCharactersMatchingRole;
        }
        public List<PlayerCharacter> getDummyList()
        {
            var playerCharactersMatchingRole = new List<PlayerCharacter>();

            foreach (GameObject gameObject in Services.Objects)
            {
                PlayerCharacter? playerCharacter = gameObject as PlayerCharacter;

                if (playerCharacter == null)//|| playerCharacter == Services.ClientState.LocalPlayer)
                {
                    continue;
                }
                for (int i = 0; i < 16; i++)
                {
                    playerCharactersMatchingRole.Add((gameObject as PlayerCharacter)!);
                }
                break;
            }

            return playerCharactersMatchingRole;
        }

        /*public void MimicRole(IMimicryRole mimicryRole)
        {
            var playerCharactersMatchingRole = new List<PlayerCharacter>();
            foreach (MimicryRoleKind mimicryRoleKind in mimicryRole.RoleKinds)
            {
                playerCharactersMatchingRole.AddRange(PlayerCharactersMatchingRole(mimicryRoleKind));
            }

            var closestPlayer = ClosestToLocalPlayer(playerCharactersMatchingRole);

            if (closestPlayer == null)
            {
                Services.Chat.Print("Could not find a character to mimic.");
                return;
            }

            Services.Chat.Print($"Attempting to mimic {closestPlayer.Name} in the {GetRelativeCompassDirection(closestPlayer)} direction...");
            ActionManager.Instance()->UseAction(ActionType.Action, IMimicryMaster.AethericMimicryActionID, closestPlayer.ObjectId);
        }*/
    }
}
