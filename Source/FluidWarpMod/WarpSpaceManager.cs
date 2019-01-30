using Harmony;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace FluidWarpMod
{
    internal class ValvesList : List<ValveBase>
    {
        private int currentRequestorItem;

        private int currentProviderItem;

        private ConduitFlow flowManager;

        public ConduitFlow FlowManager
        {
            get
            {
                return flowManager;
            }
        }

        public ValvesList(ConduitFlow flowManager) : base()
        {
            currentProviderItem = 0;
            currentRequestorItem = 0;
            this.flowManager = flowManager;
        }

        private ValvesList()
        {

        }

        private bool isValidRequestor(ValveBase warpGate)
        {
            return warpGate != null && flowManager.HasConduit(warpGate.GetOutputCell());
        }

        private bool isValidProvider(ValveBase warpGate)
        {
            return warpGate != null && flowManager.HasConduit(warpGate.GetInputCell());
        }

        public ValveBase getCurrentRequestor()
        {
            if (Count == 0)
            {
                return null;
            }
            if (isValidRequestor(base[currentRequestorItem]))
            {
                return base[currentRequestorItem];
            }
            else
            {
                return getNextRequestor();
            }
        }

        public ValveBase getNextRequestor()
        {
            if (Count == 0)
            {
                return null;
            }

            for (int i = 0; i < Count; i++)
            {
                if (++currentRequestorItem >= Count)
                {
                    currentRequestorItem = 0;
                }
                ValveBase warpGate = base[currentRequestorItem];
                if (isValidRequestor(warpGate))
                {
                    return warpGate;
                }
            };
            return null;
        }

        public ValveBase getNextProvider()
        {
            if (Count == 0)
            {
                return null;
            }

            for (int i=0; i < Count; i++)
            {
                if (++currentProviderItem >= Count)
                {
                    currentProviderItem = 0;
                }
                ValveBase warpGate = base[currentProviderItem];
                if (isValidProvider(warpGate))
                {
                    return warpGate;
                }
            };
            return null;
        }

        public new bool Remove(ValveBase valveBaseToRemove)
        {
            bool result = base.Remove(valveBaseToRemove);
            if (result)
            {
                if (currentProviderItem >= Count)
                {
                    currentProviderItem = 0;
                }
                if (currentRequestorItem >= Count)
                {
                    currentRequestorItem = 0;
                }
            }
            return result;
        }
    }

    internal class ValveChannels : Dictionary<int, ValvesList> { }

    static class WarpSpaceManager
    {
        private static bool isUpdaterRegistered = false;

        private static ValveChannels gasChannels = new ValveChannels();
 
        private static ValveChannels liquidChannels = new ValveChannels();

        private static ValveChannels getChannelsForConduitType(ConduitType conduitType)
        {
            return (conduitType == LiquidWarpConfig.CONDUIT_TYPE ? liquidChannels : gasChannels);
        }

        public static void RegisterConduitUpdater()
        {
            if (!isUpdaterRegistered)
            {
                Logger.Log("WarpSpaceManager.RegisterConduitUpdater start");
                Conduit.GetFlowManager(ConduitType.Gas).AddConduitUpdater(new Action<float>(WarpSpaceManager.GasConduitUpdate), ConduitFlowPriority.Default);
                Conduit.GetFlowManager(ConduitType.Liquid).AddConduitUpdater(new Action<float>(WarpSpaceManager.LiquidConduitUpdate), ConduitFlowPriority.Default);
                isUpdaterRegistered = true;
                Logger.Log("WarpSpaceManager.RegisterConduitUpdater end");
            }
        }

        public static void UnregisterConduitUpdate()
        {
            if (isUpdaterRegistered)
            {
                Logger.Log("WarpSpaceManager.UnregisterConduitUpdate start");
                Conduit.GetFlowManager(ConduitType.Gas).RemoveConduitUpdater(new Action<float>(WarpSpaceManager.GasConduitUpdate));
                Conduit.GetFlowManager(ConduitType.Liquid).RemoveConduitUpdater(new Action<float>(WarpSpaceManager.LiquidConduitUpdate));
                isUpdaterRegistered = false;
                Logger.Log("WarpSpaceManager.UnregisterConduitUpdate end");
            }
        }

        private static ConduitFlow getFlowManager(ConduitType conduitType)
        {
            switch (conduitType)
            {
                case LiquidWarpConfig.CONDUIT_TYPE:
                    return Conduit.GetFlowManager(ConduitType.Liquid);
                case GasWarpConfig.CONDUIT_TYPE:
                    return Conduit.GetFlowManager(ConduitType.Gas);
                default:
                    return null;
            }
        }

        public static void SetProviderValveChannel(ValveBase valveBase, int newChannel)
        {
            Logger.LogFormat("==Enter WaprSpaceManager.SetProviderValveChannel(valveBase={0}, valveChannel={1})", valveBase.GetInstanceID(), newChannel);
            ValveChannels providers = getChannelsForConduitType(valveBase.conduitType);
            foreach (var item in providers)
            {
                if (null != item.Value)
                {
                    item.Value.Remove(valveBase);
                }
            }
            ValvesList valves;
            if (!providers.TryGetValue(newChannel, out valves))
            {
                valves = new ValvesList(getFlowManager(valveBase.conduitType));
                providers[newChannel] = valves;
            }

            valves.Add(valveBase);
            Logger.Log("==Exit WaprSpaceManager.SetProviderValveChannel");
        }

        public static void OnValveChannelChange(ValveBase valveBase)
        {
            Logger.LogFormat("==Enter WarpSpaceManager.OnValveChannelChange(valveBase={0} conduitType={1} inputCell={2} outputCell={3})", 
                valveBase.GetInstanceID(), 
                valveBase.conduitType, 
                valveBase.GetInputCell(), 
                valveBase.GetOutputCell());
            ConduitFlow flowManager = getFlowManager(valveBase.conduitType);

            if (flowManager == null)
            {
                return;
            }

            int newChannel = Mathf.RoundToInt(valveBase.CurrentFlow * 1000f);

            SetProviderValveChannel(valveBase, newChannel);
            Logger.Log("==Exit WarpSpaceManager.OnValveChannelChange");
        }

        // Tries to request fluid for current requestor in specified list of Warp Gates
        // returns false when all providers are dry at the moment and no more fluid can be transfered in this channel
        private static bool RequestFluid(ValvesList warpGates)
        {
            var requestor = warpGates.getCurrentRequestor();
            var provider = warpGates.getNextProvider();
            var start = provider;
            if (null == provider || null == requestor)
            {
                return false;
            }
            int toCell = requestor.GetOutputCell();
            var flowManager = warpGates.FlowManager;
            // Fill input cell from various providers, in case when provider's conduit is not full
            do
            {
                int fromCell = provider.GetInputCell();
                if (provider != requestor)
                {
                    ConduitFlow.Conduit providerConduit = flowManager.GetConduit(fromCell);
                    ConduitFlow.ConduitContents providerContents = providerConduit.GetContents(flowManager);
                    if (!SimHashes.Vacuum.Equals(providerContents.element))
                    {
#if DEBUG
                        ConduitFlow.Conduit requestorConduit = flowManager.GetConduit(toCell);
                        ConduitFlow.ConduitContents requestorContents = requestorConduit.GetContents(flowManager);
                        Logger.LogFormat("Trying to move {0} kg. of {1} from {2} to {3}", providerContents.mass, providerContents.element, fromCell, toCell);
                        Logger.LogFormat("Requestor contents is: {0} kg. of {1}", requestorContents.mass, requestorContents.element);
#endif
                        float addedMass = flowManager.AddElement(toCell, providerContents.element, providerContents.mass, providerContents.temperature, providerContents.diseaseIdx, providerContents.diseaseCount);
                        Game.Instance.accumulators.Accumulate(provider.AccumulatorHandle, addedMass);
                        if (addedMass > 0f)
                        {
#if DEBUG
                            Logger.LogFormat("Moved {0} kg. from {1} to {2}", addedMass, fromCell, toCell);
#endif
                            ConduitFlow.ConduitContents removed = flowManager.RemoveElement(providerConduit, addedMass);
                            Game.Instance.accumulators.Accumulate(requestor.AccumulatorHandle, addedMass);
                        }
                    }
                }
                if (flowManager.IsConduitFull(toCell))
                {
                    return true;
                }
                provider = warpGates.getNextProvider();
            } while (provider != start);
            return false;
        }

        private static void UpdateConduitsOfWarpGates(float dt, ConduitType warpGateType)
        {
            try
            {
                ConduitFlow flowManager = getFlowManager(warpGateType);
                if (flowManager == null)
                {
                    Logger.Log("unable to determine correct ConduitType.");
                    return;
                }

                ValveChannels channels = getChannelsForConduitType(warpGateType);

                foreach (KeyValuePair<int, ValvesList> warpChannel in channels)
                {
                    if (warpChannel.Key == 10000)
                    {
                        continue;
                    }
                    var warpValves = warpChannel.Value;
                    var startRequestor = warpValves.getCurrentRequestor();
                    if (startRequestor == null)
                    {
                        continue;
                    }
                    do
                    {
                        int destinationCell = warpValves.getCurrentRequestor().GetOutputCell();
                        if (!flowManager.IsConduitFull(destinationCell) && !RequestFluid(warpValves))
                        {
                            break;
                        }
                        warpValves.getNextRequestor();
                    } while (startRequestor != warpValves.getCurrentRequestor());
                }
            }
            catch (Exception ex)
            {
                Logger.LogFormat("Exception in WarpSpaceManager.UpdateConduitsOfWarpGates: {0}\n{1}", ex.Message, ex.StackTrace);
            }
        }

        private static void GasConduitUpdate(float dt)
        {
            UpdateConduitsOfWarpGates(dt, GasWarpConfig.CONDUIT_TYPE);
        }

        private static void LiquidConduitUpdate(float dt)
        {
            UpdateConduitsOfWarpGates(dt, LiquidWarpConfig.CONDUIT_TYPE);
        }

        public static void RemoveProviderValve(ValveBase valveBase)
        {
            Logger.LogFormat("==Enter WarpSpaceManager.RemoveProviderValve(valveBase={0})", valveBase);
            ValveChannels providers = getChannelsForConduitType(valveBase.conduitType);
            int totalWarpGates = 0;
            foreach (var channel in providers)
            {
                channel.Value.Remove(valveBase);
                totalWarpGates += channel.Value.Count;
            }
            if (totalWarpGates == 0)
            {
                UnregisterConduitUpdate();
            }
            Logger.Log("==Exit WarpSpaceManager.RemoveProviderValve");
        }
    }
}
