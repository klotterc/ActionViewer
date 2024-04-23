using Dalamud.Game.ClientState.Objects.SubKinds;

namespace ActionViewer.Models
{
    public class CharRow
    {
        public PlayerCharacter character { get; set; }
        public uint jobId { get; set; }
        public string playerName { get; set; }
        public StatusInfo statusInfo { get; set; }
    }
    public class StatusInfo
    {
        public int essenceId { get; set; }
        public int reraiserStatus { get; set; }
        public int leftId { get; set; }
        public int rightId { get; set; }
        public uint essenceIconID
        {
            get
            {
                if (essenceId >= 2311 && essenceId <= 2325)
                {
                    return 62386 + (uint)essenceId;
                }
                if (essenceId >= 2434 && essenceId <= 2439)
                {
                    return 62293 + (uint)essenceId;
                }
                return 26;
            }

        }
        public uint rightIconID
        {
            get
            {
                if (rightId > 0)
                {
                    return 64656 + (uint)rightId;
                }
                return 33;
            }
        }
        public uint leftIconID
        {
            get
            {
                if (leftId > 0)
                {
                    return 64656 + (uint)leftId;
                }
                return 33;
            }
        }
        public uint reraiserIconID
        {
            get
            {
                switch(reraiserStatus)
                {
                    case 1:
                        return 19301;
                    case 2:
                        return 19302;
                    default:
                        return 19321;
                }
            }
        }
    }
}
