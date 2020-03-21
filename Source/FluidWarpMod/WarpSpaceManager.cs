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

            //Logger.Log(String.Format("WarpSpaceManager.RequestFluid2 Is valid requestor {0}", FlowManager.HasConduit(ValveBase.GetOutputCell())));
            return ValveBase != null && FlowManager.HasConduit(ValveBase.GetOutputCell());
        }

        public bool isValidProvider()
        {
            //Logger.Log(String.Format("WarpSpaceManager.RequestFluid2 Is valid provider {0}", FlowManager.HasConduit(ValveBase.GetInputCell())));
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
    internal static class WarpSpaceManager
    {
        private static bool isUpdaterRegistered = false;
        public static Dictionary<int, ValveBaseExt> _gasRequesters = new Dictionary<int, ValveBaseExt>();
        public static Dictionary<int, ValveBaseExt> _gasProviders = new Dictionary<int, ValveBaseExt>();
        public static Dictionary<int, ValveBaseExt> _liquidRequesters = new Dictionary<int, ValveBaseExt>();
        public static Dictionary<int, ValveBaseExt> _liquidProviders = new Dictionary<int, ValveBaseExt>();

        public static void RegisterConduitUpdater()
        {
            if (!isUpdaterRegistered)
            {
                Conduit.GetFlowManager(ConduitType.Gas).AddConduitUpdater(new Action<float>(WarpSpaceManager.GasConduitUpdate), ConduitFlowPriority.Default);
                Conduit.GetFlowManager(ConduitType.Liquid).AddConduitUpdater(new Action<float>(WarpSpaceManager.LiquidConduitUpdate), ConduitFlowPriority.Default);
                isUpdaterRegistered = true;
            }
        }

        public static void UnregisterConduitUpdate()
        {
            if (isUpdaterRegistered)
            {
                Conduit.GetFlowManager(ConduitType.Gas).RemoveConduitUpdater(new Action<float>(WarpSpaceManager.GasConduitUpdate));
                Conduit.GetFlowManager(ConduitType.Liquid).RemoveConduitUpdater(new Action<float>(WarpSpaceManager.LiquidConduitUpdate));
                isUpdaterRegistered = false;
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

        public static void RegisterWarpGate(ValveBase valveBase, int newChannel)
        {
            Logger.LogFormat("==Enter WarpSpaceManager.RegisterWarpGate(valveBase={0}, valveChannel={1})", valveBase.GetInstanceID(), newChannel);
            var sourceDict = GetWarpGateDictionary(valveBase);
            var key = valveBase.GetInstanceID();
            ValveBaseExt baseExt;
            if (sourceDict.TryGetValue(key, out baseExt))
            { }
            else
            {
                baseExt = new ValveBaseExt(valveBase);
                sourceDict.Add(key, baseExt);
            }
            baseExt.Channel = newChannel;
            Logger.Log("==Exit WarpSpaceManager.RegisterWarpGate");
        }

        public static void OnWarpGateChannelChange(ValveBase valveBase)
        {
            Logger.LogFormat("==Enter WarpSpaceManager.OnValveChannelChange(valveBase={0} conduitType={1} inputCell={2} outputCell={3})",
                valveBase.GetInstanceID(),
                valveBase.conduitType,
                valveBase.GetInputCell(),
                valveBase.GetOutputCell());

            int newChannel = Mathf.RoundToInt(valveBase.CurrentFlow * 1000f);

            RegisterWarpGate(valveBase, newChannel);
            Logger.Log("==Exit WarpSpaceManager.OnValveChannelChange");
        }

        private static void PushFluid(List<ValveBaseExt> requesters, List<ValveBaseExt> providers)
        {

            foreach (var provider in providers)
            {
                if (!provider.isValidProvider())
                    continue;
                var flowManager = provider.FlowManager;
                int fromCell = provider.ValveBase.GetInputCell();
                ConduitFlow.Conduit providerConduit = flowManager.GetConduit(fromCell);
                ConduitFlow.ConduitContents providerContents = providerConduit.GetContents(flowManager);
                if (SimHashes.Vacuum.Equals(providerContents.element))
                {
                    Logger.LogFormat("PushFluid: Vacuum found in Provider {0}, channel {1}", provider.ID, provider.Channel);
                    continue;
                }

                var matchedRequesters = requesters.Where(x => x.Channel == provider.Channel).ToList();
                if (matchedRequesters.Count == 0)
                {
                    Logger.LogFormat("PushFluid: No matched requestors for Provider {0}, channel {1}", provider.ID, provider.Channel);
                    continue;
                }
                var splitMass = providerContents.mass / matchedRequesters.Count();

                //Logger.Log(String.Format("WarpSpaceManager.RequestFluid splitMass: {0}", splitMass));
                //Logger.Log(String.Format("WarpSpaceManager.RequestFluid matchedRequesters: {0}", matchedRequesters.Count));
                foreach (var requester in matchedRequesters)
                {
                    if (!requester.isValidRequestor())
                        continue;
                    int toCell = requester.ValveBase.GetOutputCell();
                    ConduitFlow.Conduit requestorConduit = flowManager.GetConduit(toCell);
                    ConduitFlow.ConduitContents requestorContents = requestorConduit.GetContents(flowManager);

                    if (requestorContents.mass < 1f && requestorContents.element != providerContents.element && requestorContents.element != SimHashes.Vacuum)
                    {
                        //Logger.LogFormat("Removing contents is: {0} kg. of {1}", requestorContents.mass, requestorContents.element);
                        flowManager.RemoveElement(requestorConduit, requestorContents.mass);
                    }

                    float addedMass = flowManager.AddElement(toCell, providerContents.element, splitMass, providerContents.temperature, providerContents.diseaseIdx, providerContents.diseaseCount);
                    Game.Instance.accumulators.Accumulate(provider.ValveBase.AccumulatorHandle, addedMass);
//                    Logger.LogFormat($@"Requestor Info 
//requester mass: {requestorContents.mass} requester element: {requestorContents.element}
//provider mass: {providerContents.mass} provider element: {providerContents.element}
//provider split: {splitMass} added mass: {addedMass}
//");
                    if (addedMass > 0f)
                    {
                        ConduitFlow.ConduitContents removed = flowManager.RemoveElement(providerConduit, addedMass);
                        Game.Instance.accumulators.Accumulate(requester.ValveBase.AccumulatorHandle, addedMass);
                       // Logger.LogFormat("Moved {0} kg. from {1} to {2}. ", addedMass, fromCell, toCell);
                    }
                    else
                    {
                        Logger.Log(String.Format("WarpSpaceManager.RequestFluid No mass moved"));
                    }
                }

            }

        }

        private static void UpdateConduitsOfWarpGates(float dt, ConduitType warpGateType)
        {
            try
            {
                Logger.LogFormat("UpdateConduitsOfWarpGates: Liquid Providers: {0}, Gas Providers {1}", _liquidProviders.Count, _gasProviders.Count);
                Logger.LogFormat("UpdateConduitsOfWarpGates: Liquid Requestors: {0}, Gas Requestors {1}", _liquidRequesters.Count, _gasRequesters.Count);
                if (warpGateType == ConduitType.Liquid)
                {
                    var requesters = _liquidRequesters.Values.Where(x => x.Channel != 10000).ToList();
                    var providers = _liquidProviders.Values.Where(x => x.Channel != 10000).ToList();
                    if (requesters.Count == 0 || providers.Count == 0)
                        return;
                    PushFluid(requesters, providers);
                }
                else
                {
                    var requesters = _gasRequesters.Values.Where(x => x.Channel != 10000).ToList();
                    var providers = _gasProviders.Values.Where(x => x.Channel != 10000).ToList();
                    if (requesters.Count == 0 || providers.Count == 0)
                        return;
                    PushFluid(requesters, providers);
                }
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