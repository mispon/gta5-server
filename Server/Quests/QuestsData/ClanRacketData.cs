using System.Collections.Generic;
using gta_mp_server.Models.Clan;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Shared.Math;

namespace gta_mp_server.Quests.QuestsData {
    /// <summary>
    /// Данные задания на рэкет фермеров
    /// </summary>
    internal class ClanRacketData {
        /// <summary>
        /// Квестовые фермеры
        /// </summary>
        internal static List<ClanQuestNpc> Npcs = new List<ClanQuestNpc> {
            new ClanQuestNpc {
                Index = 1, Hash = PedHash.Chip, Position = new Vector3(1380.81, 1147.56, 114.33),
                Rotation = new Vector3(0.00, 0.00, 92.98), ShapePosition = new Vector3(1379.98, 1147.50, 113.33)
            },
            new ClanQuestNpc {
                Index = 2, Hash = PedHash.Eastsa03AFY, Position = new Vector3(-86.13, 1882.23, 197.31),
                Rotation = new Vector3(0.00, 0.00, -93.80), ShapePosition = new Vector3(-85.33, 1882.00, 196.30)
            },
            new ClanQuestNpc {
                Index = 3, Hash = PedHash.Business01AMM, Position = new Vector3(-1909.45, 2071.53, 140.39),
                Rotation = new Vector3(0.00, 0.00, 134.33), ShapePosition = new Vector3(-1909.86, 2071.09, 139.39)
            },
            new ClanQuestNpc {
                Index = 4, Hash = PedHash.Barry, Position = new Vector3(-2291.51, 363.55, 174.60),
                Rotation = new Vector3(0.00, 0.00, 13.04), ShapePosition = new Vector3(-2291.77, 364.20, 173.60)
            },
            new ClanQuestNpc {
                Index = 5, Hash = PedHash.Bestmen, Position = new Vector3(928.60, 46.15, 80.90),
                Rotation = new Vector3(0.00, 0.00, 62.01), ShapePosition = new Vector3(928.07, 46.49, 79.90)
            }
        };
    }
}