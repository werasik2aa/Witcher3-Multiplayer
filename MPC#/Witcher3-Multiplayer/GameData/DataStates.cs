using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Witcher3_Multiplayer.GameData
{
    public class DataStates
    {
        public static List<string> States = new List<string>()
        {
            "Idle",
            "Swim",
            "Climb",
            "CombatExploration",
            "TraverseExploration",
            "Jump",
            "Slide",
            "Interaction",
            "Land",
            "Roll",
            "IdleJump"
        };
        public enum WeatherState
        {
            STORM,
            RAINI
        }
        public enum PlayerStates
        {
            JumpClimb,
            Exploration,
            TraverseExploration,
            Swimming,
            HorseRiding,
            Sailing,
            SailingPassive,

        }
        public enum Acts
        {
            EBG_Combat_Shield,
            EBG_Combat_1Handed_Sword,
            EBG_Combat_1Handed_Axe,
            EBG_Combat_1Handed_Blunt,
            EBG_Combat_1Handed_Any,
            EBG_Combat_2Handed_Sword,
            EBG_Combat_2Handed_Any,
            EBG_Combat_2Handed_Hammer,
            EBG_Combat_2Handed_Axe,
            EBG_Combat_2Handed_Halberd,
            EBG_Combat_2Handed_Spear,
            EBG_Combat_2Handed_Staff,
            EBG_Combat_Fists,
            EBG_Combat_Bow,
            EBG_Combat_Crossbow,
            EBG_Combat_Witcher,
            EBG_Combat_Sorceress,
            EBG_Combat_WildHunt_Imlerith,
            EBG_Combat_WildHunt_Imlerith_Second_Stage,
            EBG_Combat_WildHunt_Caranthir,
            EBG_Combat_WildHunt_Caranthir_Second_Stage,
            EBG_Combat_WildHunt_Eredin,
            EBG_Combat_Olgierd,
            EBG_Combat_Caretaker,
            EBG_Combat_Dettlaff_Vampire,
            EBG_Combat_Gregoire,
            EBG_Combat_Dettlaff_Minion,
            EBG_None
        }
        public static string ActionStr(Acts graphEnum)
        {
            switch (graphEnum)
            {
                case Acts.EBG_Combat_Shield: return "Shield";
                case Acts.EBG_Combat_1Handed_Sword: return "sword_1handed";
                case Acts.EBG_Combat_1Handed_Axe: return "sword_1handed";
                case Acts.EBG_Combat_1Handed_Blunt: return "sword_1handed";
                case Acts.EBG_Combat_1Handed_Any: return "sword_1handed";
                case Acts.EBG_Combat_2Handed_Sword: return "sword_2handed";
                case Acts.EBG_Combat_2Handed_Any: return "TwoHanded";
                case Acts.EBG_Combat_2Handed_Hammer: return "TwoHanded";
                case Acts.EBG_Combat_2Handed_Axe: return "TwoHanded";
                case Acts.EBG_Combat_2Handed_Halberd: return "TwoHanded";
                case Acts.EBG_Combat_2Handed_Spear: return "TwoHanded";
                case Acts.EBG_Combat_2Handed_Staff: return "TwoHanded";
                case Acts.EBG_Combat_Fists: return "FistFight";
                case Acts.EBG_Combat_Bow: return "Bow";
                case Acts.EBG_Combat_Crossbow: return "Bow";
                case Acts.EBG_Combat_Witcher: return "Witcher";
                case Acts.EBG_Combat_Sorceress: return "Sorceress";
                case Acts.EBG_Combat_WildHunt_Imlerith: return "Imlerith";
                case Acts.EBG_Combat_WildHunt_Imlerith_Second_Stage: return "ImlerithSecondStage";
                case Acts.EBG_Combat_WildHunt_Caranthir: return "Caranthir";
                case Acts.EBG_Combat_WildHunt_Caranthir_Second_Stage: return "CaranthirSecondStage";
                case Acts.EBG_Combat_WildHunt_Eredin: return "Eredin";
                case Acts.EBG_Combat_Olgierd: return "Olgierd";
                case Acts.EBG_Combat_Caretaker: return "Exploration";
                case Acts.EBG_Combat_Dettlaff_Vampire: return "DettlaffVampire";
                case Acts.EBG_Combat_Gregoire: return "Exploration";
                case Acts.EBG_Combat_Dettlaff_Minion: return "DettlaffMinion";
                case Acts.EBG_None: return "None";
                default: return "";
            }
        }
        enum EAreaName
        {
            AN_Undefined,
            AN_NMLandNovigrad,
            AN_Skellige_ArdSkellig,
            AN_Kaer_Morhen,
            AN_Prologue_Village,
            AN_Wyzima,
            AN_Island_of_Myst,
            AN_Spiral,
            AN_Prologue_Village_Winter,
            AN_Velen,
            AN_CombatTestLevel
        }
        enum AttackType
        {
            EBAT_LightAttack,
            EBAT_HeavyAttack,
            EBAT_SpecialAttack_Light,
            EBAT_SpecialAttack_Heavy,
            EBAT_Ciri_SpecialAttack,
            EBAT_Ciri_SpecialAttack_Heavy
        }
    }
}
