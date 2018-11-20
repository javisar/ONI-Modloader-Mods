using System;
namespace FluidWarpMod
{

	public class PacketData
    {
        public int channelNo;
        public ConduitType conduitType;
        public ConduitFlow.ConduitContents contents;

        public PacketData(int channelNo, ConduitType conduitType, ConduitFlow.ConduitContents contents)
        {
			this.channelNo = channelNo;
            this.conduitType = conduitType;
            this.contents = contents;
        }
    }
}
