/// <reference path='../../../types-gt-mp/index.d.ts' />

import { Menu } from '../Menu'

export class RentOfScootersMenu extends Menu {
    private bicycleItem: NativeUI.UIMenuItem
    private scooterItem: NativeUI.UIMenuItem
    private motocycleItem: NativeUI.UIMenuItem

    constructor() {
        super('Аренда скутеров')        
    }

    public show(args: any[] = []) {
        let position = args[0] as Vector3
        let rotation = args[1] as Vector3
        let district = args[2] as number
        this.createItems(position, rotation, district)
        this.menu.Clear()
        this.menu.AddItem(this.bicycleItem)
        this.menu.AddItem(this.scooterItem)
        this.menu.AddItem(this.motocycleItem)
        super.show()
    }

    private createItems(position: Vector3, rotation: Vector3, district: number) {
        const bicyclePrice = 50
        this.bicycleItem = API.createMenuItem('Велосипед', `Арендовать велосипед за ${bicyclePrice}$`);        
        this.bicycleItem.Activated.connect(() => API.triggerServerEvent('RentScooter', 0, bicyclePrice, position, rotation, district))

        const scooterPrice = 100
        this.scooterItem = API.createMenuItem('Скутер', `Арендовать скутер за ${scooterPrice}$`);        
        this.scooterItem.Activated.connect(() => API.triggerServerEvent('RentScooter', 1, scooterPrice, position, rotation, district))

        const motocyclePrice = 200
        this.motocycleItem = API.createMenuItem('Мотоцикл', `Арендовать мотоцикл за ${motocyclePrice}$`);        
        this.motocycleItem.Activated.connect(() => API.triggerServerEvent('RentScooter', 2, motocyclePrice, position, rotation, district))
    }

    protected fillMenuByDefault(): void {}
}