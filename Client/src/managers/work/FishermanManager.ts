import { ServerEventHandler } from '../../event-handlers/ServerEventHandler'

namespace FishermanManager {
    let pointMarker: LocalHandle
    let pointBlip: LocalHandle
    let pointLabel: LocalHandle

    ServerEventHandler.getInstance().on('ShowFishermanPoint', showNextPoint)
    ServerEventHandler.getInstance().on('HideFishermanPoint', deleteCurrentPoint)

    API.onKeyDown.connect((sender, args) => {
        let player = API.getLocalPlayer()
        let playerIsFisherman = API.hasEntitySyncedData(player, 'IsFisherman') || API.hasEntitySyncedData(player, 'IsFishermanOnBoat')
        if (!(args.KeyCode == Keys.E && playerIsFisherman)) {
            return
        }        
        if (API.hasEntitySyncedData(player, 'OnFishermanTarget')) {
            if (API.isPlayerInAnyVehicle(player)) {
                API.sendNotification('~r~Необходимо встать на палубу')
                return
            }
            API.resetEntitySyncedData(player, 'OnFishermanTarget')
            API.triggerServerEvent('ProcessFishermanPoint')
            return
        }
        if (API.hasEntitySyncedData(player, 'FishBitesEvent')) {
            API.resetEntitySyncedData(player, 'FishBitesEvent')
            API.triggerServerEvent('FinishFishermanPoint', true)
        }
    })

    function showNextPoint(args: any[]) { 
        deleteCurrentPoint()
        let position = args[0] as Vector3
        let isBoat = args[1] as boolean
        let scale = isBoat ? 5 : 1
        pointMarker = API.createMarker(1, position, new Vector3(), new Vector3(), new Vector3(scale, scale, scale), 0, 137, 216, 150)
        createBlip(position)
        pointLabel = API.createTextLabel('Нажмите ~y~Е', position.Add(new Vector3(0, 0, 1.5)), 10, 0.5)
    }

    function deleteCurrentPoint() {
        if (!pointMarker) {
           return
        }
        API.deleteEntity(pointMarker)
        API.deleteEntity(pointBlip)
        API.deleteEntity(pointLabel)
    }

    function createBlip(position: Vector3) {        
        pointBlip = API.createBlip(position)
        API.setBlipSprite(pointBlip, 1)
        API.setBlipColor(pointBlip, 3)
        API.setBlipName(pointBlip, 'Место ловли')
        API.setBlipFlashing(pointBlip, false)
    }
}