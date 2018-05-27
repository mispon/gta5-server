/// <reference path='../../types-gt-mp/index.d.ts' />

import { Menu } from './Menu'
import addCloseItem from '../utils/CloseItem'

export class RacecourseMenu extends Menu {
    constructor() {
        super('Ипподром')
    }

    protected fillMenuByDefault(): void {
        let startRaceItem = API.createMenuItem('Запустить гонку', '')
        startRaceItem.Activated.connect(() => API.triggerServerEvent('OnStartRacecourseRace'))
        this.menu.AddItem(startRaceItem)

        let stopRaceItem = API.createMenuItem('Остановить гонку', '')
        stopRaceItem.Activated.connect(() => API.triggerServerEvent('StopRacecourseRace'))
        this.menu.AddItem(stopRaceItem)

        let spawnCarItem = API.createMenuItem('~b~Создать чебуреки', '')
        spawnCarItem.Activated.connect(() => API.triggerServerEvent('SpawnRacecourseCars'))
        this.menu.AddItem(spawnCarItem)

        let removeCarItem = API.createMenuItem('~r~Удалить чебуреки', '')
        removeCarItem.Activated.connect(() => API.triggerServerEvent('RemoveRacecourseCars'))
        this.menu.AddItem(removeCarItem)
    }
}