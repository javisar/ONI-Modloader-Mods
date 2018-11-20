using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarpMod
{
    public static class WarpPackageManager
    {
        private static Dictionary<ConduitType, List<PacketData>> availablePackets = new Dictionary<ConduitType, List<PacketData>>();
        public static List<PacketData> getAvailablePackages(ConduitType conduitType)
        {
            List<PacketData> result;
            if (!availablePackets.TryGetValue(conduitType, out result))
            {
                result = new List<PacketData>();
                availablePackets.Add(conduitType, result);
            }
            return result;
        }

        public static void addPackage(PacketData packetData, ConduitType conduitType)
        {
            List<PacketData> pd = getAvailablePackages(conduitType);
            pd.Add(packetData);
        }

        public static float getTotalStoredMass(ConduitType conduitType, int channelNo)
        {
            float result = 0.0f;
            foreach (PacketData packetData in getAvailablePackages(conduitType))
            {
                if (channelNo == packetData.channel_no)
                {
                    result = result + packetData.mass;
                }
            }
            return result;
        }

        internal static float getMaxContent(ConduitType conduitType)
        {
            switch ((int)conduitType)
            {
                case 100:
                    return 20.0f;
                case 101:
                    return 2.0f;
                default:
                    return 0.0f;
            }
        }
    }
}
