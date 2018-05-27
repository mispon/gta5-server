import { ServerEventHandler } from '../event-handlers/ServerEventHandler'
import { RaceMenu } from '../menu/RaceMenu'

namespace Race {       
    let menu: RaceMenu = new RaceMenu()  

    ServerEventHandler.getInstance().on('ShowRaceMenu', showMenu)
    ServerEventHandler.getInstance().on('HideRaceMenu', hideMenu)

    function showMenu(args: any[]): void {
        let cars = JSON.parse(args[0])
        let motos = JSON.parse(args[1])
        let membersInfo = JSON.parse(args[2])
        menu.fillMenu(cars, motos, membersInfo)
        menu.show()
    }

    function hideMenu(): void {
        menu.hide()
    }
}