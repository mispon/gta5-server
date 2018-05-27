/// <reference path='../../../types-gt-mp/index.d.ts' />

import { Menu } from '../Menu'
import { TuningInfo } from '../../models/TuningInfo'
import { TuningCameraManager } from '../../managers/places/TuningCameraManager'
import addCloseItem from '../../utils/CloseItem'
import rgbColors from '../../utils/Colors'

export class TuningMenu extends Menu {
    private default: string = 'станд.'
    private engineValues: number[] = [10, 15, 20, 25, 30]
    private colors = new List(String)
    private cameraManager: TuningCameraManager

    constructor() {        
        super(' ')
        TuningMenu.setMenuBanner(this.menu)
        const colorsCount = 160
        for (let color = 1; color < colorsCount; color++) {            
            this.colors.Add(`${color} / ${colorsCount}`)
        }
        this.cameraManager = new TuningCameraManager()     
    }

    public show() {
        this.cameraManager.setFrontCamera()
        super.show()
    }

    public fillMenu(tuningInfo: TuningInfo[], hasNeon: boolean) {
        this.menu.Clear()        
        tuningInfo.forEach((info) => {
            this.addTuningItem(info)
        });
        if (hasNeon) {
            this.addNeonItem()
        }        
        this.addEngineItem()
        this.addColorPicker(true)
        this.addColorPicker(false)        
        this.addRepairItem()
        addCloseItem(this.menu, 'Закрыть')
        this.menu.OnIndexChange.connect((sender, index) => {
            let slot = -1
            if (index <= tuningInfo.length) {
                slot = tuningInfo[index].Slot
            }
            this.cameraManager.moveCamera(slot)
        })
    }

    // Добавляет подменю тюнинга
    private addTuningItem(info: TuningInfo) {
        let list = this.createListItem(info.Values.length)
        let selectedMod = API.getVehicleMod(this.getVehicle(), info.Slot) + 1
        let tuningItem = API.createListItem(info.Name, this.default, list, selectedMod)
        tuningItem.OnListChanged.connect((sender, index) => {
            tuningItem.Description = index == 0 ? this.default : `Стоимость: ${info.Price}$`
            let value = info.Values[index]
            API.setVehicleMod(this.getVehicle(), info.Slot, value)
        })
        tuningItem.Activated.connect(() => {
            let value = info.Values[tuningItem.Index]
            API.triggerServerEvent('SetVehicleMod', info.Price, info.Slot, value)
        })
        this.menu.AddItem(tuningItem)
    }

    // Меню выбора неона
    private addNeonItem() {
        const neonPrice = 5000
        let colorsList = this.createListItem(rgbColors.length)
        let neonItem = API.createListItem('~o~Неоновая подсветка', this.default, colorsList, 0)
        neonItem.OnListChanged.connect((sender, index) => {
            neonItem.Description = index == 0 ? this.default : `Стоимость: ${neonPrice}$`
            let vehicle = this.getVehicle()
            this.turnOnNeon(vehicle)
            let color = rgbColors[index]
            API.setVehicleNeonColor(vehicle, color['r'], color['g'], color['b'])                   
        })
        neonItem.Activated.connect(() => {
            let color = rgbColors[neonItem.Index]
            API.triggerServerEvent('SetNeon', neonPrice, color['r'], color['g'], color['b'])
        })
        this.menu.AddItem(neonItem)
    }

    // Включает неоновую подсветку
    private turnOnNeon(vehicle: LocalHandle) {
        for (let i = 0; i <= 3; i++) {
            API.setVehicleNeonState(vehicle, i, true)
        }
    }

    // Добавляет элемент выбора мощности движка
    private addEngineItem() {
        const desc = 'Стоимость: '
        const enginePowerCoef = 1500
        let listItem = this.createListItem(this.engineValues.length)
        let enginePowerItem = API.createListItem('~q~Разгон двигателя', this.default, listItem, 0)
        enginePowerItem.OnListChanged.connect((sender, index) => {
            enginePowerItem.Description = `${desc}${this.getEnginePrice(index, enginePowerCoef)}$`
            API.setVehicleEnginePowerMultiplier(this.getVehicle(), this.engineValues[index])
        })
        enginePowerItem.Activated.connect(() => {
            let power = this.engineValues[enginePowerItem.Index]
            API.triggerServerEvent('SetEnginePower', this.getEnginePrice(enginePowerItem.Index, enginePowerCoef), power)
        })
        this.menu.AddItem(enginePowerItem)
    }

    // Выбор цвета корпуса
    private addColorPicker(isMain: boolean) {
        const price = isMain ? 1500 : 400;
        let picker = API.createListItem(`~b~${isMain ? 'Основной' : 'Дополнительный'} цвет`, `Стоимость перекраски: ${price}$`, this.colors, 0)
        picker.OnListChanged.connect((sender, index) => {
            if (isMain) {
                API.setVehiclePrimaryColor(this.getVehicle(), index)                
            } 
            else {
                API.setVehicleSecondaryColor(this.getVehicle(), index)                
            }
        })
        picker.Activated.connect(() => {
            let color = picker.Index + 1
            API.triggerServerEvent('SetVehicleColor', price, color, isMain)
        })
        this.menu.AddItem(picker)
    }

    // Добавляет элемент починки транспорта
    private addRepairItem() {
        const repairPrice = 2000;
        let repairItem = API.createMenuItem('~g~Починить транспорт', `Стоимость: ${repairPrice}$`)
        repairItem.Activated.connect(() => API.triggerServerEvent('RepairVehicle', repairPrice))
        this.menu.AddItem(repairItem)
    }

    // Возвращает стоимость апгрейда движка
    private getEnginePrice(index: number, coef: number) : number {
        return this.engineValues[index] * coef
    }

    // Возращает элемент-список
    private createListItem(count: number) : System.Collections.Generic.List<any> {
        let result = new List(String)
        for (let i = 1; i <= count; i++) {
            result.Add(`${i} / ${count}`)            
        }
        return result
    }

    private getVehicle(): LocalHandle {
        let player = API.getLocalPlayer()
        return API.getPlayerVehicle(player)
    }
 
    private static setMenuBanner(menu: NativeUI.UIMenu) {
        API.setMenuBannerSprite(menu, 'shopui_title_carmod', 'shopui_title_carmod');
    }

    protected fillMenuByDefault() {
        this.menu.OnMenuClose.connect(() => {
            API.triggerServerEvent('ExitFromTuningGarage')
            API.setGameplayCameraActive()
        })
    }
}