/// <reference path='../../../types-gt-mp/index.d.ts' />

import { Menu } from '../Menu'
import addCloseItem from '../../utils/CloseItem'

export class MainClanMenu extends Menu {
    constructor() {
        super('Банда')
    }

    public fillMenu(clanId: number) {
        this.menu.Clear()
        let joinItem = API.createMenuItem('Вступить в банду', 'Стать участником банды')
        joinItem.Activated.connect(() => API.triggerServerEvent('JoinClan', clanId))
        this.menu.AddItem(joinItem)

        let leftItem = API.createMenuItem('~m~Покинуть банду', 'При выходе вы ~r~потеряете весь прогресс')
        leftItem.Activated.connect(() => API.triggerServerEvent('LeftClan'))
        this.menu.AddItem(leftItem)

        addCloseItem(this.menu, 'Закрыть')
    }

    protected fillMenuByDefault() {}
}