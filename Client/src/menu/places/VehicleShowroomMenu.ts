/// <reference path='../../../types-gt-mp/index.d.ts' />

import { Menu } from '../Menu'
import { ShowroomVehicle } from '../../models/ShowroomVehicle'
import { ShowroomPositions } from '../../models/ShowroomPositions'
import { ActiveCamera } from '../../utils/ActiveCamera'
import addBackItem from '../../utils/CloseItem'

export class VehicleShowroomMenu extends Menu {
    private buyMenu: NativeUI.UIMenu
    private vehicleOnPreview: LocalHandle
    private colors = new List(String)
    private primaryColor: number = 0
    private secondColor: number = 0

    constructor() {
        super('Автосалон')
        const colorsCount = 160
        for (let color = 1; color < colorsCount; color++) {            
            this.colors.Add(`${color} / ${colorsCount}`)
        }
    }

    public fillMenu(vehicles: ShowroomVehicle[], playerVehicles: ShowroomVehicle[], positions: ShowroomPositions, type: number, district: number) {
        this.menu.Clear()
        this.menu.Children.Clear()
        this.addBuyMenu(vehicles, positions, type, district)
        this.addSellMenu(playerVehicles)
        let closeItem = API.createMenuItem('~r~Закрыть', '')
        closeItem.Activated.connect(() => {
            API.triggerServerEvent('ExitFromVehiclePreview')
            this.hide()
            API.setGameplayCameraActive()
        })
        this.menu.AddItem(closeItem)
        this.menu.OnItemSelect.connect((sender, item, index) => this.setPreviewCamera(index, positions))
    }

    private addBuyMenu(vehicles: ShowroomVehicle[], positions: ShowroomPositions, type: number, district: number) {
        this.buyMenu = API.addSubMenu(this.menu, 'Покупка', '', true)
        let vehiclesGroup = this.groupVehicles(vehicles)
        switch (type) {
            case 1:
                this.createCheapMenuItems(vehiclesGroup, positions, district)
                break
            case 2:
                this.createExpensiveMenuItems(vehiclesGroup, positions, district)
                break
            case 3:
                this.createVehiclesList(vehicles, positions, 'Транспорт банды', district)
                break
        }
        addBackItem(this.buyMenu)
    }

    private addSellMenu(playerVehicles: ShowroomVehicle[]) {
        let sellMenu = API.addSubMenu(this.menu, 'Продажа', '', true)
        playerVehicles.forEach(vehicle => {
            let name = API.getVehicleDisplayName(vehicle.Hash)
            let submenu = API.addSubMenu(sellMenu, name, '', true)
            let sellItem = API.createMenuItem('Продать', `Сумма продажи ${vehicle.Price}$`)
            sellItem.Activated.connect(() => {
                API.closeAllMenus()
                this.hide()
                API.triggerServerEvent('SellVehicle', vehicle.Id, vehicle.Price)
            })
            submenu.AddItem(sellItem)
            addBackItem(submenu, 'Отмена')
        })
        addBackItem(sellMenu)
    }

    private createCheapMenuItems(vehicles: GroupedVehicles, positions: ShowroomPositions, district: number) {
        this.createCommonPart(vehicles, positions, district)
        this.createVehiclesList(vehicles.mini, positions, 'Мини', district)
        this.createVehiclesList(vehicles.sedans, positions, 'Седаны', district)
    }

    private createExpensiveMenuItems(vehicles: GroupedVehicles, positions: ShowroomPositions, district: number) {
        this.createCommonPart(vehicles, positions, district)
        this.createVehiclesList(vehicles.sport, positions, 'Спорткары', district)
        this.createVehiclesList(vehicles.super, positions, 'Суперкары', district)
    }

    private createCommonPart(vehicles: GroupedVehicles, positions: ShowroomPositions, district: number) {
        this.createVehiclesList(vehicles.moto, positions, 'Мотоциклы', district)
        this.createVehiclesList(vehicles.suv, positions, 'Кроссоверы', district)
        this.createVehiclesList(vehicles.muscle, positions, 'Маслкары', district)
    }

    private createVehiclesList(vehicles: ShowroomVehicle[], positions: ShowroomPositions, name: string, district: number) : NativeUI.UIMenu {
        let result = API.addSubMenu(this.buyMenu, name, '', true)
        this.addBackItem(result)
        for (let index = 0; index < vehicles.length; index++) {
            const vehicle = vehicles[index];
            const vehicleName = API.getVehicleDisplayName(vehicle.Hash)
            const maxSpeed = API.getVehicleMaxSpeed(vehicle.Hash) * 3.6
            let vehicleMenu = API.addSubMenu(result, vehicleName, `~g~${vehicle.Price}$~w~, ~b~${Math.floor(maxSpeed)} km/h`, true)
            this.fillVehicleMenu(vehicleMenu, vehicle, district)
        }
        result.OnIndexChange.connect((sender, index) => {
            if (index == 0) {
                API.deleteEntity(this.vehicleOnPreview)
            }
            this.previewVehicle(vehicles[index - 1], positions)          
        })
        result.OnMenuClose.connect(() => API.deleteEntity(this.vehicleOnPreview))
        return result
    }

    private fillVehicleMenu(vehicleMenu: NativeUI.UIMenu, vehicle: ShowroomVehicle, district: number) {
        vehicleMenu.AddItem(this.createColorPicker(true))   
        vehicleMenu.AddItem(this.createColorPicker(false))        
        vehicleMenu.AddItem(this.createBuyItem(vehicleMenu, vehicle, false, district))
        vehicleMenu.AddItem(this.createBuyItem(vehicleMenu, vehicle, true, district))
        this.addBackItem(vehicleMenu)
    }

    private createColorPicker(main: boolean): NativeUI.UIMenuListItem {
        let picker = API.createListItem(`${main ? 'Основной' : 'Дополнительный'} цвет`, '', this.colors, 0)
        picker.OnListChanged.connect((sender, color) => {
            if (main) {
                API.setVehiclePrimaryColor(this.vehicleOnPreview, color)
                this.primaryColor = color
            } 
            else {
                API.setVehicleSecondaryColor(this.vehicleOnPreview, color)
                this.secondColor = color
            }
        })
        return picker
    }

    private createBuyItem(vehicleMenu: NativeUI.UIMenu, vehicle: ShowroomVehicle, toHouse: boolean, district: number): NativeUI.UIMenuItem {
        let desc = toHouse ? 'Отправить транспорт в гараж дома' : 'Отправить транспорт на главную парковку'
        let buyItem = API.createMenuItem(`~g~Купить за ${vehicle.Price}$ (${toHouse ? 'Домой' : 'На парковку'})`, desc)
        buyItem.Activated.connect(() => {
            API.triggerServerEvent('BuyVehicle', JSON.stringify(vehicle), this.primaryColor, this.secondColor, toHouse, district)
            vehicleMenu.GoBack()
        })
        return buyItem
    } 

    private previewVehicle(vehicle: ShowroomVehicle, positions: ShowroomPositions) {
        if (this.vehicleOnPreview != null) {
            API.deleteEntity(this.vehicleOnPreview)
        }
        this.vehicleOnPreview = this.createVehicle(vehicle.Hash, positions.PreviewPosition, positions.PreviewRotation)
    }

    private createVehicle(hash: number, pos: Vector3, rot: Vector3) : LocalHandle {        
        let position = new Vector3(pos.X, pos.Y, pos.Z)
        let rotation = new Vector3(rot.X, rot.Y, rot.Z)
        let result = API.createVehicle(hash, position, rotation)
        let player = API.getLocalPlayer()        
        API.setEntityDimension(result, API.getEntityDimension(player))
        return result
    }

    private addBackItem(menu: NativeUI.UIMenu) {
        let backItem = API.createMenuItem('~r~Назад', '')
        backItem.Activated.connect(() => menu.GoBack())
        menu.AddItem(backItem)
    }

    private setPreviewCamera(index: number, positions: ShowroomPositions) {
        if (index != 0) {
            return
        }
        let player = API.getLocalPlayer()        
        if (API.hasEntitySyncedData(player, 'IsRegistered')) {
            API.sendNotification('~r~Вы находитесь в ожидании эвента')
            return
        }
        ActiveCamera.setActiveCamera(positions.CameraPosition, positions.CameraRotation)
        API.triggerServerEvent('EnterToVehiclePreview')
    }

    private groupVehicles(vehicles: ShowroomVehicle[]): GroupedVehicles {
        let result = new GroupedVehicles()
        vehicles.forEach((vehicle) => {
            switch (vehicle.Type) {
                case 1:
                result.moto.push(vehicle)
                    break
                case 2:
                result.mini.push(vehicle)
                    break
                case 3:
                result.sedans.push(vehicle)
                    break
                case 4:
                result.suv.push(vehicle)
                    break
                case 5:
                result.muscle.push(vehicle)
                    break
                case 6:
                result.sport.push(vehicle)
                    break
                case 7:
                result.super.push(vehicle)
                    break
            }
        })
        return result
    }

    protected fillMenuByDefault() {
        this.menu.OnMenuClose.connect(() => {
            API.triggerServerEvent('ExitFromVehiclePreview')
            API.setGameplayCameraActive()
        })
    }
}

class GroupedVehicles {
    public moto: ShowroomVehicle[] = new Array<ShowroomVehicle>()
    public mini: ShowroomVehicle[] = new Array<ShowroomVehicle>()
    public sedans: ShowroomVehicle[] = new Array<ShowroomVehicle>()
    public suv: ShowroomVehicle[] = new Array<ShowroomVehicle>()
    public muscle: ShowroomVehicle[] = new Array<ShowroomVehicle>()
    public sport: ShowroomVehicle[] = new Array<ShowroomVehicle>()
    public super: ShowroomVehicle[] = new Array<ShowroomVehicle>()
}