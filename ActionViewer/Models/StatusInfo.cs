using Dalamud.Game.ClientState.Objects.SubKinds;

namespace ActionViewer.Models
{
    public class CharRow
    {
        public IPlayerCharacter character { get; set; }
        public uint jobId { get; set; }
        public string playerName { get; set; }
        public StatusInfo statusInfo { get; set; }
    }
    public class StatusInfo
    {
        public Lumina.Excel.GeneratedSheets2.Action? leftLuminaStatusInfo { get; set; }
        public Lumina.Excel.GeneratedSheets2.Action? rightLuminaStatusInfo { get; set; }
        public Lumina.Excel.GeneratedSheets2.Item? itemLuminaInfo { get; set; }
		public int essenceId { get; set; }
        public int reraiserStatus { get; set; }
        public string essenceName
        {
            get
            {
                if(itemLuminaInfo != null)
                {
                    return itemLuminaInfo.Name;
                }
                return null;
            }
        }
        public uint essenceIconID
        {
            get
            {
                if (itemLuminaInfo != null)
                {
                    return itemLuminaInfo.Icon;
                }
                return 26;
            }

        }
        public uint rightIconID
        {
            get
            {
                if (rightLuminaStatusInfo != null)
                {
				    return rightLuminaStatusInfo.Icon;
				}
                return 33;
            }
        }
        public uint leftIconID
        {
            get
            {
                if (leftLuminaStatusInfo != null)
                {
                    return leftLuminaStatusInfo.Icon;
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
