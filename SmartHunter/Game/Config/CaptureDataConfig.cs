using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHunter.Game.Config
{
    public class CaptureDataConfig
    {
        public static CaptureDataConfig Default { get; } = new CaptureDataConfig();

        public IDictionary<string, float> Thresholds { get; } = new Dictionary<string, float>() {
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
    }
}
