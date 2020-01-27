using SmartHunter.Core;
using SmartHunter.Core.Data;
using SmartHunter.Game.Config;
using SmartHunter.Game.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SmartHunter.Game.Data
{
    public enum MonsterCrown
    {
        None,
        Mini,
        Silver,
        Gold
    }

    public class MonsterHealth : Progress
    {
        public string Id { get; }

        public MonsterHealth(string id, float max, float current, bool shouldCapCurrent = false) : base(max, current, shouldCapCurrent)
        {
            Id = id;
        }

        static readonly IDictionary<string, float> CapturableThreshold = new Dictionary<string, float>() {
            // 10%
            { "em001_02", 0.1F }, // Gold Rathian LOC_MONSTER_GOLD_RATHIAN
            { "em002_02", 0.1F }, // Silver Rathalos LOC_MONSTER_SILVER_RATHALOS

            // 15%
            { "em002_01", 0.15F }, // Azure Rathalos LOC_MONSTER_AZURE_RATHALOS
            { "em007_01", 0.15F }, // Black Diablos LOC_MONSTER_BLACK_DIABLOS
            { "em100_01", 0.15F }, // Anjanath Flugur LOC_MONSTER_ANJANATH_FULGUR
            { "em113_01", 0.15F }, // Ebony Odogaron LOC_MONSTER_EBONY_ODOGARON

            // 20%
            { "em111_00", 0.2F }, // Legiana LOC_MONSTER_LEGIANA
            { "em113_00", 0.2F }, // Odogaron LOC_MONSTER_ODOGARON
            { "em002_00", 0.2F }, // Rathalos LOC_MONSTER_RATHALOS
            { "em007_00", 0.2F }, // Diablos LOC_MONSTER_DIABLOS
            { "em036_00", 0.2F }, // Lavasioth LOC_MONSTER_LAVASIOTH
            { "em043_00", 0.2F }, // Deviljho LOC_MONSTER_DEVILJHO
            { "em045_00", 0.2F }, // Uragaan LOC_MONSTER_URAGAAN
            { "em102_01", 0.2F }, // Pukei-Pukei Coral LOC_MONSTER_PUKEI_PUKEI_CORAL
            { "em042_00", 0.2F }, // Barioth LOC_MONSTER_BARIOTH

            // 25%
            { "em100_00", 0.25F }, // Anjanath LOC_MONSTER_ANJANATH
            { "em102_00", 0.25F }, // Pukei-Pukei LOC_MONSTER_PUKEI_PUKEI
            { "em108_00", 0.25F }, // Jyuratodus LOC_MONSTER_JYURATODUS
            { "em109_00", 0.25F }, // Tobi Kadachi LOC_MONSTER_TOBI_KADACHI
            { "em110_00", 0.25F }, // Paolumu LOC_MONSTER_PAOLUMU
            { "em001_00", 0.25F }, // Rathian LOC_MONSTER_RATHIAN
            { "em001_01", 0.25F }, // Pink Rathian LOC_MONSTER_PINK_RATHIAN
            { "em044_00", 0.25F }, // Barroth LOC_MONSTER_BARROTH
            { "em122_00", 0.25F }, // Beotodus LOC_MONSTER_BEOTODUS
            { "em109_01", 0.25F }, // Viper Tobi Kadachi LOC_MONSTER_VIPER_TOBI_KADACHI
            { "em110_01", 0.25F }, // Paolumu Nightshade LOC_MONSTER_PAOLUMU_NIGHTSHADE

            // 30%
            { "em101_00", 0.3F }, // Great Jagras LOC_MONSTER_GREAT_JAGRAS
            { "em112_00", 0.3F }, // Great Girros LOC_MONSTER_GREAT_GIRROS
            { "em116_00", 0.3F }, // Dodogama LOC_MONSTER_DODOGAMA
            { "em118_00", 0.3F }, // Bazelgeuse LOC_MONSTER_BAZELGEUSE
        };

        public override float Current
        {
            set
            {
                base.Current = value;
                NotifyPropertyChanged(nameof(IsCapturable));
            }
        }

        public bool IsCapturable
        {
            get
            {
                if (CapturableThreshold.ContainsKey(Id))
                {
                    float threshold = CapturableThreshold[Id];
                    return Fraction <= threshold;
                }

                // Unknown
                return false;
            }
        }

    }

    public class Monster : TimedVisibility
    {
        public ulong Address { get; private set; }

        string m_Id;
        public string Id
        {
            get { return m_Id; }
            set
            {
                if (SetProperty(ref m_Id, value))
                {
                    NotifyPropertyChanged(nameof(IsVisible));
                    UpdateLocalization();
                }
            }
        }

        public string Name
        {
            get
            {
                return LocalizationHelper.GetMonsterName(Id);
            }
        }

        float m_SizeScale;
        public float SizeScale
        {
            get { return m_SizeScale; }
            set
            {
                if (SetProperty(ref m_SizeScale, value))
                {
                    NotifyPropertyChanged(nameof(ModifiedSizeScale));
                    NotifyPropertyChanged(nameof(Size));
                    NotifyPropertyChanged(nameof(Crown));
                }
            }
        }

        public float ModifiedSizeScale
        {
            get
            {
                float modifiedSizeScale = SizeScale;

                MonsterConfig config = null;
                if (ConfigHelper.MonsterData.Values.Monsters.TryGetValue(Id, out config))
                {
                    modifiedSizeScale /= config.ScaleModifier;
                }

                return modifiedSizeScale;
            }
        }

        public float Size
        {
            get
            {
                float size = 0;

                MonsterConfig config = null;
                if (ConfigHelper.MonsterData.Values.Monsters.TryGetValue(Id, out config))
                {
                    size = config.BaseSize * ModifiedSizeScale;
                }

                return size;
            }
        }

        public MonsterCrown Crown
        {
            get
            {
                MonsterCrown crown = MonsterCrown.None;

                MonsterConfig config = null;
                if (ConfigHelper.MonsterData.Values.Monsters.TryGetValue(Id, out config) && config.Crowns != null)
                {
                    float modifiedSizeScale = ModifiedSizeScale;

                    if (modifiedSizeScale <= config.Crowns.Mini)
                    {
                        crown = MonsterCrown.Mini;
                    }
                    else if (modifiedSizeScale >= config.Crowns.Gold)
                    {
                        crown = MonsterCrown.Gold;
                    }
                    else if (modifiedSizeScale >= config.Crowns.Silver)
                    {
                        crown = MonsterCrown.Silver;
                    }
                }

                return crown;
            }
        }

        public MonsterHealth Health { get; private set; }
        public ObservableCollection<MonsterPart> Parts { get; private set; }
        public ObservableCollection<MonsterStatusEffect> StatusEffects { get; private set; }

        public bool IsVisible
        {
            get
            {
                return IsIncluded(Id) && IsTimeVisible(ConfigHelper.Main.Values.Overlay.MonsterWidget.ShowUnchangedMonsters, ConfigHelper.Main.Values.Overlay.MonsterWidget.HideMonstersAfterSeconds);
            }
        }

        public Monster(ulong address, string id, float maxHealth, float currentHealth, float sizeScale)
        {
            Address = address;
            m_Id = id;
            Health = new MonsterHealth(id, maxHealth, currentHealth);
            m_SizeScale = sizeScale;

            Parts = new ObservableCollection<MonsterPart>();
            StatusEffects = new ObservableCollection<MonsterStatusEffect>();
        }

        public MonsterPart UpdateAndGetPart(ulong address, bool isRemovable, float maxHealth, float currentHealth, int timesBrokenCount)
        {
            MonsterPart part = Parts.SingleOrDefault(collectionPart => collectionPart.Address == address);
            if (part != null)
            {
                part.IsRemovable = isRemovable;
                part.Health.Max = maxHealth;
                part.Health.Current = currentHealth;
                part.TimesBrokenCount = timesBrokenCount;
            }
            else
            {
                part = new MonsterPart(this, address, isRemovable, maxHealth, currentHealth, timesBrokenCount);
                part.Changed += PartOrStatusEffect_Changed;

                Parts.Add(part);
            }

            part.NotifyPropertyChanged(nameof(MonsterPart.IsVisible));

            return part;
        }

        public MonsterStatusEffect UpdateAndGetStatusEffect(int index, float maxBuildup, float currentBuildup, float maxDuration, float currentDuration, int timesActivatedCount)
        {
            MonsterStatusEffect statusEffect = StatusEffects.SingleOrDefault(collectionStatusEffect => collectionStatusEffect.Index == index);
            if (statusEffect != null)
            {
                statusEffect.Duration.Max = maxDuration;
                statusEffect.Duration.Current = currentDuration;
                statusEffect.Buildup.Max = maxBuildup;
                statusEffect.Buildup.Current = currentBuildup;
                statusEffect.TimesActivatedCount = timesActivatedCount;
            }
            else
            {
                statusEffect = new MonsterStatusEffect(this, index, maxBuildup, currentBuildup, maxDuration, currentDuration, timesActivatedCount);
                statusEffect.Changed += PartOrStatusEffect_Changed;

                StatusEffects.Add(statusEffect);
            }

            statusEffect.NotifyPropertyChanged(nameof(MonsterStatusEffect.IsVisible));

            return statusEffect;
        }

        public void UpdateLocalization()
        {
            NotifyPropertyChanged(nameof(Name));

            foreach (var part in Parts)
            {
                part.NotifyPropertyChanged(nameof(MonsterPart.Name));
            }
            foreach (var statusEffect in StatusEffects)
            {
                statusEffect.NotifyPropertyChanged(nameof(MonsterStatusEffect.Name));
            }
        }

        public static bool IsIncluded(string monsterId)
        {
            return ConfigHelper.Main.Values.Overlay.MonsterWidget.MatchIncludeMonsterIdRegex(monsterId);
        }

        private void PartOrStatusEffect_Changed(object sender, GenericEventArgs<DateTimeOffset> e)
        {
            UpdateLastChangedTime();
        }
    }
}
