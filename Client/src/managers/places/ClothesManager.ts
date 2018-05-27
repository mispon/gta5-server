import { ClothesMenu } from '../../menu/places/ClothesMenu'
import { DressingRoomMenu } from '../../menu/places/DressingRoomMenu'
import { DressingRoom } from '../../models/DressingRoom'
import { ClothesModel } from '../../models/ClothesModel'
import { ServerEventHandler } from '../../event-handlers/ServerEventHandler'

namespace ClothesManager {
    let clothesMenu: ClothesMenu
    let dressingMenu: DressingRoomMenu = new DressingRoomMenu()

    ServerEventHandler.getInstance().on('ShowClothesMenu', showClothesMenu)
    ServerEventHandler.getInstance().on('HideClothesMenu', hideClothesMenu)
    ServerEventHandler.getInstance().on('ShowClothesList', showClothesList)

    API.onKeyDown.connect((sender, arg) => {
        let player = API.getLocalPlayer()
        if (!API.hasEntitySyncedData(player, 'InDressingRoom')) {
            return
        }
        if (arg.KeyCode == Keys.O) {
            if (dressingMenu.isOpen) {
                dressingMenu.hide()
            }
            dressingMenu.show()
        }
    })

    function showClothesMenu(args: any[]) {
        let type = args[0] as number
        let dressingRoom = JSON.parse(args[1]) as DressingRoom
        let clothes = JSON.parse(args[2]) as ClothesModel[]
        let district = args[3] as number
        clothesMenu = new ClothesMenu(type)        
        clothesMenu.initialize(dressingRoom) 
        dressingMenu.fillMenu(clothes, district)       
        clothesMenu.show() 
    }

    function hideClothesMenu() {
        clothesMenu.hide()
    }

    function showClothesList(args: any[]) {
        let dressingRoom = JSON.parse(args[0]) as DressingRoom
        dressingMenu.initDressingRoomCamera(dressingRoom)
        dressingMenu.show()
    }
}