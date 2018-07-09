// ProcGen.WorldGenTags

namespace ProcGen
{
    public class WorldGenTags
    {
        public static readonly Tag ConnectToSiblings = TagManager.Create("ConnectToSiblings");

        public static readonly Tag ConnectTypeMinSpan = TagManager.Create("ConnectTypeMinSpan");

        public static readonly Tag ConnectTypeSpan = TagManager.Create("ConnectTypeSpan");

        public static readonly Tag ConnectTypeNone = TagManager.Create("ConnectTypeNone");

        public static readonly Tag ConnectTypeFull = TagManager.Create("ConnectTypeFull");

        public static readonly Tag ConnectTypeRandom = TagManager.Create("ConnectTypeRandom");

        public static readonly Tag Cell = TagManager.Create("Cell");

        public static readonly Tag Edge = TagManager.Create("Edge");

        public static readonly Tag Corner = TagManager.Create("Corner");

        public static readonly Tag EdgeUnpassable = TagManager.Create("EdgeUnpassable");

        public static readonly Tag EdgeClosed = TagManager.Create("EdgeClosed");

        public static readonly Tag EdgeOpen = TagManager.Create("EdgeOpen");

        public static readonly Tag IgnoreCaveOverride = TagManager.Create("IgnoreCaveOverride");

        public static readonly Tag ErodePointToCentroid = TagManager.Create("ErodePointToCentroid");

        public static readonly Tag ErodePointToCentroidInv = TagManager.Create("ErodePointToCentroidInv");

        public static readonly Tag ErodePointToEdge = TagManager.Create("ErodePointToEdge");

        public static readonly Tag ErodePointToEdgeInv = TagManager.Create("ErodePointToEdgeInv");

        public static readonly Tag DistFunctionPointCentroid = TagManager.Create("DistFunctionPointCentroid");

        public static readonly Tag DistFunctionPointEdge = TagManager.Create("DistFunctionPointEdge");

        public static readonly Tag SplitOnParentDensity = TagManager.Create("SplitOnParentDensity");

        public static readonly Tag SplitTwice = TagManager.Create("SplitTwice");

        public static readonly Tag UltraHighDensitySplit = TagManager.Create("UltraHighDensitySplit");

        public static readonly Tag VeryHighDensitySplit = TagManager.Create("VeryHighDensitySplit");

        public static readonly Tag HighDensitySplit = TagManager.Create("HighDensitySplit");

        public static readonly Tag MediumDensitySplit = TagManager.Create("MediumDensitySplit");

        public static readonly Tag UnassignedNode = TagManager.Create("UnassignedNode");

        public static readonly Tag Feature = TagManager.Create("Feature");

        public static readonly Tag CenteralFeature = TagManager.Create("CenteralFeature");

        public static readonly Tag Overworld = TagManager.Create("Overworld");

        public static readonly Tag StartNear = TagManager.Create("StartNear");

        public static readonly Tag StartMedium = TagManager.Create("StartMedium");

        public static readonly Tag StartFar = TagManager.Create("StartFar");

        public static readonly Tag NearEdge = TagManager.Create("NearEdge");

        public static readonly Tag NearSurface = TagManager.Create("NearSurface");

        public static readonly Tag NearDepths = TagManager.Create("NearDepths");

        public static readonly Tag AtSurface = TagManager.Create("AtSurface");

        public static readonly Tag AtDepths = TagManager.Create("AtDepths");

        public static readonly Tag AtEdge = TagManager.Create("AtEdge");

        public static readonly Tag EdgeOfVoid = TagManager.Create("EdgeOfVoid");

        public static readonly Tag Dry = TagManager.Create("Dry");

        public static readonly Tag Wet = TagManager.Create("Wet");

        public static readonly Tag River = TagManager.Create("River");

        public static readonly Tag StartWorld = TagManager.Create("StartWorld");

        public static readonly Tag StartLocation = TagManager.Create("StartLocation");

        public static readonly Tag NearStartLocation = TagManager.Create("NearStartLocation");

        public static readonly Tag POI = TagManager.Create("POI");

        public static readonly Tag RoomBorderNone = TagManager.Create("RoomBorderNone");

        public static readonly Tag RoomBorderMixed = TagManager.Create("BorderMixed");

        public static readonly Tag RoomBorderRandom = TagManager.Create("BorderRandom");

        public static readonly Tag AllowExceedNodeBorders = TagManager.Create("AllowExceedNodeBorders");

        public static readonly Tag CaveVoidSliver = TagManager.Create("CaveVoidSliver");

        public static readonly Tag Geode = TagManager.Create("Geode");

        public static readonly Tag TheVoid = TagManager.Create("TheVoid");

        public static readonly Tag SprinkleOfMetal = TagManager.Create("SprinkleOfMetal");

        public static readonly Tag SprinkleOfOxyRock = TagManager.Create("SprinkleOfOxyRock");

        public static readonly Tag Infected = TagManager.Create("Infected");

        public static readonly Tag InfectedDweebcephaly = TagManager.Create("Infected:Dweebcephaly");

        public static readonly Tag InfectedLazibonitis = TagManager.Create("Infected:Lazibonitis");

        public static readonly Tag InfectedDiarrhea = TagManager.Create("Infected:Diarrhea");

        public static readonly Tag InfectedFoodPoisoning = TagManager.Create("Infected:FoodPoisoning");

        public static readonly Tag InfectedPutridOdour = TagManager.Create("Infected:PutridOdour");

        public static readonly Tag InfectedSpores = TagManager.Create("Infected:Spores");

        public static readonly Tag InfectedColdBrain = TagManager.Create("Infected:ColdBrain");

        public static readonly Tag InfectedHeatRash = TagManager.Create("Infected:HeatRash");

        public static readonly Tag InfectedSlimeLung = TagManager.Create("Infected:SlimeLung");

        public static readonly Tag InfectedRadioactivity = TagManager.Create("Infected:Radioactive");

        public static readonly Tag DEBUG_Split = TagManager.Create("DEBUG_Split");

        public static readonly Tag DEBUG_SplitForChildCount = TagManager.Create("DEBUG_SplitForChildCount");

        public static readonly Tag DEBUG_SplitTopSite = TagManager.Create("DEBUG_SplitTopSite");

        public static readonly Tag DEBUG_SplitBottomSite = TagManager.Create("DEBUG_SplitBottomSite");

        public static readonly Tag DEBUG_SplitLargeStartingSites = TagManager.Create("DEBUG_SplitLargeStartingSites");

        public static readonly Tag DEBUG_NoSplitForChildCount = TagManager.Create("DEBUG_NoSplitForChildCount");

        public static readonly TagSet DebugTags = new TagSet(new Tag[6]
        {
            WorldGenTags.DEBUG_Split,
            WorldGenTags.DEBUG_SplitForChildCount,
            WorldGenTags.DEBUG_SplitTopSite,
            WorldGenTags.DEBUG_SplitBottomSite,
            WorldGenTags.DEBUG_SplitLargeStartingSites,
            WorldGenTags.DEBUG_NoSplitForChildCount
        });

        public static readonly TagSet MapTags = new TagSet(new Tag[6]
        {
            WorldGenTags.Cell,
            WorldGenTags.Edge,
            WorldGenTags.Corner,
            WorldGenTags.EdgeUnpassable,
            WorldGenTags.EdgeClosed,
            WorldGenTags.EdgeOpen
        });

        public static readonly TagSet CommandTags = new TagSet(new Tag[11]
        {
            WorldGenTags.IgnoreCaveOverride,
            WorldGenTags.ErodePointToCentroid,
            WorldGenTags.ErodePointToCentroidInv,
            WorldGenTags.DistFunctionPointCentroid,
            WorldGenTags.DistFunctionPointEdge,
            WorldGenTags.SplitOnParentDensity,
            WorldGenTags.SplitTwice,
            WorldGenTags.UltraHighDensitySplit,
            WorldGenTags.VeryHighDensitySplit,
            WorldGenTags.HighDensitySplit,
            WorldGenTags.MediumDensitySplit
        });

        public static readonly TagSet WorldTags = new TagSet(new Tag[11]
        {
            WorldGenTags.UnassignedNode,
            WorldGenTags.Feature,
            WorldGenTags.CenteralFeature,
            WorldGenTags.Overworld,
            WorldGenTags.NearSurface,
            WorldGenTags.NearDepths,
            WorldGenTags.AtSurface,
            WorldGenTags.AtDepths,
            WorldGenTags.AtEdge,
            WorldGenTags.StartNear,
            WorldGenTags.StartMedium
        });

        public static readonly TagSet DistanceTags = new TagSet(new Tag[7]
        {
            WorldGenTags.NearEdge,
            WorldGenTags.NearSurface,
            WorldGenTags.NearDepths,
            WorldGenTags.AtSurface,
            WorldGenTags.AtDepths,
            WorldGenTags.AtEdge,
            WorldGenTags.StartWorld
        });
    }
}
