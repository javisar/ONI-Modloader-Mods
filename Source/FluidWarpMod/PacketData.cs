using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarpMod
{

	public class PacketData
    {
		public int content_type;
        public int channel_no;
        public int cell_idx;
        public SimHashes element;
        public float mass;
        public float temperature;
        public byte disease_idx;
        public int disease_count;

        public PacketData(int content_type, int channel_no, int cell_idx, SimHashes element, float mass, float temperature, byte disease_idx, int disease_count)
        {
			this.content_type = content_type;
			this.channel_no = channel_no;
            this.cell_idx = cell_idx;
            this.element = element;
            this.mass = mass;
            this.temperature = temperature;
            this.disease_idx = disease_idx;
            this.disease_count = disease_count;
        }
    }
}
