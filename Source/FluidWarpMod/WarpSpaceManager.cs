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
    internal class StatefullList<T>: List<T>
    {
        private int currentItem = -1;

        public T getNext()
        {
            if (++currentItem >= Count)
            {
                currentItem = 0;
            }
            if (currentItem < Count)
            {
                return base[currentItem];
            }
            else
            {
                return default(T);
            }
        }

    }

    internal class ValvesList : StatefullList<ValveBase> { }

    internal class ValveChannels : Dictionary<int, ValvesList> { }

    static class WarpSpaceManager
    {
        private static FieldInfo valveBaseOutputCellFieldInfo = AccessTools.Field(typeof(ValveBase), "outputCell");

        private static FieldInfo valveBaseInputCellFieldInfo = AccessTools.Field(typeof(ValveBase), "inputCell");

        private static ValveChannels gasChannels = new ValveChannels();
 
        private static ValveChannels liquidChannels = new ValveChannels();

        private static ValveChannels getChannelsForConduitType(ConduitType conduitType)
        {
            return (conduitType == LiquidWarpConfig.CONDUIT_TYPE ? liquidChannels : gasChannels);
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
                valves = new ValvesList();
                providers[newChannel] = valves;
            }

            valves.Add(valveBase);
            Logger.Log("==Exit WaprSpaceManager.SetProviderValveChannel");
        }

        public static void OnValveChannelChange(ValveBase valveBase)
        {
            Logger.LogFormat("==Enter WarpSpaceManager.OnValveChannelChange(valveBase={0})", valveBase.GetInstanceID());
            ConduitFlow flowManager = null;
            if (valveBase.conduitType == LiquidWarpConfig.CONDUIT_TYPE)
            {
                flowManager = Conduit.GetFlowManager(ConduitType.Liquid);
            }
            else if (valveBase.conduitType == GasWarpConfig.CONDUIT_TYPE)
            {
                flowManager = Conduit.GetFlowManager(ConduitType.Gas);
            }
            else
            {
                return;
            }

            int newChannel = Mathf.RoundToInt(valveBase.CurrentFlow * 1000f);

            SetProviderValveChannel(valveBase, newChannel);
            Logger.Log("==Exit WarpSpaceManager.OnValveChannelChange");
        }


        public static void RemoveProviderValve(ValveBase valveBase)
        {
            Logger.LogFormat("==Enter WarpSpaceManager.RemoveProviderValve(valveBase={0})", valveBase);
            ValveChannels providers = getChannelsForConduitType(valveBase.conduitType);
            foreach (var channel in providers)
            {
                channel.Value.Remove(valveBase);
            }
            Logger.Log("==Exit WarpSpaceManager.RemoveProviderValve");
        }

        public static void RequestFluidFromChannel(ValveBase requestor, int channelNo)
        {
            Logger.LogFormat("==Entry WarpSpaceManager.RequestFluidFromChannel(requestor={0}, channelNo={1})", requestor.GetInstanceID(), channelNo);
            try
            {
                ConduitFlow flowManager = null;
                if (requestor.conduitType == LiquidWarpConfig.CONDUIT_TYPE)
                {
                    flowManager = Conduit.GetFlowManager(ConduitType.Liquid);
                }
                else if (requestor.conduitType == GasWarpConfig.CONDUIT_TYPE)
                {
                    flowManager = Conduit.GetFlowManager(ConduitType.Gas);
                }
                else
                {
                    Logger.Log("unable to determine correct ConduitType.");
                    return;
                }

                ValveChannels channels = getChannelsForConduitType(requestor.conduitType);
                ValvesList providers;

                if (!channels.TryGetValue(channelNo, out providers) || (providers.Count == 0))
                {
                    Logger.LogFormat("No providers for channel {0} found.", channelNo);
                    return;
                }

                ValveBase provider = providers.getNext();
                ValveBase start = provider;
                if (null == provider)
                {
                    Logger.Log("You should never see this message! provider is null");
                    return;
                }
                int toCell = (int)valveBaseOutputCellFieldInfo.GetValue(requestor);
                ConduitFlow.ConduitContents requestorContents = flowManager.GetContents(toCell);
                // Fill input cell from various providers, in case when provider's conduit is not full
                do
                {
                    Logger.LogFormat("Trying to request from valveBase {0}", provider.GetInstanceID());
                    int fromCell = (int)valveBaseInputCellFieldInfo.GetValue(provider);
                    if (provider != requestor && flowManager.HasConduit(fromCell))
                    {
                        ConduitFlow.ConduitContents providerContents = flowManager.GetContents(fromCell);
                        float addedMass = flowManager.AddElement(toCell, providerContents.element, providerContents.mass, providerContents.temperature, providerContents.diseaseIdx, providerContents.diseaseCount);
                        Game.Instance.accumulators.Accumulate(provider.AccumulatorHandle, addedMass);
                        if (addedMass > 0f)
                        {
                            Logger.LogFormat("Adding Element to cell: requestor={0} provider={1} actually added mass={2}, element type={3}", requestor.GetInstanceID(), provider.GetInstanceID(), addedMass, providerContents.element);
                            flowManager.RemoveElement(fromCell, addedMass);
                            Game.Instance.accumulators.Accumulate(requestor.AccumulatorHandle, addedMass);
                        }
                    }
                    if (flowManager.IsConduitFull(toCell))
                    {
                        break;
                    }
                    provider = providers.getNext();
                } while (provider != start);
            }
            catch(Exception ex)
            {
                Logger.LogFormat("Exception in WarpSpaceManager.RequestFluidFromChannel: {0}\n{1}", ex.Message, ex.StackTrace);  
            }
            Logger.Log("==Exit WarpSpaceManager.RequestFluidFromChannel");

        }

    }
}
