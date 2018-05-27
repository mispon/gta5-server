import { Menu } from '../Menu'

export class ParachuteMenu extends Menu {
    constructor() {
        super('Парашют')        
    }

    protected fillMenuByDefault(): void {
        const price = 100
        let loaderItem = API.createMenuItem(`Купить парашют (${price}$)`, 'Приобрести 1шт.');
        this.menu.AddItem(loaderItem)
        loaderItem.Activated.connect(() => API.triggerServerEvent('BuyParachute', price))
    }
}