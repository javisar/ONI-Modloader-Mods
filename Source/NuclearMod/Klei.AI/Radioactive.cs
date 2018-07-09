// Klei.AI.Radioactive
using Klei.AI;
using Klei.AI.DiseaseGrowthRules;
using STRINGS;
using System.Collections.Generic;
using UnityEngine;

namespace Klei.AI
{
    public class Radioactive : Disease
    {
        public class RadioactiveComponent : DiseaseComponent
        {
            public class StatesInstance : GameStateMachine<States, StatesInstance, DiseaseInstance, object>.GameInstance
            {
                public float lastCoughtTime;

                public StatesInstance(DiseaseInstance master)
                    : base(master)
                {
                }

                public Reactable GetReactable()
                {
                    return new SelfEmoteReactable(base.master.gameObject, Db.Get().ChoreTypes.Cough, "anim_sneeze_kanim").AddStep(new EmoteReactable.EmoteStep
                    {
                        anim = "sneeze",
                        finishcb = this.ProduceSlime
                    }).AddStep(new EmoteReactable.EmoteStep
                    {
                        anim = "sneeze_pst"
                    }).AddStep(new EmoteReactable.EmoteStep
                    {
                        startcb = this.FinishedCoughing
                    });
                }

                private void ProduceSlime(GameObject cougher)
                {
                    AmountInstance amountInstance = Db.Get().Amounts.Temperature.Lookup(cougher);
                    int gameCell = Grid.PosToCell(cougher);
                    SimMessages.AddRemoveSubstance(gameCell, SimHashes.ContaminatedOxygen, CellEventLogger.Instance.Cough, 0.1f, amountInstance.value, Db.Get().Diseases.GetIndex("Radioactive"), 1000, true, -1);
                    PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Resource, string.Format(DUPLICANTS.DISEASES.ADDED_POPFX, base.master.modifier.Name, 1000), cougher.transform, 1.5f, false);
                }

                private void FinishedCoughing(GameObject cougher)
                {
                    base.sm.coughFinished.Trigger(this);
                }
            }

            public class States : GameStateMachine<States, StatesInstance, DiseaseInstance>
            {
                public class BreathingStates : State
                {
                    public State normal;

                    public State cough;
                }

                public Signal coughFinished;

                public BreathingStates breathing;

                public State notbreathing;

                public override void InitializeStates(out BaseState default_state)
                {
                    default_state = this.breathing;
                    this.breathing.DefaultState(this.breathing.normal).TagTransition(GameTags.NoOxygen, this.notbreathing, false).Enter("SetCoughTime", delegate (StatesInstance smi)
                    {
                        smi.lastCoughtTime = Time.time;
                    })
                        .Update("Cough", delegate (StatesInstance smi, float dt)
                        {
                            if (!smi.master.IsDoctored && Time.time - smi.lastCoughtTime > 20f)
                            {
                                smi.GoTo(this.breathing.cough);
                            }
                        }, UpdateRate.SIM_4000ms, false);
                    this.breathing.cough.ToggleReactable((StatesInstance smi) => smi.GetReactable()).OnSignal(this.coughFinished, this.breathing.normal);
                    this.notbreathing.TagTransition(new Tag[1]
                    {
                        GameTags.NoOxygen
                    }, this.breathing, true);
                }
            }

            public override object OnInfect(GameObject go, DiseaseInstance diseaseInstance)
            {
                StatesInstance statesInstance = new StatesInstance(diseaseInstance);
                statesInstance.StartSM();
                return statesInstance;
            }

            public override void OnCure(GameObject go, object instance_data)
            {
                StatesInstance statesInstance = (StatesInstance)instance_data;
                statesInstance.StopSM("Cured");
            }

            public override List<Descriptor> GetSymptoms()
            {
                List<Descriptor> list = new List<Descriptor>();
                //list.Add(new Descriptor(DUPLICANTS.DISEASES.Radioactive.COUGH_SYMPTOM, DUPLICANTS.DISEASES.Radioactive.COUGH_SYMPTOM_TOOLTIP, Descriptor.DescriptorType.SymptomAidable, false));
                list.Add(new Descriptor(Strings.Get("DUPLICANTS.DISEASES.Radioactive.COUGH_SYMPTOM"), Strings.Get("DUPLICANTS.DISEASES.Radioactive.COUGH_SYMPTOM_TOOLTIP"), Descriptor.DescriptorType.SymptomAidable, false));
                return list;
            }
        }

        private const float COUGH_FREQUENCY = 20f;

        private const float COUGH_MASS = 0.1f;

        private const int DISEASE_AMOUNT = 1000;

        private const float DEATH_TIMER = 4800f;

        public const string ID = "Radioactive";

        public Radioactive()
            : base("Radioactive", DiseaseType.Pathogen, Severity.Critical, 0.00025f, new List<InfectionVector>
            {
                InfectionVector.Inhalation
            }, 2400f, 1, new RangeInfo(283.15f, 293.15f, 363.15f, 373.15f), new RangeInfo(10f, 1200f, 1200f, 10f), new RangeInfo(0f, 0f, 1000f, 1000f), RangeInfo.Idempotent())
        {
            base.doctorRequired = true;
            base.fatalityDuration = 4800f;
            base.AddDiseaseComponent(new CommonSickEffectDisease());
            base.AddDiseaseComponent(new AttributeModifierDisease(new AttributeModifier[2]
            {
                new AttributeModifier("BreathDelta", -1.13636363f, Strings.Get("DUPLICANTS.DISEASES.Radioactive.NAME"), false, false, true),
                new AttributeModifier("Athletics", -3f, Strings.Get("DUPLICANTS.DISEASES.Radioactive.NAME"), false, false, true)
            }));
            base.AddDiseaseComponent(new RadioactiveComponent());
        }

        protected override void PopulateElemGrowthInfo()
        {
            base.InitializeElemGrowthArray(ref base.elemGrowthInfo, Disease.DEFAULT_GROWTH_INFO);
            base.AddGrowthRule(new GrowthRule
            {
                underPopulationDeathRate = 2.66666675f,
                minCountPerKG = 0.4f,
                populationHalfLife = 12000f,
                maxCountPerKG = 500f,
                overPopulationHalfLife = 1200f,
                minDiffusionCount = 1000,
                diffusionScale = 0.001f,
                minDiffusionInfestationTickCount = 1
            });
            base.AddGrowthRule(new StateGrowthRule(Element.State.Solid)
            {
                minCountPerKG = 0.4f,
                populationHalfLife = 3000f,
                overPopulationHalfLife = 1200f,
                diffusionScale = 1E-06f,
                minDiffusionCount = 1000000
            });
            base.AddGrowthRule(new ElementGrowthRule(SimHashes.SlimeMold)
            {
                underPopulationDeathRate = 0f,
                populationHalfLife = -3000f,
                overPopulationHalfLife = 3000f,
                maxCountPerKG = 4500f,
                diffusionScale = 0.05f
            });
            base.AddGrowthRule(new ElementGrowthRule(SimHashes.BleachStone)
            {
                populationHalfLife = 10f,
                overPopulationHalfLife = 10f,
                minDiffusionCount = 100000,
                diffusionScale = 0.001f
            });
            base.AddGrowthRule(new StateGrowthRule(Element.State.Gas)
            {
                minCountPerKG = 250f,
                populationHalfLife = 12000f,
                overPopulationHalfLife = 1200f,
                maxCountPerKG = 10000f,
                minDiffusionCount = 5100,
                diffusionScale = 0.005f
            });
            base.AddGrowthRule(new ElementGrowthRule(SimHashes.ContaminatedOxygen)
            {
                underPopulationDeathRate = 0f,
                populationHalfLife = -300f,
                overPopulationHalfLife = 1200f
            });
            base.AddGrowthRule(new ElementGrowthRule(SimHashes.Oxygen)
            {
                populationHalfLife = 1200f,
                overPopulationHalfLife = 10f
            });
            base.AddGrowthRule(new ElementGrowthRule(SimHashes.ChlorineGas)
            {
                populationHalfLife = 10f,
                overPopulationHalfLife = 10f,
                minDiffusionCount = 100000,
                diffusionScale = 0.001f
            });
            base.AddGrowthRule(new StateGrowthRule(Element.State.Liquid)
            {
                minCountPerKG = 0.4f,
                populationHalfLife = 1200f,
                overPopulationHalfLife = 300f,
                maxCountPerKG = 100f,
                diffusionScale = 0.01f
            });
            base.InitializeElemExposureArray(ref base.elemExposureInfo, Disease.DEFAULT_EXPOSURE_INFO);
            base.AddExposureRule(new ExposureRule
            {
                populationHalfLife = float.PositiveInfinity
            });
            base.AddExposureRule(new ElementExposureRule(SimHashes.DirtyWater)
            {
                populationHalfLife = -12000f
            });
            base.AddExposureRule(new ElementExposureRule(SimHashes.ContaminatedOxygen)
            {
                populationHalfLife = -12000f
            });
            base.AddExposureRule(new ElementExposureRule(SimHashes.Oxygen)
            {
                populationHalfLife = 3000f
            });
            base.AddExposureRule(new ElementExposureRule(SimHashes.ChlorineGas)
            {
                populationHalfLife = 10f
            });
        }

        public override List<Descriptor> GetDiseaseSourceDescriptors()
        {
            List<Descriptor> list = new List<Descriptor>();
            list.Add(new Descriptor(string.Format(Strings.Get("DUPLICANTS.DISEASES.Radioactive.DISEASE_SOURCE_DESCRIPTOR"), GameUtil.GetFormattedTime(20f), GameUtil.GetFormattedDiseaseAmount(1000), base.Name), string.Format(Strings.Get("DUPLICANTS.DISEASES.Radioactive.DISEASE_SOURCE_DESCRIPTOR_TOOLTIP"), GameUtil.GetFormattedTime(20f), GameUtil.GetFormattedDiseaseAmount(1000), base.Name), Descriptor.DescriptorType.DiseaseSource, false));
            return list;
        }
    }
}