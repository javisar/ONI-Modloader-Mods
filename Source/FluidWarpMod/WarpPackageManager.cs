using System.Collections.Generic;

namespace FluidWarpMod
{
    public static class WarpPackageManager
    {
        private static List<PacketData> liquidPackets = new List<PacketData>();

        private static List<PacketData> gasPackets = new List<PacketData>();

        private static float[] gasChannelMass = new float[10000];

        private static float[] liquidChannelMass = new float[10000];

        public static List<PacketData> getAvailablePackages(ConduitType conduitType)
        {
            switch (conduitType)
            {
                case LiquidWarpConfig.CONDUIT_TYPE:
                case ConduitType.Liquid:
                    return liquidPackets;
                case GasWarpConfig.CONDUIT_TYPE:
                case ConduitType.Gas:
                    return gasPackets;
                default:
                    return null; // muahahaha!!!
            }
        }

        public static float getTotalStoredMass(ConduitType conduitType, int channelNo)
        {
            if (conduitType == LiquidWarpConfig.CONDUIT_TYPE)
            {
                return liquidChannelMass[channelNo];
            }
            else
            {
                return gasChannelMass[channelNo];
            }
    }

        internal static float getMaxAllowedMass(ConduitType conduitType)
        {
            switch (conduitType)
            {
                case LiquidWarpConfig.CONDUIT_TYPE:
                case ConduitType.Liquid:
                    return 100.0f;
                case GasWarpConfig.CONDUIT_TYPE:
                case ConduitType.Gas:
                    return 50.0f;
                default:
                    return 0.0f;
            }
        }

        internal static void ProvideConduitContents(ConduitFlow flowManager, ConduitType conduitType, int fromCell, int channelNo, float mass)
        {
            if (getTotalStoredMass(conduitType, channelNo) < WarpPackageManager.getMaxAllowedMass(conduitType))
            {
                List<PacketData> availablePackets = getAvailablePackages(conduitType);
                ConduitFlow.ConduitContents removedContents = flowManager.RemoveElement(fromCell, mass);
                if (removedContents.mass > 0f)
                {
                    Logger.LogFormat("Adding Element to WarpSpace, mass={0}", mass);
                    availablePackets.Add(new PacketData(channelNo, conduitType, removedContents));
                    if (conduitType == LiquidWarpConfig.CONDUIT_TYPE)
                    {
                        liquidChannelMass[channelNo] += mass;
                        Logger.LogFormat("Total available liquid mass for channel {0} = {1}", channelNo, liquidChannelMass[channelNo]);
                    }
                    else
                    {
                        gasChannelMass[channelNo] += mass;
                        Logger.LogFormat("Total available gas mass for channel {0} = {1}", channelNo, gasChannelMass[channelNo]);
                    }
                }
            }
        }

        public static float RequestElementFromChannel(ConduitFlow flowManager, ConduitType conduitType, int toCell, int channelNo)
        {
            List<PacketData> availablePackets = getAvailablePackages(conduitType);
            float result = 0f;
            if (!flowManager.IsConduitFull(toCell))
            {
                foreach (PacketData packet in availablePackets)
                {
                    if (channelNo == packet.channelNo
                        && conduitType == packet.conduitType)
                    {
                        float num3 = flowManager.AddElement(toCell, packet.contents.element, packet.contents.mass, packet.contents.temperature, packet.contents.diseaseIdx, packet.contents.diseaseCount);
                        Logger.LogFormat("Adding Element to pipe: packet mass={0}, actually added mass={1}, element type={2}", packet.contents.mass, num3, packet.contents.element);
                        result += num3;
                        packet.contents.mass -= num3;
                        break;
                    }
                }
            }

            if (result > 0)
            {
                int removed = availablePackets.RemoveAll(_ => _.contents.mass <= 0);
                if (conduitType == LiquidWarpConfig.CONDUIT_TYPE)
                {
                    liquidChannelMass[channelNo] -= result;
                    Logger.LogFormat("Packets removed: {0}, total channel {1} mass left: {2}", removed, channelNo, liquidChannelMass[channelNo]);
                }
                else
                {
                    gasChannelMass[channelNo] -= result;
                    Logger.LogFormat("Packets removed: {0}, total channel {1} mass left: {2}", removed, channelNo, gasChannelMass[channelNo]);
                }
            }
            return result;
        }


    }
}
