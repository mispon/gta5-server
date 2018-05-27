import { ServerEventHandler } from '../../event-handlers/ServerEventHandler'

namespace FarmerManager {
    let pointMarker: LocalHandle
    let pointBlip: LocalHandle
    let pointLabel: LocalHandle
    let endPointMarker: LocalHandle
    let endPointBlip: LocalHandle

    ServerEventHandler.getInstance().on('ShowFarmerPoint', showNextPoint)
    ServerEventHandler.getInstance().on('HideFarmerPoint', deleteCurrentPoint)
    ServerEventHandler.getInstance().on('ShowFarmerEndPoint', showEndPoint)
    ServerEventHandler.getInstance().on('HideFarmerEndPoint', deleteEndPoint)

    API.onKeyDown.connect((sender, args) => {
        let player = API.getLocalPlayer()
        if (!(args.KeyCode == Keys.E && API.hasEntitySyncedData(player, 'OnFarmerTarget'))) {
            return
        }
        API.resetEntitySyncedData(player, 'OnFarmerTarget')
        API.triggerServerEvent('ProcessFarmerPoint')
    })

    function showNextPoint(args: any[]) { 
        deleteCurrentPoint()
        let position = args[0] as Vector3
        pointMarker = API.createMarker(1, position, new Vector3(), new Vector3(), new Vector3(1, 1, 0.7), 112, 249, 165, 150)
        pointBlip = createBlip(position, 69, 'Точка сбора')
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

    function showEndPoint(args: any[]) {
        let position = args[0] as Vector3
        endPointMarker = API.createMarker(1, position, new Vector3(), new Vector3(), new Vector3(2, 2, 1), 0, 137, 216, 150)
        endPointBlip = createBlip(position, 3, 'Сдача урожая')        
    }

    function deleteEndPoint() {
        if (!endPointMarker) {
           return
        }
        API.deleteEntity(endPointMarker)
        API.deleteEntity(endPointBlip)
    }

    function createBlip(position: Vector3, color: number, name: string) {        
        let result = API.createBlip(position)
        API.setBlipSprite(result, 1)
        API.setBlipColor(result, color)
        API.setBlipName(result, name)
        return result
    }
}