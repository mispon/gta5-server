/// <reference path='../../../types-gt-mp/index.d.ts' />

import {Menu} from '../Menu'
import addCloseItem from '../../utils/CloseItem'

export class ClanLeaderMenu extends Menu {
    private spriteDict: string
    private spriteName: string

    constructor() {
        super(' ')
    }

    public fillMenu(clanId: number, missionVotes: number, authority: number) {
        this.menu.Clear()
        this.setBannerSprite(clanId)
        this.addMissionMenu(clanId, missionVotes, authority)
        let questItem = API.createMenuItem('Задание', 'Выполнить задание лидера')
        questItem.Activated.connect(() => API.triggerServerEvent('AcceptClanQuest'))
        this.menu.AddItem(questItem)
        addCloseItem(this.menu, 'Закрыть')
    }

    private addMissionMenu(clanId: number, missionVotes: number, authority: number) {
        let missionMenu = API.addSubMenu(this.menu, 'Миссия', `Авторитет банды: ~b~${authority} ед.`, true)
        this.setBannerSprite(clanId)
        let joinItem = API.createMenuItem(`Запустить  ~b~(${missionVotes} / 2)`, 'Принять участие в миссии банды')
        joinItem.SetRightBadge(BadgeStyle.Michael)
        joinItem.Activated.connect(() => API.triggerServerEvent('JoinClanMission', clanId))
        missionMenu.AddItem(joinItem)

        let leftItem = API.createMenuItem('~m~Отказаться', 'Отменить свое участие в миссии')
        leftItem.Activated.connect(() => API.triggerServerEvent('LeftClanMission', clanId))
        missionMenu.AddItem(leftItem)
        addCloseItem(missionMenu)
    }

    private setBannerSprite(clanId: number) {
        switch(clanId) {
            case 1:
                this.spriteDict = 'shopui_title_graphics_michael'
                this.spriteName = 'shopui_title_graphics_michael'
                break               
            case 2:
                this.spriteDict = 'shopui_title_graphics_trevor'
                this.spriteName = 'shopui_title_graphics_trevor'
                break               
            case 3:            	
                this.spriteDict = 'shopui_title_graphics_franklin'
                this.spriteName = 'shopui_title_graphics_franklin'
                break
        }
        API.setMenuBannerSprite(this.menu, this.spriteDict, this.spriteName)
    }

    protected fillMenuByDefault() {}
}