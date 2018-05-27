import { Menu } from '../Menu'

export class NurseMenu extends Menu {
    private static healTypes: number[] = [1, 3, 5, 7]

    constructor() {
        super('Госпиталь', 'Восстановить здоровье:')        
    }

    protected fillMenuByDefault(): void {
        for (let i = 0; i < NurseMenu.healTypes.length - 1; i++) {
            const type = NurseMenu.healTypes[i];
            let item = API.createMenuItem(`Восстановить ${type}0 хп`, 'Стоимость 10-ти хп 1$')
            item.Activated.connect(() => API.triggerServerEvent('RestoreHealth', type))
            this.menu.AddItem(item)
        }
        let fullHpItem = API.createMenuItem('~g~Полностью восстановиться', 'Стоимость 10-ти хп 1$')
        this.menu.AddItem(fullHpItem)
        fullHpItem.Activated.connect(() => API.triggerServerEvent('RestoreHealth', 0))
    }
}