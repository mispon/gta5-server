import { Menu } from '../Menu'

export class ParkingMenu extends Menu {
    private parkItem: NativeUI.UIMenuItem
    private closeItem: NativeUI.UIMenuItem

    constructor() {
        super('Парковка')
    }

    public fillMenu(vehicles: any[]) {
        this.menu.Clear()
        this.createSubMenu(vehicles)
        this.menu.AddItem(this.parkItem)
        this.menu.AddItem(this.closeItem)
    }

    private createSubMenu(vehicles: any[]): NativeUI.UIMenu {
        let vehiclesMenu = API.addSubMenu(this.menu, 'Забрать транспорт', 'Появится внутри парковки', true)
        let backItem = API.createMenuItem('~r~Назад', '')
        backItem.Activated.connect(() => vehiclesMenu.GoBack())
        vehiclesMenu.AddItem(backItem)
        for (let index = 0; index < vehicles.length; index++) {
            const vehicle = vehicles[index];
            let name = API.getVehicleDisplayName(vehicle.Hash)
            let vehicleItem = API.createMenuItem(name, '')
            vehicleItem.Activated.connect(() => {
                API.triggerServerEvent('GetVehicleFromParking', JSON.stringify(vehicle))
                vehiclesMenu.GoBack()
            })
            vehiclesMenu.AddItem(vehicleItem)
        }
        return vehiclesMenu
    }

    protected fillMenuByDefault() {
        const price = 50
        this.parkItem = API.createMenuItem('Припарковать', `Стоимость парковки ${price}$`)
        this.parkItem.Activated.connect(() => API.triggerServerEvent('ParkVehicle', price))
        this.closeItem = API.createMenuItem('~r~Закрыть', '')
        this.closeItem.Activated.connect(() => this.hide())
    }
}