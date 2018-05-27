/// <reference path='../../../types-gt-mp/index.d.ts' />

import { VehicleShowroomMenu } from '../../menu/places/VehicleShowroomMenu'
import { ShowroomVehicle } from '../../models/ShowroomVehicle'
import { ShowroomPositions } from '../../models/ShowroomPositions'
import { ServerEventHandler } from '../../event-handlers/ServerEventHandler'

namespace VehicleShowroomManager {
    let showroomMenu: VehicleShowroomMenu = new VehicleShowroomMenu()

    ServerEventHandler.getInstance().on('ShowShowroomMenu', showShowroomMenu)
    ServerEventHandler.getInstance().on('HideShowroomMenu', hideShowroomMenu)

    API.onKeyDown.connect((sender, arg) => {
        let player = API.getLocalPlayer()
        if (!API.hasEntitySyncedData(player, 'InShowroomPreview')) {
            return
        }
        if (arg.KeyCode == Keys.O) {
            if (showroomMenu.isOpen) {
                showroomMenu.hide()
            }
            showroomMenu.show()
        }
    })

    function showShowroomMenu(args: any[]) {
        let vehicles = JSON.parse(args[0]) as ShowroomVehicle[]
        let playerVehicles = JSON.parse(args[1]) as ShowroomVehicle[]
        let positions = JSON.parse(args[2]) as ShowroomPositions
        let type = args[3] as number
        let district = args[4] as number
        showroomMenu.fillMenu(vehicles, playerVehicles, positions, type, district)
        showroomMenu.show()
    }

    function hideShowroomMenu() {
        showroomMenu.hide()
    }
}