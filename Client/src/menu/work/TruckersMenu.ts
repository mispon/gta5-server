/// <reference path='../../../types-gt-mp/index.d.ts' />

import { DeliveryContract } from '../../models/DeliveryContract'
import { Menu } from '../Menu'

export class TruckersMenu extends Menu {
    constructor() {
        super('Дальнобойщики', 'Выберите контракт или награду:')        
    }

    public fillMenu(contracts: DeliveryContract[]) {
        this.menu.Clear()
        contracts.forEach(contract => {
            let item = API.createMenuItem(`${contract.Name} - ${contract.Reward}$`, '')
            item.Activated.connect(() => API.triggerServerEvent('ChooseTruckerContract', JSON.stringify(contract)))
            this.menu.AddItem(item)
        });
        let salaryItem = API.createMenuItem('~b~Получить деньги', '')
        salaryItem.Activated.connect(() => API.triggerServerEvent('TruckerSalary'))
        this.menu.AddItem(salaryItem)
    }

    protected fillMenuByDefault(): void {}
}