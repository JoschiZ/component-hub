using System.ComponentModel;
using NetEscapades.EnumGenerators;

namespace ComponentHub.Domain.Features.Components;

public sealed class ComponentTag
{
    private ComponentTag(){}
    public ComponentTag(ComponentTagId id)
    {
        Id = id;
    }
    
    public ComponentTagId Id { get; set; }
    
    
    private List<ComponentPage> _pages = [];
    public IEnumerable<ComponentPage> Pages => _pages.AsReadOnly();

    public string Description
    {
        get => Id.ToStringFast();
        
        // This is a hack for EFCore
        private set => _ = value;
    }
}

[EnumExtensions]

public enum ComponentTagId
{
    [Description("World of Warcraft")]
    Warcraft = 1,
    
    [Description("Damage")]
    Damage = 2,
    
    [Description("Tank")]
    Tank = 3,
    
    [Description("Healer")]
    Healer = 4,
    
    [Description("Death Knight")]
    DeathKnight = 5,
    
    [Description("Blood Death Knight")]
    BloodDk = 6,
    
    [Description("Unholy Death Knight")]
    UnholyDk = 7,
    
    [Description("Frost Death Knight")]
    FrostDk = 8,
    
    [Description("Demon Hunter")]
    DemonHunter = 9,
    
    [Description("Vengeance Demon Hunter")]
    VengeanceDemonHunter = 10,
    
    [Description("Devastation Demon Hunter")]
    DevastationDemonHunter = 11,
    
    [Description("Druid")]
    Druid = 12,
    
    [Description("Balance Druid")]
    BalanceDruid = 13,
    
    [Description("Feral Druid")]
    FeralDruid = 14,
    
    [Description("Guardian Druid")]
    GuardianDruid = 15,
    
    [Description("Restoration Druid")]
    RestorationDruid = 16,
    
    [Description("Evoker")]
    Evoker = 17,
    
    [Description("Devastation Evoker")]
    DevastationEvoker = 18,
    
    [Description("Preservation Evoker")]
    PreservationEvoker = 19,
    
    [Description("Augmentation Evoker")]
    AugmentationEvoker = 20,
    
    [Description("Hunter")]
    Hunter = 21,
    
    [Description("Beast Mastery Hunter")]
    BeastMasteryHunter = 22,
    
    [Description("Marksmanship Hunter")]
    MarksmanshipHunter = 23,
    
    [Description("Survival Hunter")]
    SurvivalHunter = 24,
    
    [Description("Mage")]
    Mage = 25,
    
    [Description("Arcane Mage")]
    ArcaneMage = 26,
    
    [Description("Fire Mage")]
    FireMage = 27,
    
    [Description("Frost Mage")]
    FrostMage = 28,
    
    [Description("Monk")]
    Monk = 29,
    
    [Description("Brewmaster Monk")]
    BrewmasterMonk = 30,
    
    [Description("Mistweaver Monk")]
    MistweaverMonk = 31,
    
    [Description("Windwalker Monk")]
    WindwalkerMonk = 32,
    
    [Description("Paladin")]
    Paladin = 33,
    
    [Description("Holy Paladin")]
    HolyPaladin = 34,
    
    [Description("Protection Paladin")]
    ProtectionPaladin = 35,
    
    [Description("Retribution Paladin")]
    RetributionPaladin = 36,
    
    [Description("Priest")]
    Priest = 37,
    
    [Description("Discipline Priest")]
    DisciplinePriest = 38,
    
    [Description("Holy Priest")]
    HolyPriest = 39,
    
    [Description("Shadow Priest")]
    ShadowPriest = 40,
    
    [Description("Rogue")]
    Rogue = 41,
    
    [Description("Assassination Rogue")]
    AssassinationRogue = 42,
    
    [Description("Outlaw Rogue")]
    OutlawRogue = 43,
    
    [Description("Subtlety Rogue")]
    SubtletyRogue = 44,
    
    [Description("Shaman")]
    Shaman = 45,
    
    [Description("Elemental Shaman")]
    ElementalShaman = 46,
    
    [Description("Enhancement Shaman")]
    EnhancementShaman = 47,
    
    [Description("Restoration Shaman")]
    RestorationShaman = 48,
    
    [Description("Warlock")]
    Warlock = 49,
    
    [Description("Affliction Warlock")]
    AfflictionWarlock = 50,
    
    [Description("Demonology Warlock")]
    DemonologyWarlock = 51,
    
    [Description("Destruction Warlock")]
    DestructionWarlock = 52,
    
    [Description("Retail")]
    Retail = 53,
    
    [Description("Amirdrassil")]
    Amirdrassil = 54,
    
    [Description("Gnalroot")]
    Gnalroot = 55,
    
    [Description("Igira the Cruel")]
    Igira = 56,
    
    [Description("Volcoross")]
    Volcoross = 57,
    
    [Description("Larodar, Keeper of the Flame")]
    Larodar = 58,
    
    [Description("Council Of Dreams: Urctos, Aerwynn, Pip")]
    CouncilOfDreams = 59,
    
    [Description("Nymue, Weaver of the Cycle")]
    Nymue = 60,
    
    [Description("Smolderon, the Firelord")]
    Smolderon = 61,
    
    [Description("Tindral, Seer of the Flame")]
    Tindral = 62,
    
    [Description("Fyrakk, the Blazing")]
    Fyrakk = 63,
    
    [Description("Mythic+ DF Season 3")]
    DragonFlightSeason3 = 64,
    
    [Description("Dawn of the Infinite")]
    DawnOfTheInfinite = 65,
    
    [Description("Murozond's Rise")]
    DawnRise = 66,
    
    [Description("Galakrond's Fall")]
    DawnFall = 67,
    [Description("Waycrest Manor")]
    Waycrest = 68,
    [Description("Darkheart Thicket")]
    Darkheart = 69,
    [Description("Atal'Dazar")]
    AtalDazar = 70,
    [Description("Black Rook Hold")]
    BlackRookHold = 71,
    [Description("The Everbloom")]
    Everbloom = 72,
    [Description("Throne of the Tides")]
    ThroneOfTheTides = 73,
    [Description("World of Warcraft Classic")]
    Classic = 74,
    [Description("Final Fantasy")]
    FinalFantasy = 75,
    [Description("Elder Scrolls Online")]
    ElderScrollsOnline = 76,
    [Description("Star Wars the Old Republic")]
    StarWars = 77,
}


