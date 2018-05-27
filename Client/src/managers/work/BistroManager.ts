import { BistroMenu } from '../../menu/work/BistroMenu'
import { BistroFoodMenu } from '../../menu/work/BistroFoodMenu'
import { ServerEventHandler } from '../../event-handlers/ServerEventHandler'

namespace BistoManager { 
    let currentMarker: LocalHandle
    let currentLabel: LocalHandle
    let bistroMenu: BistroMenu = new BistroMenu()
    let bistroFoodMenu: BistroFoodMenu = new BistroFoodMenu()

    ServerEventHandler.getInstance().on('ShowBistroMenu', showBistroMenu)
    ServerEventHandler.getInstance().on('HideBistroMenu', hideBistroMenu)
    ServerEventHandler.getInstance().on('ShowBistroFoodMenu', showBistroFoodMenu)
    ServerEventHandler.getInstance().on('HideBistroFoodMenu', hideBistroFoodMenu)
    ServerEventHandler.getInstance().on('ShowDeliveryPoint', showNextDeliveryPoint)

    API.onKeyDown.connect((sender, args) => {
        let player = API.getLocalPlayer()
        if (API.hasEntitySyncedData(player, 'OnDeliveryPoint') && args.KeyCode == Keys.E) {
            API.triggerServerEvent('CompleteDelivery')
        }
        if (API.hasEntitySyncedData(player, 'PointNumber') && args.KeyCode == Keys.O) {
            var position = API.getEntityPosition(currentLabel)
            API.setWaypoint(position.X, position.Y)
        }
    })

    function showBistroMenu(args: any[]) {
        bistroMenu.show()
    }

    function hideBistroMenu() {
        bistroMenu.hide()
    }

    function showBistroFoodMenu(args: any[]) {
        let driverName = ''
        if (args.length > 0) {
            driverName = args[0] as string
        }
        bistroFoodMenu.fillMenu(driverName)
        bistroFoodMenu.show()
    }

    function hideBistroFoodMenu() {
        bistroFoodMenu.hide()
    }

    function showNextDeliveryPoint(args: any[]): void {
        let position = args[0] as Vector3
        deleteCurrentPoint()        
        currentMarker = API.createMarker(1, position, new Vector3(), new Vector3(), new Vector3(1, 1, 1), 255, 100, 50, 200)
        currentLabel = API.createTextLabel('Нажмите ~y~Е', position.Add(new Vector3(0, 0, 1.5)), 10, 0.5)
        API.setWaypoint(position.X, position.Y)
    }

    function deleteCurrentPoint(): void {
        if (currentMarker) {
            API.deleteEntity(currentMarker)
            API.deleteEntity(currentLabel)
        }
        API.removeWaypoint()
    }
}