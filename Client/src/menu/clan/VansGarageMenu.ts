/// <reference path='../../../types-gt-mp/index.d.ts' />

import { Menu } from '../Menu'
import addCloseItem from '../../utils/CloseItem'

export class VansGarageMenu extends Menu {
    constructor() {
        super('Гараж фургонов')
    }

    public fillMenu(position: Vector3, rotation: Vector3) {
        this.menu.Clear()
        let spawnItem = API.createMenuItem('Вызвать фургон', '')
        spawnItem.Activated.connect(() => API.triggerServerEvent('SpawnMissionVans', position, rotation))
        this.menu.AddItem(spawnItem)
        addCloseItem(this.menu, 'Закрыть')
    }

    protected fillMenuByDefault() {}
}