using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluidWarpMod
{
    public class PacketData
    {
        public float current_flow;
        public int cell_idx;
        public SimHashes element;
        public float mass;
        public float temperature;
        public byte disease_idx;
        public int disease_count;

        public PacketData(float current_flow, int cell_idx, SimHashes element, float mass, float temperature, byte disease_idx, int disease_count)
        {
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
