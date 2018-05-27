using System.Collections.Generic;
using gta_mp_data.Enums;
using gta_mp_server.Managers.Player.Interfaces;
using gta_mp_server.Managers.Work.Police.Interfaces;
using gta_mp_server.Models.Work;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;

namespace gta_mp_server.Managers.Work.Police {
    /// <summary>
    /// Логика работы полицейского
    /// </summary>
    internal class PoliceRewardManager : Script, IPoliceRewardManager {
        private readonly IPlayerInfoManager _playerInfoManager;
        private readonly IWorkInfoManager _workInfoManager;

        private readonly Dictionary<long, WorkReward> _workReward = new Dictionary<long, WorkReward> {
            [1] = new WorkReward {Salary = 80, Exp = 38, WorkExp = 12},
            [2] = new WorkReward {Salary = 120, Exp = 40, WorkExp = 13},
            [3] = new WorkReward {Salary = 170, Exp = 42, WorkExp = 14},
            [4] = new WorkReward {Salary = 230, Exp = 45, WorkExp = 15},
            [5] = new WorkReward {Salary = 300, Exp = 50, WorkExp = 0}
        };

        public PoliceRewardManager() {}
        public PoliceRewardManager(IPlayerInfoManager playerInfoManager, IWorkInfoManager workInfoManager) {
            _playerInfoManager = playerInfoManager;
            _workInfoManager = workInfoManager;
        }

        /// <summary>
        /// Начислить награду за патрулирование
        /// </summary>
        public void SetPatrolReward(Client player) {
            var workInfo = _workInfoManager.GetWorkInfo(player, WorkType.Police);
            var reward = _workReward[workInfo.Level];
            _playerInfoManager.SetExperience(player, reward.Exp);
            _workInfoManager.SetSalary(player, WorkType.Police, reward.Salary);
            _workInfoManager.SetExperience(player, WorkType.Police, reward.WorkExp);
        }

        /// <summary>
        /// Начислить награду за арест
        /// </summary>
        public void SetEffortReward(Client policeman, Client prisoner) {
            var reward = CalculateReward(prisoner);
            SetReward(policeman, reward);
        }

        /// <summary>
        /// Вычисляет кол-во начисляемого опыта
        /// </summary>
        private WorkReward CalculateReward(Client prisoner) {
            var prisonerInfo = _playerInfoManager.GetInfo(prisoner);
            var exp = prisonerInfo.Wanted.WantedLevel + prisonerInfo.Level;
            return new WorkReward {
                Salary = PoliceManager.CalculatePenalty(prisonerInfo) * 2,
                Exp = exp,
                WorkExp = exp
            };
        }

        /// <summary>
        /// Применить награду
        /// </summary>
        private void SetReward(Client policeman, WorkReward reward) {
            _workInfoManager.SetSalary(policeman, WorkType.Police, reward.Salary);
            _playerInfoManager.SetExperience(policeman, reward.Exp);
            _workInfoManager.SetExperience(policeman, WorkType.Police, reward.WorkExp);
        }
    }
}