using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarpMod
{
	public static class LiquidWarpData
	{
		public static List<PacketData> LiquidPackets = new List<PacketData>();

	}

	public static class GasWarpData
	{
		public static List<PacketData> GasPackets = new List<PacketData>();

	}

	public class PacketData
    {
		public int content_type;
        public float current_flow;
        public int cell_idx;
        public SimHashes element;
        public float mass;
        public float temperature;
        public byte disease_idx;
        public int disease_count;

        public PacketData(int content_type, float current_flow, int cell_idx, SimHashes element, float mass, float temperature, byte disease_idx, int disease_count)
        {
			this.content_type = content_type;
			this.current_flow = current_flow;
            this.cell_idx = cell_idx;
            this.element = element;
            this.mass = mass;
            this.temperature = temperature;
            this.disease_idx = disease_idx;
            this.disease_count = disease_count;
        }
    }
}
