/// <reference path='../../../types-gt-mp/index.d.ts' />

import { Menu } from '../Menu'
import addCloseItem from '../../utils/CloseItem'

export class PoliceActionsMenu extends Menu {
    constructor() {
        super('Действия')
    }

    public triggerVisible(args: any[]) {
        let state = args[0] as number
        this.fillMenuByType(state)
        super.triggerVisible()
    }

    private fillMenuByType(state: number) {
        this.menu.Clear()
        switch (state) {
            case 1:
                this.addCheckItem()
                this.addPenaltyItem();
                this.addFinishAlertItem()
                break
            case 2:
                this.addPenaltyItem()
                this.addPutInCarItem()
                this.addReleaseItem()
                this.addFinishAlertItem()
                break
            case 3:
                this.addTakeFromCarItem()
                this.addReleaseItem()
                this.addFinishAlertItem()
                break
        }
        addCloseItem(this.menu, 'Закрыть')
    }

    private addCheckItem() {
        let checkItem = API.createMenuItem('Проверить', 'Проверить уровень розыска игрока')        
        checkItem.Activated.connect(() => API.triggerServerEvent('CheckPlayer'))
        this.menu.AddItem(checkItem)
    }

    private addPenaltyItem() {
        let penaltyItem = API.createMenuItem('Оштрафовать', 'Оштрафовать игрока')        
        penaltyItem.Activated.connect(() => API.triggerServerEvent('GivePenalty'))
        this.menu.AddItem(penaltyItem)
    }

    private addPutInCarItem() {
        let putInCarItem = API.createMenuItem('Посадить в машину', 'Посадить задержанного игрока в машину')        
        putInCarItem.Activated.connect(() => API.triggerServerEvent('PutPlayerInCar'))
        this.menu.AddItem(putInCarItem)
    }

    private addTakeFromCarItem() {
        let takeFromCarItem = API.createMenuItem('Достать из машины', 'Достать задержанного игрока из машины')        
        takeFromCarItem.Activated.connect(() => API.triggerServerEvent('TakePlayerFromCar'))
        this.menu.AddItem(takeFromCarItem)
    }

    private addReleaseItem() {
        let releaseItem = API.createMenuItem('Отпустить', 'Отпустить игрока')        
        releaseItem.Activated.connect(() => API.triggerServerEvent('ReleasePlayer'))
        this.menu.AddItem(releaseItem)
    }

    private addFinishAlertItem() {
        let finishAlertItem = API.createMenuItem('Завершить вызов', 'Отчитаться о завершении вызова')        
        finishAlertItem.SetRightBadge(BadgeStyle.Star)
        finishAlertItem.Activated.connect(() => API.triggerServerEvent('FinishPoliceAlert'))
        this.menu.AddItem(finishAlertItem)
    }

    protected fillMenuByDefault(): void {}
}