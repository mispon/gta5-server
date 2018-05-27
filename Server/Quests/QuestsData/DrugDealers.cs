using System.Collections.Generic;
using gta_mp_server.Models.Clan;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Shared.Math;

namespace gta_mp_server.Quests.QuestsData {
    /// <summary>
    /// Данные задания на рэкет фермеров
    /// </summary>
    internal class DrugDealers {
        /// <summary>
        /// Драг-дилеры
        /// </summary>
        internal static List<ClanQuestNpc> Npcs = new List<ClanQuestNpc> {
            new ClanQuestNpc {
                Index = 1, Hash = PedHash.Dealer01SMY, Position = new Vector3(469.27, -863.95, 26.70),
                Rotation = new Vector3(0.00, 0.00, -66.62), ShapePosition = new Vector3(470.08, -863.75, 25.67)
            },
            new ClanQuestNpc {
                Index = 2, Hash = PedHash.Dealer01SMY, Position = new Vector3(336.06, -1100.61, 29.41),
                Rotation = new Vector3(0.00, 0.00, -48.79), ShapePosition = new Vector3(336.69, -1100.05, 28.41)
            },
            new ClanQuestNpc {
                Index = 3, Hash = PedHash.ChinGoonCutscene, Position = new Vector3(85.21, -1955.28, 20.79),
                Rotation = new Vector3(0.00, 0.00, -42.67), ShapePosition = new Vector3(85.70, -1954.67, 19.80)
            },
            new ClanQuestNpc {
                Index = 4, Hash = PedHash.Stretch, Position = new Vector3(-174.33, -1562.66, 35.36),
                Rotation = new Vector3(0.00, 0.00, -125.03), ShapePosition = new Vector3(-173.87, -1563.04, 34.36)
            },
            new ClanQuestNpc {
                Index = 5, Hash = PedHash.Families01GFY, Position = new Vector3(412.35, -1817.09, 28.55),
                Rotation = new Vector3(0.00, 0.00, 52.82), ShapePosition = new Vector3(411.99, -1816.59, 27.56)
            },
            new ClanQuestNpc {
                Index = 6, Hash = PedHash.Genfat01AMM, Position = new Vector3(381.03, -2039.13, 22.94),
                Rotation = new Vector3(0.00, 0.00, 69.26), ShapePosition = new Vector3(380.26, -2038.86, 21.79)
            },
            new ClanQuestNpc {
                Index = 7, Hash = PedHash.GroveStrDlrCutscene, Position = new Vector3(954.96, -1738.38, 30.97),
                Rotation = new Vector3(0.00, 0.00, 179.30), ShapePosition = new Vector3(954.93, -1739.01, 29.98)
            },
            new ClanQuestNpc {
                Index = 8, Hash = PedHash.RampMex, Position = new Vector3(969.80, -1810.56, 31.23),
                Rotation = new Vector3(0.00, 0.00, 177.87), ShapePosition = new Vector3(969.71, -1811.44, 30.21)
            },
            new ClanQuestNpc {
                Index = 9, Hash = PedHash.Salton04AMM, Position = new Vector3(1073.67, -2451.84, 29.10),
                Rotation = new Vector3(0.00, 0.00, 95.62), ShapePosition = new Vector3(1073.12, -2451.85, 28.08)
            },
            new ClanQuestNpc {
                Index = 10, Hash = PedHash.Soucent02AMY, Position = new Vector3(979.76, -1864.63, 31.19),
                Rotation = new Vector3(0.00, 0.00, -88.66), ShapePosition = new Vector3(980.23, -1864.68, 30.17)
            }
        };
    }
}