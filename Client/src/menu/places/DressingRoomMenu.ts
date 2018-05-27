/// <reference path='../../../types-gt-mp/index.d.ts' />

import { Menu } from '../Menu'
import { ActiveCamera } from '../../utils/ActiveCamera'
import { DressingCameraManager } from '../../managers/places/DressingCameraManager'
import { DressingRoom } from '../../models/DressingRoom'
import { ClothesModel } from '../../models/ClothesModel'

export class DressingRoomMenu extends Menu {
    private dressingRoom: DressingCameraManager
    private good: ClothesModel

    constructor() {
        super('Примерочная')
    }

    public fillMenu(clothes: ClothesModel[], district: number) {
        this.menu.Clear()
        let hats = clothes.filter((good, index) => {return good.Slot == 0})
        this.createSubMenu(hats, 'Шапки', district)
        let tops = clothes.filter((good, index) => {return good.Slot == 11})
        this.createSubMenu(tops, 'Верхняя одежда', district)
        let legs = clothes.filter((good, index) => {return good.Slot == 4})
        this.createSubMenu(legs, 'Штаны', district)
        let feets = clothes.filter((good, index) => {return good.Slot == 6})
        this.createSubMenu(feets, 'Обувь', district)
        this.addBackItem()        
    }

    public initDressingRoomCamera(dressingRoom: DressingRoom) {
        ActiveCamera.setActiveCamera(dressingRoom.CameraHatsPosition, dressingRoom.CameraRotation)
        this.dressingRoom = new DressingCameraManager(dressingRoom)
        this.menu.OnIndexChange.connect((sender, index) => this.dressingRoom.moveCamera(index))
    }

    private addBackItem() {
        let backItem = API.createMenuItem('~r~Выйти', '')
        backItem.Activated.connect(() => {
            API.triggerServerEvent('ExitFromDressingRoom')
            API.setGameplayCameraActive()
            super.hide()
        })
        this.menu.AddItem(backItem)
    }

    private createSubMenu(clothes: ClothesModel[], name: string, district: number) : NativeUI.UIMenu {
        let result = API.addSubMenu(this.menu, name, '', true)
        let backItem =  API.createMenuItem('~r~Назад', '')
        backItem.Activated.connect(() => result.GoBack())
        result.AddItem(backItem)
        this.fillSubMenu(result, clothes)        
        result.OnIndexChange.connect((sender, index) => this.onClothesIndexChange(clothes, index))
        result.OnItemSelect.connect((sender, item, index) => this.onClothesSelect(index, district))
        return result
    }

    private fillSubMenu(menu: NativeUI.UIMenu, clothes: ClothesModel[]) {
        for (let index = 0; index < clothes.length; index++) {
            const good = clothes[index]
            let texturesList = new List(String)
            for (let texture = 0; texture < good.Textures.length; texture++) {
                texturesList.Add(`${texture + 1} / ${good.Textures.length}`)                
            }
            let desc = good.Price == 0 ? '' : `Стоимость: ${good.Price}$`
            let goodItem = API.createListItem(`Вариант ${index + 1}`, desc, texturesList, 0)
            menu.AddItem(goodItem)
            goodItem.OnListChanged.connect((sender, index) => {
                this.good.Texture = this.good.Textures[index]
                this.previewGood()
            })
        }
    }

    private onClothesIndexChange(clothes: ClothesModel[], index: number) {
        if (index == 0) {
            API.triggerServerEvent('DressPlayerClothes')
            return
        }
        this.good = clothes[index - 1]
        this.previewGood()
    }

    private onClothesSelect(index: number, district: number) {
        if (index == 0) {
            return
        }
        API.triggerServerEvent('DressOrBuyGood', JSON.stringify(this.good), district)
    }

    private previewGood() {
        let player = API.getLocalPlayer()
        if (this.good.Torso != null) {
            API.setPlayerClothes(player, 3, this.good.Torso, 0);
        }
        if (this.good.Undershirt != null) {
            API.setPlayerClothes(player, 8, this.good.Undershirt, 0);
        }      
        if (this.good.IsClothes) {
            API.setPlayerClothes(player, this.good.Slot, this.good.Variation, this.good.Texture)            
        } else {
            API.setPlayerAccessory(player, this.good.Slot, this.good.Variation, this.good.Texture)
        }        
    }

    private dressPlayerClothes() {
        let player = API.getLocalPlayer()
    }

    protected fillMenuByDefault() {
        this.menu.OnMenuClose.connect(() => API.triggerServerEvent('DressPlayerClothes'))
    }
}