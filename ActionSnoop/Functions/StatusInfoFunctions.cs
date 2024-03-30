using ActionSnoop.Models;
using Dalamud.Game.ClientState.Statuses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActionSnoop.Functions
{
    public static class StatusInfoFunctions
    {
        private static List<int> essenceIds = new List<int>() { 2311, 2312, 2313, 2314, 2315, 2316, 2317, 2318, 2319, 2320, 2321, 2322, 2323, 2324, 2325, 2434, 2435, 2436, 2437, 2438, 2439, };
        public static StatusInfo GetStatusInfo(StatusList statusList)
        {
            int leftId = 0;
            StatusInfo statusInfo = new StatusInfo();

            foreach (Status status in statusList)
            {
                int statusId = (int)status.StatusId;
                if (essenceIds.Contains(statusId))
                {
                    statusInfo.essenceId = statusId;
                }
                if (statusId.Equals(2348))
                {
                    statusInfo.leftId = status.Param % 256;
                    statusInfo.rightId = (status.Param - leftId) / 256;

                    if (statusInfo.leftId == 71 || statusInfo.leftId == 72)
                    {
                        statusInfo.leftId += 6;
                    }
                    if (statusInfo.rightId == 71 || statusInfo.rightId == 72)
                    {
                        statusInfo.rightId += 6;
                    }
                }
            }
            return statusInfo;
        }
    }
}
