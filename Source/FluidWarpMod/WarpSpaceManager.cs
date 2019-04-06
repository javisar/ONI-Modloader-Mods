using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Harmony;
using UnityEngine;

namespace FluidWarpMod
{
    internal class ValveBaseExt
    {
        public int ID { get; private set; }
        public int Channel { get; set; }
        public ValveBase ValveBase { get; private set; }
        public ConduitFlow FlowManager { get; private set; }

        public ValveBaseExt(ValveBase vb)
        {
            this.ValveBase = vb;
            this.ID = ValveBase.GetInstanceID();
            this.FlowManager = getFlowManager();
        }

        public bool isValidRequestor()
        {
            return ValveBase != null && FlowManager.HasConduit(ValveBase.GetOutputCell());
        }

        public bool isValidProvider()
        {
            return ValveBase != null && FlowManager.HasConduit(ValveBase.GetInputCell());
        }

        private ConduitFlow getFlowManager()
        {
            switch (ValveBase.conduitType)
            {
                case FluidWarpMod_Utils.LIQUID_CONDUIT_PROVIDER_TYPE:
                case FluidWarpMod_Utils.LIQUID_CONDUIT_REQUESTER_TYPE:
                    return Conduit.GetFlowManager(ConduitType.Liquid);

                case FluidWarpMod_Utils.GAS_CONDUIT_PROVIDER_TYPE:
                case FluidWarpMod_Utils.GAS_CONDUIT_REQUESTER_TYPE:
                    return Conduit.GetFlowManager(ConduitType.Gas);

                default:
                    return null;
            }
        }
    }

    internal class ValvesList : List<ValveBase>
    {
        private int currentRequestorItem;

        private int currentProviderItem;

        private ConduitFlow flowManager;
        public ConduitFlow FlowManager { get { return flowManager; } }

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

            for (int i = 0; i < Count; i++)
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

    internal static class WarpSpaceManager
    {
        private static bool isUpdaterRegistered = false;
        private static Dictionary<int, ValveBaseExt> _gasRequesters = new Dictionary<int, ValveBaseExt>();
        private static Dictionary<int, ValveBaseExt> _gasProviders = new Dictionary<int, ValveBaseExt>();
        private static Dictionary<int, ValveBaseExt> _liquidRequesters = new Dictionary<int, ValveBaseExt>();
        private static Dictionary<int, ValveBaseExt> _liquidProviders = new Dictionary<int, ValveBaseExt>();

        private static ValveChannels gasChannels = new ValveChannels();

        private static ValveChannels liquidChannels = new ValveChannels();

        private static ValveChannels getChannelsForConduitType(ConduitType conduitType)
        {
            return (conduitType == FluidWarpMod_Utils.LIQUID_CONDUIT_PROVIDER_TYPE ? liquidChannels : gasChannels);
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
        private static Dictionary<int, ValveBaseExt> GetWarpGateDictionary(ValveBase vb)
        {
            switch (vb.conduitType)
            {
                case FluidWarpMod_Utils.LIQUID_CONDUIT_PROVIDER_TYPE:
                    return _liquidProviders;
                case FluidWarpMod_Utils.LIQUID_CONDUIT_REQUESTER_TYPE:
                    return _liquidRequesters;

                case FluidWarpMod_Utils.GAS_CONDUIT_PROVIDER_TYPE:
                    return _gasProviders;
                case FluidWarpMod_Utils.GAS_CONDUIT_REQUESTER_TYPE:
                    return _gasRequesters;

                default:
                    return null;
            }
        }

        private static ConduitFlow getFlowManager(ConduitType conduitType)
        {
            switch (conduitType)
            {
                case FluidWarpMod_Utils.LIQUID_CONDUIT_PROVIDER_TYPE:
                case FluidWarpMod_Utils.LIQUID_CONDUIT_REQUESTER_TYPE:
                    return Conduit.GetFlowManager(ConduitType.Liquid);

                case FluidWarpMod_Utils.GAS_CONDUIT_PROVIDER_TYPE:
                case FluidWarpMod_Utils.GAS_CONDUIT_REQUESTER_TYPE:
                    return Conduit.GetFlowManager(ConduitType.Gas);

                default:
                    return null;
            }
        }

        public static void RegisterWarpGate(ValveBase valveBase, int newChannel)
        {
            Logger.LogFormat("==Enter WarpSpaceManager.RegisterWarpGate(valveBase={0}, valveChannel={1})", valveBase.GetInstanceID(), newChannel);
            var sourceDict = GetWarpGateDictionary(valveBase);
            var key = valveBase.GetInstanceID();
            ValveBaseExt baseExt;
            if (sourceDict.TryGetValue(key, out baseExt))
            {
                baseExt.Channel = newChannel;
            }
            else
            {
                baseExt = new ValveBaseExt(valveBase);
                baseExt.Channel = newChannel;
                sourceDict.Add(key, baseExt);
            }


            //ValveChannels providers = getChannelsForConduitType(valveBase.conduitType);
            //foreach (var item in providers)
            //{
            //    if (null != item.Value)
            //    {
            //        item.Value.Remove(valveBase);
            //    }
            //}
            //ValvesList valves;
            //if (!providers.TryGetValue(newChannel, out valves))
            //{
            //    valves = new ValvesList(getFlowManager(valveBase.conduitType));
            //    providers[newChannel] = valves;
            //}

            //valves.Add(valveBase);
            Logger.Log("==Exit WarpSpaceManager.RegisterWarpGate");
        }

        public static void OnWarpGateChannelChange(ValveBase valveBase)
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

            RegisterWarpGate(valveBase, newChannel);
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
                        ConduitFlow.Conduit requestorConduit = flowManager.GetConduit(toCell);
                        ConduitFlow.ConduitContents requestorContents = requestorConduit.GetContents(flowManager);
#if DEBUG
                        Logger.LogFormat("Trying to move {0} kg. of {1} from {2} to {3}", providerContents.mass, providerContents.element, fromCell, toCell);
                        Logger.LogFormat("Requestor contents is: {0} kg. of {1}", requestorContents.mass, requestorContents.element);
#endif
                        if (requestorContents.mass < 1f && requestorContents.element != providerContents.element && requestorContents.element != SimHashes.Vacuum)
                        {
                            Logger.LogFormat("Removing contents is: {0} kg. of {1}", requestorContents.mass, requestorContents.element);
                            flowManager.RemoveElement(requestorConduit, requestorContents.mass);
                        }
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

        private static void RequestFluid2(List<ValveBaseExt> requesters, List<ValveBaseExt> providers)
        {
            Logger.Log("WarpSpaceManager.RequestFluid2 start");

            Logger.Log(String.Format("WarpSpaceManager.RequestFluid2 requestor count: {0}", requesters.Count));
            Logger.Log(String.Format("WarpSpaceManager.RequestFluid2 providers count: {0}", providers.Count));

            foreach (var provider in providers)
            {
                if (!provider.isValidProvider())
                    continue;
                var flowManager = provider.FlowManager;
                int fromCell = provider.ValveBase.GetInputCell();
                ConduitFlow.Conduit providerConduit = flowManager.GetConduit(fromCell);
                ConduitFlow.ConduitContents providerContents = providerConduit.GetContents(flowManager);
                if (SimHashes.Vacuum.Equals(providerContents.element))
                    continue;

                var matchedRequesters = requesters.Where(x => x.Channel == provider.Channel).ToList();
                if (matchedRequesters.Count == 0)
                    continue;
                var splitMass = providerContents.mass / matchedRequesters.Count();
                foreach (var requester in matchedRequesters)
                {
                    if (!requester.isValidRequestor())
                        continue;
                    int toCell = requester.ValveBase.GetOutputCell();
                    ConduitFlow.Conduit requestorConduit = flowManager.GetConduit(toCell);
                    ConduitFlow.ConduitContents requestorContents = requestorConduit.GetContents(flowManager);

                    if (requestorContents.mass < 1f && requestorContents.element != providerContents.element && requestorContents.element != SimHashes.Vacuum)
                    {
                        Logger.LogFormat("Removing contents is: {0} kg. of {1}", requestorContents.mass, requestorContents.element);
                        flowManager.RemoveElement(requestorConduit, requestorContents.mass);
                    }
                    float addedMass = flowManager.AddElement(toCell, providerContents.element, splitMass, providerContents.temperature, providerContents.diseaseIdx, providerContents.diseaseCount);
                    Game.Instance.accumulators.Accumulate(provider.ValveBase.AccumulatorHandle, addedMass);
                    if (addedMass > 0f)
                    {
                        ConduitFlow.ConduitContents removed = flowManager.RemoveElement(providerConduit, addedMass);
                        Game.Instance.accumulators.Accumulate(requester.ValveBase.AccumulatorHandle, addedMass);
                        Logger.LogFormat("Moved {0} kg. from {1} to {2}. ", addedMass, fromCell, toCell);
                    }

                    if (flowManager.IsConduitFull(toCell))
                    {
                        break;
                    }
                }
            }

            Logger.Log("WarpSpaceManager.RequestFluid2 End");
        }

        private static void UpdateConduitsOfWarpGates(float dt, ConduitType warpGateType)
        {
            try
            {
                if (warpGateType == ConduitType.Liquid)
                {
                    var requesters = _liquidRequesters.Values.Where(x => x.Channel != 1000).ToList();
                    var providers = _liquidProviders.Values.Where(x => x.Channel != 1000).ToList();
                    if (requesters.Count == 0 || providers.Count == 0)
                        return;
                    RequestFluid2(requesters, providers);
                }
                else
                {
                    var requesters = _gasRequesters.Values.Where(x => x.Channel != 1000).ToList();
                    var providers = _gasProviders.Values.Where(x => x.Channel != 1000).ToList();
                    if (requesters.Count == 0 || providers.Count == 0)
                        return;
                    RequestFluid2(requesters, providers);
                }
                //ConduitFlow flowManager = getFlowManager(warpGateType);
                //if (flowManager == null)
                //{
                //    Logger.Log("unable to determine correct ConduitType.");
                //    return;
                //}

                //ValveChannels channels = getChannelsForConduitType(warpGateType);

                //foreach (KeyValuePair<int, ValvesList> warpChannel in channels)
                //{
                //    if (warpChannel.Key == 10000)
                //    {
                //        continue;
                //    }
                //    var warpValves = warpChannel.Value;
                //    var startRequestor = warpValves.getCurrentRequestor();
                //    if (startRequestor == null)
                //    {
                //        continue;
                //    }
                //    do
                //    {
                //        int destinationCell = warpValves.getCurrentRequestor().GetOutputCell();
                //        if (!flowManager.IsConduitFull(destinationCell) && !RequestFluid(warpValves))
                //        {
                //            break;
                //        }
                //        warpValves.getNextRequestor();
                //    } while (startRequestor != warpValves.getCurrentRequestor());
                //}
            }
            catch (Exception ex)
            {
                Logger.LogFormat("Exception in WarpSpaceManager.UpdateConduitsOfWarpGates: {0}\n{1}", ex.Message, ex.StackTrace);
            }
        }

        private static void GasConduitUpdate(float dt)
        {
            UpdateConduitsOfWarpGates(dt, ConduitType.Gas);
        }

        private static void LiquidConduitUpdate(float dt)
        {
            UpdateConduitsOfWarpGates(dt, ConduitType.Liquid);
        }

        public static void RemoveWarpGate(ValveBase valveBase)
        {
            Logger.LogFormat("==Enter WarpSpaceManager.RemoveWarpGate(valveBase={0})", valveBase);
            //ValveChannels providers = getChannelsForConduitType(valveBase.conduitType);
            //int totalWarpGates = 0;
            //foreach (var channel in providers)
            //{
            //    channel.Value.Remove(valveBase);
            //    totalWarpGates += channel.Value.Count;
            //}
            //if (totalWarpGates == 0)
            //{
            //    UnregisterConduitUpdate();
            //}
            var sourceDict = GetWarpGateDictionary(valveBase);
            var key = valveBase.GetInstanceID();
            if (sourceDict.ContainsKey(key))
            {
                sourceDict.Remove(key);
            }
            if (_liquidProviders.Count() + _liquidRequesters.Count() + _gasRequesters.Count() + _gasProviders.Count() == 0)
            {
                UnregisterConduitUpdate();
            }
            Logger.Log("==Exit WarpSpaceManager.RemoveWarpGate");
        }
    }
}