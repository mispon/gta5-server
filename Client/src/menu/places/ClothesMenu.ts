import { Menu } from '../Menu'
import { DressingRoom } from '../../models/DressingRoom'

export class ClothesMenu extends Menu {
    constructor(type: number) {
        if (type != 0) {
            super(' ')
            let spriteDisc = type == 1 ? 'shopui_title_midfashion' : 'shopui_title_highendfashion'
            let spriteName = type == 1 ? 'shopui_title_midfashion' : 'shopui_title_highendfashion'
            API.setMenuBannerSprite(this.menu, spriteDisc, spriteName)
        }
        else {
            super('Гардероб')
        }        
    }

    public initialize(dressingRoom: DressingRoom) {
        this.menu.Clear()
        API.addSubMenu(this.menu, 'Выбрать одежду', '', true)
        let closeItem = API.createMenuItem('~r~Закрыть', '')
        closeItem.Activated.connect(() => this.menu.GoBack())
        this.menu.AddItem(closeItem)   
        this.menu.OnItemSelect.connect(
            (sender: NativeUI.UIMenu, selectedItem: NativeUI.UIMenuItem, index: number) => {
                this.toDressingRoom(index, dressingRoom)
            }
        )
    }

    protected fillMenuByDefault() {}

    private toDressingRoom(index: number, dressingRoom: DressingRoom) {
        if (index != 0) {
            return
        }
        let player = API.getLocalPlayer()        
        if (API.hasEntitySyncedData(player, 'IsRegistered')) {
            API.sendNotification('~r~Вы находитесь в ожидании эвента')
            return
        }
        API.triggerServerEvent('GoToDressingRoom', JSON.stringify(dressingRoom))
    }
}